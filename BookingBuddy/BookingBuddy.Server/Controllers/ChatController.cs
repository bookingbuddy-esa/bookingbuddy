using System.Net.WebSockets;
using System.Text.Json;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddy.Server.Controllers;

/// <summary>
/// Classe que representa o controlador de chat.
/// </summary>
[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private static readonly WebSocketWrapper WebSocketWrapper = new();
    private readonly BookingBuddyServerContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private static readonly Dictionary<string, List<WebSocket>> ChatSockets = new();

    /// <summary>
    /// Classe que representa o controlador de chat.
    /// </summary>
    public ChatController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Método que obtém um chat.
    /// </summary>
    /// <param name="chatId">Identificador do chat.</param>
    /// <returns>
    /// Retorna um IActionResult indicando o resultado da operação</returns>
    [HttpGet]
    [Route("{chatId}")]
    public async Task<IActionResult> GetChat(string chatId)
    {
        var chat = await _context.Chat.FindAsync(chatId);
        return Ok(chat);
    }

    /// <summary>
    /// Método que obtém as mensagens de um chat.
    /// </summary>
    /// <param name="chatId">Identificador do chat.</param>
    /// <param name="numberOfMessages">Número de mensagens a obter.</param>
    /// <returns>
    /// Retorna um IActionResult indicando o resultado da operação</returns>
    [HttpGet]
    [Route("{chatId}/messages")]
    public async Task<IActionResult> GetChatMessages(string chatId, [FromQuery] int numberOfMessages = 10)
    {
        var chat = await _context.Chat.FindAsync(chatId);
        if (chat == null)
        {
            return NotFound();
        }

        return Ok(await _context.ChatMessage
            .Where(m => chat.MessageIds.Contains(m.MessageId))
            .OrderByDescending(m => m.SentAt)
            .Take(numberOfMessages)
            .ToListAsync());
    }

    /// <summary>
    /// Método que lida com a conexão de um WebSocket.
    /// </summary>
    /// <param name="chatId">Identificador do chat.</param>
    /// <param name="userId">Identificador do utilizador.</param>
    /// <param name="webSocket">WebSocket a ser tratado.</param>
    [NonAction]
    public async Task HandleWebSocketAsync(string chatId, string userId, WebSocket webSocket)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return;
        WebSocketWrapper.OnConnect = async (_, _) =>
        {
            Console.WriteLine($"User \"{user.Name}\" connected.");
            if (ChatSockets.TryGetValue(chatId, out var value))
            {
                value.Add(webSocket);
                foreach (var socket in value.Where(s => s != webSocket))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "UserConnected",
                        Content = $"User \"{user.Name}\" connected."
                    });
                }
            }
            else
            {
                ChatSockets.Add(chatId, [webSocket]);
            }
        };
        WebSocketWrapper.OnReceive = (sender, message) =>
        {
            Console.WriteLine($"{sender?.GetType().Name} received: {JsonSerializer.Serialize(message)}");
            return Task.CompletedTask;
        };
        WebSocketWrapper.OnDisconnect = (_, _) =>
        {
            Console.WriteLine($"User \"{user.Name}\" disconnected.");
            return Task.CompletedTask;
        };
        WebSocketWrapper.OnClose = (_, _) =>
        {
            Console.WriteLine($"User \"{user.Name}\" closed the connection.");
            if (ChatSockets.TryGetValue(chatId, out var value))
                value.Remove(webSocket);
            return Task.CompletedTask;
        };
        await WebSocketWrapper.HandleAsync(this, webSocket);
    }
}