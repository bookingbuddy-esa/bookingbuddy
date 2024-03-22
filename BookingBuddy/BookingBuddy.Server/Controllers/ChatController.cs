using System.Net.WebSockets;
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
[Route("api/chat/{chatId}")]
public class ChatController : ControllerBase
{
    private static readonly WebSocketWrapper WebSocketWrapper = new();
    private readonly BookingBuddyServerContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private static readonly Dictionary<string, List<WebSocket>> ChatSockets = new();
    private static readonly Dictionary<WebSocket, ApplicationUser> UserSockets = new();

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
    [Route("messages")]
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
        WebSocketWrapper.AddOnConnectListener(webSocket, async (_, _) =>
        {
            UserSockets.Add(webSocket, user);
            if (ChatSockets.TryGetValue(chatId, out var value))
            {
                foreach (var socket in value)
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "UserConnected",
                        Content = new
                        {
                            user.Id,
                            user.Name,
                        }
                    });
                }

                value.Add(webSocket);
            }
            else
            {
                ChatSockets.Add(chatId, [webSocket]);
            }
        });
        WebSocketWrapper.AddOnReceiveListener(webSocket, async (_, message) =>
        {
            if (message.Message == null) return;
            if (ChatSockets.TryGetValue(chatId, out var value))
            {
                foreach (var socket in value.Where(socket => socket != message.Socket))
                {
                    UserSockets.TryGetValue(socket, out var applicationUser);
                    if (applicationUser == null) return;
                    var userMessage = new SocketMessage
                    {
                        Code = "Message",
                        Content = new
                        {
                            MessageId = Guid.NewGuid().ToString(),
                            message.Message.Content,
                            SentAt = DateTime.Now,
                            ApplicationUser = new
                            {
                                applicationUser.Id,
                                applicationUser.Name,
                            }
                        }
                    };
                    await WebSocketWrapper.SendAsync(socket, userMessage);
                }
            }
        });
        WebSocketWrapper.AddOnCloseListener(webSocket, async (_, _) =>
        {
            if (ChatSockets.TryGetValue(chatId, out var value))
            {
                value.Remove(webSocket);
                UserSockets.TryGetValue(webSocket, out var applicationUser);
                UserSockets.Remove(webSocket);
                if (applicationUser == null) return;
                foreach (var socket in value)
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "UserDisconnected",
                        Content = new
                        {
                            applicationUser.Id,
                            applicationUser.Name,
                        }
                    });
                }
            }
            
        });
        await WebSocketWrapper.HandleAsync(this, webSocket);
    }
}