using System.Net.WebSockets;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
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
public class ChatController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager)
    : ControllerBase
{
    private static readonly WebSocketWrapper WebSocketWrapper = new();
    private static readonly Dictionary<string, List<WebSocket>> ChatSockets = new();

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
        context.Chat.Add(chat);
        try
        {
            await context.SaveChangesAsync();
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
        var chat = await context.Chat.FirstOrDefaultAsync(c => c.ChatId == chatId);
        if (chat == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            chatId = chat.ChatId,
            name = chat.Name,
            lastMessages = await context.ChatMessage
                .Include(m => m.ApplicationUser)
                .Where(m => chat.MessageIds.Contains(m.MessageId))
                .Select(m => new
                {
                    messageId = m.MessageId,
                    user = m.ApplicationUser != null
                        ? new
                        {
                            m.ApplicationUser.Id,
                            m.ApplicationUser.Name,
                        }
                        : null,
                    content = m.Content,
                    sentAt = m.SentAt
                })
                .OrderByDescending(m => m.sentAt)
                .Take(10)
                .ToListAsync()
        });
    }

    /// <summary>
    /// Obtém as mensagens de um chat específico, com a opção de especificar o número de mensagens e o índice de início.
    /// </summary>
    /// <param name="chatId">O identificador único do chat.</param>
    /// <param name="limit">O número de mensagens a serem retornadas. O padrão é 10.</param>
    /// <param name="offset">O índice de início das mensagens a serem retornadas. O padrão é 0.</param>
    /// <returns>
    /// Uma lista de mensagens do chat, com a opção de especificar o número de mensagens e o índice de início, se for bem-sucedido.
    /// Um código de estado 200 (OK) se as mensagens do chat forem obtidas com sucesso.
    /// Um código de estado 404 (Não Encontrado) se o chat com o ID especificado não existir.
    /// </returns>
    [HttpGet]
    [Route("messages/{chatId}")]
    public async Task<IActionResult> GetChatMessages(string chatId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var chat = await context.Chat.FindAsync(chatId);
        if (chat == null)
        {
            return NotFound();
        }

        return Ok(await context.ChatMessage
            .Include(m => m.ApplicationUser)
            .Where(m => chat.MessageIds.Contains(m.MessageId))
            .OrderByDescending(m => m.SentAt)
            .Skip(offset)
            .Take(limit)
            .Select(m => new
            {
                messageId = m.MessageId,
                user = m.ApplicationUser != null
                    ? new
                    {
                        m.ApplicationUser.Id,
                        m.ApplicationUser.Name,
                    }
                    : null,
                content = m.Content,
                sentAt = m.SentAt
            })
            .ToListAsync());
    }

    /// <summary>
    /// Método que lida com a conexão de um WebSocket.
    /// </summary>
    /// <param name="chatId">Identificador único do chat.</param>
    /// <param name="webSocket">WebSocket a ser tratado.</param>
    [NonAction]
    [Authorize]
    public async Task HandleWebSocketAsync(string chatId, WebSocket webSocket)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return;
        WebSocketWrapper.AddOnConnectListener(webSocket, async (_, _) =>
        {
            if (ChatSockets.TryGetValue(chatId, out var sockets))
            {
                foreach (var socket in sockets.Where(socket => socket != webSocket))
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "UserConnected",
                        Content = new
                        {
                            chatId,
                            user = new
                            {
                                user.Id,
                                user.Name
                            }
                        }
                    });
                }

                sockets.Add(webSocket);
            }
            else
            {
                ChatSockets.Add(chatId, [webSocket]);
            }
        });
        WebSocketWrapper.AddOnReceiveListener(webSocket, async (_, message) =>
        {
            if (message.Message == null) return;
            var chat = await context.Chat.FindAsync(chatId);
            if (chat == null) return;
            var msg = context.ChatMessage.Add(new ChatMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                Content = message.Message.Content,
                SentAt = DateTime.Now
            }).Entity;
            chat.MessageIds.Add(msg.MessageId);
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return;
            }

            if (ChatSockets.TryGetValue(chatId, out var value))
            {
                foreach (var socket in value)
                {
                    var userMessage = new SocketMessage
                    {
                        Code = "UserMessage",
                        Content = new
                        {
                            messageId = msg.MessageId,
                            user = new
                            {
                                id = user.Id,
                                name = user.Name,
                            },
                            content = msg.Content,
                            sentAt = msg.SentAt
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
                foreach (var socket in value)
                {
                    await WebSocketWrapper.SendAsync(socket, new SocketMessage
                    {
                        Code = "UserDisconnected",
                        Content = new
                        {
                            user.Id,
                            user.Name,
                        }
                    });
                }
            }
        });
        await WebSocketWrapper.HandleAsync(this, webSocket);
    }
}