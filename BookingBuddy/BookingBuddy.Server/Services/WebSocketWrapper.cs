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
    public delegate Task WebSocketEventHandler(object? sender, EventArgs e);

    /// <summary>
    /// Evento que é invocado quando uma conexão é estabelecida.
    /// </summary>
    public WebSocketEventHandler? OnConnect;

    /// <summary>
    /// Evento que é invocado quando uma mensagem é recebida.
    /// </summary>
    public WebSocketEventHandler<SocketMessage>? OnReceive;

    /// <summary>
    /// Evento que é invocado quando ocorre um erro.
    /// </summary>
    public WebSocketEventHandler<Exception>? OnError;

    /// <summary>
    /// Evento que é invocado quando uma conexão é terminada.
    /// </summary>
    public WebSocketEventHandler? OnDisconnect;

    /// <summary>
    /// Evento que é invocado quando a conexão é fechada.
    /// </summary>
    public WebSocketEventHandler? OnClose;

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
            OnConnect?.Invoke(sender, EventArgs.Empty);

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
                        OnReceive?.Invoke(sender, chatMessage);
                    }
                }
                catch (Exception e)
                {
                    OnError?.Invoke(sender, e);
                }

                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            OnDisconnect?.Invoke(sender, EventArgs.Empty);
            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            OnClose?.Invoke(sender, EventArgs.Empty);
        }
        catch (WebSocketException)
        {
            OnClose?.Invoke(sender, EventArgs.Empty);
        }
        catch (Exception e)
        {
            OnError?.Invoke(sender, e);
        }
    }

    /// <summary>
    /// Envia uma mensagem para um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket para o qual enviar a mensagem.</param>
    /// <param name="message">Mensagem a enviar.</param>
    public async Task SendAsync(WebSocket socket, SocketMessage message)
    {
        var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
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
    public required string Content { get; init; }
}