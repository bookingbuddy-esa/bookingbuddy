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
    /// Método que lida com a conexão de um WebSocket.
    /// </summary>
    /// <param name="objectToTrack">Objeto que será rastreado.</param>
    /// <param name="socket">WebSocket que se conectou.</param>
    /// <param name="onReceive">Função que será executada quando o WebSocket receber uma mensagem.</param>
    public async Task HandleAsync(T? objectToTrack, WebSocket socket, Func<T, Task>? onReceive = null)
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
                $"{objectToTrack.GetType().Name} WebSocket connected ({_sockets.Count - 1} -> {_sockets.Count}): {socketId}");
            Console.WriteLine($"Tracking {objectToTrack.GetType().Name}: {objectToTrack.GetPrimaryKey()}");

            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
                if (onReceive != null && result.MessageType == WebSocketMessageType.Text)
                {
                    try
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var receivedObject = JsonSerializer.Deserialize<T>(message);
                        if (receivedObject != null)
                        {
                            await onReceive(receivedObject);
                        }

                        continue;
                    }
                    catch
                    {
                        Console.WriteLine($"{objectToTrack.GetType().Name} WebSocket ({socketId}) received an invalid message.");
                    }
                }

                if (result.MessageType != WebSocketMessageType.Close) continue;
                await socket.CloseAsync(result.CloseStatus!.Value, result.CloseStatusDescription, default);
                break;
            }

            _sockets.Remove(socketId);
            _trackingObject.Where(to => to.Value.Contains(socket)).ToList().ForEach(to => to.Value.Remove(socket));

            Console.WriteLine(
                $"{objectToTrack.GetType().Name} WebSocket disconnected ({_sockets.Count + 1} -> {_sockets.Count}): {socketId}");
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
        if(socketTracking.Value == null) return;
        foreach (var socket in socketTracking.Value)
        {
            var socketId = _sockets.FirstOrDefault(s => s.Value == socket).Key;
            Console.WriteLine(
                $"Notifying {trackedObject.GetType().Name} socket ({socketId}) with {trackedObject.GetType().Name}: {trackedObject.GetPrimaryKey()}");
            await socket.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(trackedObject)),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
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
            $"{socketObject.Key.GetType().Name} WebSocket forced disconnect ({_sockets.Count + 1} -> {_sockets.Count}): {socketId}");
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