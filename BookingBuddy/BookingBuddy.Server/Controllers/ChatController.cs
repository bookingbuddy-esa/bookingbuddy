﻿using System.Net.WebSockets;
using System.Text.Json;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddy.Server.Controllers;

/// <summary>
/// Classe que representa o controlador de chat.
/// </summary>
[ApiController]
[Route("api/chat")]
public class ChatController(BookingBuddyServerContext context) : ControllerBase
{
    private static readonly WebSocketWrapper<Chat> WebSocketWrapper = new();

    [HttpGet]
    [Route("{chatId}")]
    public async Task<IActionResult> GetChat(string chatId)
    {
        var chat = await context.Chat.FindAsync(chatId);
        return Ok(chat);
    }

    [HttpGet]
    [Route("{chatId}/messages")]
    public async Task<IActionResult> GetChatMessages(string chatId, [FromQuery] int numberOfMessages = 10)
    {
        var chat = await context.Chat.FindAsync(chatId);
        if (chat == null)
        {
            return NotFound();
        }

        return Ok(await context.ChatMessage
            .Where(m => chat.MessageIds.Contains(m.MessageId))
            .OrderByDescending(m => m.SentAt)
            .Take(numberOfMessages)
            .ToListAsync());
    }

    [NonAction]
    public async Task HandleWebSocketAsync(string chatId, WebSocket webSocket)
    {
        var chat = await context.Chat.FindAsync(chatId);
        if (chat == null)
        {
            return;
        }

        await WebSocketWrapper.HandleAsync(chat, webSocket, onReceive: async message =>
        {
            Console.WriteLine(message);
            var chatMessage = JsonSerializer.Deserialize<ChatMessage>(message);
            if (chatMessage == null)
            {
                return;
            }

            var sockets = WebSocketWrapper.GetSocketsTrackingById(chatId).Where(w => w != webSocket);
            foreach (var socket in sockets)
            {
                await WebSocketWrapper.SendAsync(socket, chatMessage);
            }
        }, checkType: false);
    }
}