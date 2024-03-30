using System.Net.WebSockets;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Authorization;
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
    /// Cria um novo chat com o nome especificado.
    /// </summary>
    /// <param name="name">O nome do novo chat a ser criado.</param>
    /// <returns>
    /// Um objeto contendo o identificador único e o nome do chat criado se for bem-sucedido.
    /// Um código de estado 200 (OK) se o chat for criado com sucesso.
    /// Um código de estado 401 (Não Autorizado) se o utilizador não estiver autenticado.
    /// Um código de estado 400 (Pedido Inválido) se ocorrerem erros durante o processo.
    /// </returns>
    [HttpPost]
    [Authorize]
    [Route("create")]
    public async Task<IActionResult> CreateChat([FromBody] string name)
    {
        var chat = new Chat
        {
            ChatId = Guid.NewGuid().ToString(),
            Name = name,
        };
        _context.Chat.Add(chat);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            return BadRequest();
        }

        return Ok(new
        {
            chat.ChatId,
            chat.Name
        });
    }

    /// <summary>
    /// Obtém informações sobre um chat específico, incluindo os últimos 10 mensagens enviadas.
    /// </summary>
    /// <param name="chatId">O identificador único do chat.</param>
    /// <returns>
    /// Um objeto contendo informações sobre o chat, incluindo os últimos 10 mensagens, se for bem-sucedido.
    /// Um código de estado 200 (OK) se as informações do chat forem obtidas com sucesso.
    /// Um código de estado 404 (Não Encontrado) se o chat com o ID especificado não existir.
    /// </returns>
    [HttpGet]
    [Route("{chatId}")]
    public async Task<IActionResult> GetChat(string chatId)
    {
        var chat = await _context.Chat.FirstOrDefaultAsync(c => c.ChatId == chatId);
        if (chat == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            chat.ChatId,
            chat.Name,
            LastMessages = await _context.ChatMessage
                .Include(m => m.ApplicationUser)
                .Where(m => chat.MessageIds.Contains(m.MessageId))
                .Select(m => new
                {
                    m.MessageId,
                    ApplicationUser = m.ApplicationUser != null
                        ? new
                        {
                            m.ApplicationUser.Id,
                            m.ApplicationUser.Name,
                        }
                        : null,
                    m.Content,
                    m.SentAt
                })
                .OrderByDescending(m => m.SentAt)
                .Take(10)
                .ToListAsync()
        });
    }

    /// <summary>
    /// Obtém as mensagens de um chat específico, com a opção de especificar o número de mensagens e o índice de início.
    /// </summary>
    /// <param name="chatId">O identificador único do chat.</param>
    /// <param name="numberOfMessages">O número de mensagens a serem retornadas. O padrão é 10.</param>
    /// <param name="startIndex">O índice de início das mensagens a serem retornadas. O padrão é 0.</param>
    /// <returns>
    /// Uma lista de mensagens do chat, com a opção de especificar o número de mensagens e o índice de início, se for bem-sucedido.
    /// Um código de estado 200 (OK) se as mensagens do chat forem obtidas com sucesso.
    /// Um código de estado 404 (Não Encontrado) se o chat com o ID especificado não existir.
    /// </returns>
    [HttpGet]
    [Route("messages/{chatId}")]
    public async Task<IActionResult> GetChatMessages(string chatId, [FromQuery] int numberOfMessages = 10,
        [FromQuery] int startIndex = 0)
    {
        var chat = await _context.Chat.FindAsync(chatId);
        if (chat == null)
        {
            return NotFound();
        }

        return Ok(await _context.ChatMessage
            .Include(m => m.ApplicationUser)
            .Where(m => chat.MessageIds.Contains(m.MessageId))
            .Select(m => new
            {
                m.MessageId,
                ApplicationUser = m.ApplicationUser != null
                    ? new
                    {
                        m.ApplicationUser.Id,
                        m.ApplicationUser.Name,
                    }
                    : null,
                m.Content,
                m.SentAt
            })
            .OrderByDescending(m => m.SentAt)
            .Skip(startIndex)
            .Take(10)
            .ToListAsync());
    }

    /// <summary>
    /// Método que lida com a conexão de um WebSocket.
    /// </summary>
    /// <param name="chatId">Identificador único do chat.</param>
    /// <param name="userId">Identificador único do utilizador.</param>
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
            var msg = _context.ChatMessage.Add(new ChatMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                Content = message.Message.Content,
                SentAt = DateTime.Now
            }).Entity;
            var chat = await _context.Chat.FindAsync(chatId);
            if (chat == null) return;
            chat.MessageIds.Add(msg.MessageId);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }

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
                            ApplicationUser = new
                            {
                                applicationUser.Id,
                                applicationUser.Name,
                            },
                            message.Message.Content,
                            SentAt = DateTime.Now,
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