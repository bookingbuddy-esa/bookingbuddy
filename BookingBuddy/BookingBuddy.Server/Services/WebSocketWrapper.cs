using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace BookingBuddy.Server.Services;

/// <summary>
/// Serviço que encapsula a lógica de comunicação com WebSockets.
/// </summary>
/// <typeparam name="T">Tipo de objeto que será rastreado.</typeparam>
public class WebSocketWrapper<T> where T : IPrimaryKey
{
    private readonly Dictionary<string, WebSocket> _sockets = new();

    private readonly Dictionary<T, List<WebSocket>> _trackingObject = new();

    /// <summary>
    /// Método que retorna todos os WebSockets que se encontram a rastrear um objeto, dado o seu identificador.
    /// </summary>
    /// <param name="trackingObjectId">Identificador do objeto que está a ser rastreado.</param>
    /// <returns>Lista de WebSockets que se encontram a rastrear o objeto.</returns>
    public List<WebSocket> GetSocketsTrackingById(string trackingObjectId)
    {
        var trackingObject = _trackingObject.FirstOrDefault(to => to.Key.GetPrimaryKey() == trackingObjectId);
        return trackingObject.Value;
    }

    /// <summary>
    /// Método que lida com a conexão de um WebSocket com tipo específico no recebimento de mensagens.
    /// </summary>
    /// <param name="objectToTrack">Objeto que será rastreado.</param>
    /// <param name="socket">WebSocket que se conectou.</param>
    /// <param name="onReceive">Função que será executada quando o WebSocket receber uma mensagem.</param>
    /// <param name="checkType">Indica se a mensagem recebida deve ser verificada se é do tipo do objeto rastreado.</param>
    public async Task HandleAsync(T? objectToTrack, WebSocket socket,
        Func<dynamic, Task>? onConnect = null,
        Func<dynamic, Task>? onReceive = null,
        Func<dynamic, Task>? onDisconnect = null,
        bool checkType = true)
    {
        if (objectToTrack == null)
        {
            socket.Abort();
            return;
        }

        try
        {
            var socketId = Guid.NewGuid().ToString();
            _sockets.Add(socketId, socket);
            var socketObjectPair =
                _trackingObject.FirstOrDefault(to => to.Key.GetPrimaryKey() == objectToTrack.GetPrimaryKey());
            if (socketObjectPair.Value != null)
            {
                socketObjectPair.Value.Add(socket);
            }
            else
            {
                _trackingObject.Add(objectToTrack, [socket]);
            }

            Console.WriteLine(
                $"{typeof(T).Name} WebSocket connected ({_sockets.Count - 1} -> {_sockets.Count}): {socketId}");
            Console.WriteLine($"Tracking {typeof(T).Name}: {objectToTrack.GetPrimaryKey()}");

            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
                if (onReceive != null && result.MessageType == WebSocketMessageType.Text)
                {
                    try
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        if (checkType)
                        {
                            var receivedObject = JsonSerializer.Deserialize<T>(message);
                            if (receivedObject != null)
                            {
                                await onReceive(receivedObject);
                            }
                        }
                        else
                        {
                            await onReceive(message);
                        }

                        continue;
                    }
                    catch
                    {
                        Console.WriteLine(
                            $"{typeof(T).Name} WebSocket ({socketId}) received an invalid message.");
                    }
                }

                if (result.MessageType != WebSocketMessageType.Close) continue;
                await socket.CloseAsync(result.CloseStatus!.Value, result.CloseStatusDescription, default);
                break;
            }

            _sockets.Remove(socketId);
            _trackingObject.Where(to => to.Value.Contains(socket)).ToList().ForEach(to => to.Value.Remove(socket));

            Console.WriteLine(
                $"{typeof(T).Name} WebSocket disconnected ({_sockets.Count + 1} -> {_sockets.Count}): {socketId}");
        }
        catch
        {
            await RemoveSocketAsync(socket);
        }
    }

    /// <summary>
    /// Método que notifica todos os WebSockets que se encontram a rastrear um objeto.
    /// </summary>
    /// <param name="trackedObject">Objeto que se encontra a ser rastreado.</param>
    public async Task NotifyAllAsync(T? trackedObject)
    {
        if (trackedObject == null) return;
        var socketTracking =
            _trackingObject.FirstOrDefault(sp => sp.Key.GetPrimaryKey() == trackedObject.GetPrimaryKey());
        if (socketTracking.Value == null) return;
        foreach (var socket in socketTracking.Value)
        {
            var socketId = _sockets.FirstOrDefault(s => s.Value == socket).Key;
            Console.WriteLine(
                $"Notifying {typeof(T).Name} socket ({socketId}) with {typeof(T).Name}: {trackedObject.GetPrimaryKey()}");
            await socket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(trackedObject)),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    /// <summary>
    /// Método que envia uma mensagem para um WebSocket.
    /// </summary>
    /// <param name="socket">WebSocket que irá receber a mensagem.</param>
    /// <param name="message">Mensagem que será enviada.</param>
    public async Task SendAsync(WebSocket socket, dynamic message)
    {
        var socketId = _sockets.FirstOrDefault(s => s.Value == socket).Key;
        Console.WriteLine($"Sending message to {typeof(T).Name} WebSocket ({socketId})");
        await socket.SendAsync(
            message is string
                ? Encoding.UTF8.GetBytes(message)
                : Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)), WebSocketMessageType.Text,
            true, CancellationToken.None);
    }

    private Task RemoveSocketAsync(WebSocket socket)
    {
        var socketId = _sockets.FirstOrDefault(s => s.Value == socket).Key;
        _sockets.Remove(socketId);
        var socketObject = _trackingObject.FirstOrDefault(sp => sp.Value.Contains(socket));
        if (socketObject.Value.Count == 1)
        {
            _trackingObject.Remove(socketObject.Key);
        }
        else
        {
            socketObject.Value.Remove(socket);
        }

        Console.WriteLine(
            $"{typeof(T).Name} WebSocket forced disconnect ({_sockets.Count + 1} -> {_sockets.Count}): {socketId}");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Interface que representa um objeto que possui uma chave primária.
/// </summary>
public interface IPrimaryKey
{
    /// <summary>
    /// Método que retorna a chave primária do objeto.
    /// </summary>
    /// <returns>Chave primária do objeto.</returns>
    public string GetPrimaryKey();
}