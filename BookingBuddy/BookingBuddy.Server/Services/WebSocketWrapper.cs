using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookingBuddy.Server.Services;

/// <summary>
/// Serviço que encapsula a lógica de comunicação com WebSockets.
/// </summary>
public class WebSocketWrapper
{
    /// <summary>
    /// Delegado que representa um evento de WebSocket, com argumentos.
    /// </summary>
    /// <typeparam name="TEventArgs">Tipo dos argumentos do evento.</typeparam>
    public delegate Task WebSocketEventHandler<in TEventArgs>(object? sender, TEventArgs e);

    /// <summary>
    /// Delegado que representa um evento de WebSocket, sem argumentos.
    /// </summary>
    public delegate Task WebSocketEventHandler(object? sender, WebSocketEventArgs e);

    private readonly Dictionary<WebSocket, WebSocketEventHandler> _onConnectListeners = new();
    private readonly Dictionary<WebSocket, WebSocketEventHandler> _onReceiveListeners = new();
    private readonly Dictionary<WebSocket, WebSocketEventHandler<Exception>> _onErrorListeners = new();
    private readonly Dictionary<WebSocket, WebSocketEventHandler> _onDisconnectListeners = new();
    private readonly Dictionary<WebSocket, WebSocketEventHandler> _onCloseListeners = new();

    /// <summary>
    /// Lida com uma conexão WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket a ser tratado.</param>
    public async Task HandleAsync(WebSocket socket)
    {
        await HandleAsync(this, socket);
    }

    /// <summary>
    /// Lida com uma conexão WebSocket, especificando o remetente.
    /// </summary>
    /// <param name="sender">Remetente da conexão.</param>
    /// <param name="socket">WebSocket a ser tratado.</param>
    public async Task HandleAsync(object? sender, WebSocket socket)
    {
        try
        {
            _onConnectListeners.TryGetValue(socket, out var onConnect);
            onConnect?.Invoke(sender, new WebSocketEventArgs
            {
                Socket = socket
            });

            var buffer = new byte[1024 * 4];
            var jsonOptions = new JsonSerializerOptions
            {
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            };
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                try
                {
                    var chatMessage = JsonSerializer.Deserialize<SocketMessage>(message, jsonOptions);
                    if (chatMessage != null)
                    {
                        _onReceiveListeners.TryGetValue(socket, out var onReceive);
                        onReceive?.Invoke(sender, new WebSocketEventArgs
                        {
                            Socket = socket,
                            Message = chatMessage.Content is JsonElement { ValueKind: JsonValueKind.String }
                                ? new SocketMessage
                                {
                                    Code = chatMessage.Code,
                                    Content = chatMessage.Content.GetString()
                                }
                                : chatMessage
                        });
                    }
                }
                catch (Exception e)
                {
                    _onErrorListeners.TryGetValue(socket, out var onError);
                    onError?.Invoke(sender, e);
                }

                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            _onDisconnectListeners.TryGetValue(socket, out var onDisconnect);
            onDisconnect?.Invoke(sender, new WebSocketEventArgs
            {
                Socket = socket
            });
            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _onCloseListeners.TryGetValue(socket, out var onClose);
            onClose?.Invoke(sender, new WebSocketEventArgs
            {
                Socket = socket
            });
        }
        catch (WebSocketException)
        {
            _onCloseListeners.TryGetValue(socket, out var onClose);
            onClose?.Invoke(sender, new WebSocketEventArgs
            {
                Socket = socket
            });
        }
        catch (Exception e)
        {
            _onErrorListeners.TryGetValue(socket, out var onError);
            onError?.Invoke(sender, e);
        }

        ClearAllForSocket(socket);
    }

    /// <summary>
    /// Envia uma mensagem para um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket para o qual enviar a mensagem.</param>
    /// <param name="message">Mensagem a enviar.</param>
    public static async Task SendAsync(WebSocket socket, SocketMessage message)
    {
        var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    /// <summary>
    /// Adiciona um ouvinte para o evento de conexão de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    /// <param name="listener">Ouvinte a ser adicionado.</param>
    public void AddOnConnectListener(WebSocket socket, WebSocketEventHandler listener)
    {
        _onConnectListeners.Add(socket, listener);
    }

    /// <summary>
    /// Adiciona um ouvinte para o evento de receção de uma mensagem por um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    /// <param name="listener">Ouvinte a ser adicionado.</param>
    public void AddOnReceiveListener(WebSocket socket, WebSocketEventHandler listener)
    {
        _onReceiveListeners.Add(socket, listener);
    }

    /// <summary>
    /// Adiciona um ouvinte para o evento de erro de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    /// <param name="listener">Ouvinte a ser adicionado.</param>
    public void AddOnErrorListener(WebSocket socket, WebSocketEventHandler<Exception> listener)
    {
        _onErrorListeners.Add(socket, listener);
    }

    /// <summary>
    /// Adiciona um ouvinte para o evento de desconexão de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    /// <param name="listener">Ouvinte a ser adicionado.</param>
    public void AddOnDisconnectListener(WebSocket socket, WebSocketEventHandler listener)
    {
        _onDisconnectListeners.Add(socket, listener);
    }

    /// <summary>
    /// Adiciona um ouvinte para o evento de fecho de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    /// <param name="listener">Ouvinte a ser adicionado.</param>
    public void AddOnCloseListener(WebSocket socket, WebSocketEventHandler listener)
    {
        _onCloseListeners.Add(socket, listener);
    }

    /// <summary>
    /// Remove um ouvinte para o evento de conexão de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    public void RemoveOnConnectListener(WebSocket socket)
    {
        _onConnectListeners.Remove(socket);
    }

    /// <summary>
    /// Remove um ouvinte para o evento de receção de uma mensagem por um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    public void RemoveOnReceiveListener(WebSocket socket)
    {
        _onReceiveListeners.Remove(socket);
    }

    /// <summary>
    /// Remove um ouvinte para o evento de erro de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    public void RemoveOnErrorListener(WebSocket socket)
    {
        _onErrorListeners.Remove(socket);
    }

    /// <summary>
    /// Remove um ouvinte para o evento de desconexão de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    public void RemoveOnDisconnectListener(WebSocket socket)
    {
        _onDisconnectListeners.Remove(socket);
    }

    /// <summary>
    /// Remove um ouvinte para o evento de fecho de um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado ao evento.</param>
    public void RemoveOnCloseListener(WebSocket socket)
    {
        _onCloseListeners.Remove(socket);
    }

    /// <summary>
    /// Remove todos os ouvintes para um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket associado aos ouvintes.</param>
    private void ClearAllForSocket(WebSocket socket)
    {
        _onConnectListeners.Remove(socket);
        _onReceiveListeners.Remove(socket);
        _onErrorListeners.Remove(socket);
        _onDisconnectListeners.Remove(socket);
        _onCloseListeners.Remove(socket);
    }
}

/// <summary>
/// Representa uma mensagem de um WebSocket.
/// </summary>
public class SocketMessage
{
    /// <summary>
    /// Código da mensagem.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    /// <summary>
    /// Conteúdo da mensagem.
    /// </summary>
    [JsonPropertyName("content")]
    public required dynamic Content { get; init; }
}

/// <summary>
/// Representa os argumentos de um evento de WebSocket.
/// </summary>
public class WebSocketEventArgs : EventArgs
{
    /// <summary>
    /// WebSocket associado ao evento.
    /// </summary>
    public required WebSocket Socket { get; init; }

    /// <summary>
    /// Mensagem associada ao evento.
    /// </summary>
    public SocketMessage? Message { get; init; }
}

/// <summary>
/// Exceção lançada quando uma mensagem de WebSocket é inválida.
/// </summary>
public class InvalidSocketMessageException : Exception
{
    /// <summary>
    /// Cria uma nova instância de <see cref="InvalidSocketMessageException"/>.
    /// </summary>
    /// <param name="message">Mensagem de erro.</param>
    public InvalidSocketMessageException(string? message = null) : base(message)
    {
    }
}