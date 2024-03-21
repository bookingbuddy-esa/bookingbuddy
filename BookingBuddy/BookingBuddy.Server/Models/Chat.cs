using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BookingBuddy.Server.Services;

namespace BookingBuddy.Server.Models;

/// <summary>
/// Classe que representa um chat.
/// </summary>
public class Chat : IPrimaryKey
{
    /// <summary>
    /// Identificador do chat.
    /// </summary>
    [Key] public required string ChatId { get; init; }
    
    /// <summary>
    /// Nome do chat.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Lista de identificadores de mensagens.
    /// </summary>
    public List<string> MessageIds { get; set; } = [];
    
    /// <summary>
    /// Lista de mensagens.
    /// </summary>
    public List<ChatMessage>? ChatMessages { get; set; } = [];
    
    /// <summary>
    /// Obtém a chave primária da entidade.
    /// </summary>
    /// <returns>A chave primária da entidade.</returns>
    public string GetPrimaryKey()
    {
        return ChatId;
    }
}

/// <summary>
/// Classe que representa uma mensagem de chat.
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Identificador da mensagem.
    /// </summary>
    [Key] 
    [JsonPropertyName("messageId")]
    public required string MessageId { get; set; }
    
    /// <summary>
    /// Identificador do utilizador que enviou a mensagem.
    /// </summary>
    [JsonPropertyName("applicationUserId")]
    public required string ApplicationUserId { get; set; }
    
    /// <summary>
    /// Conteúdo da mensagem.
    /// </summary>
    [JsonPropertyName("content")]
    public required string Content { get; set; }
    
    /// <summary>
    /// Data de envio da mensagem.
    /// </summary>
    [JsonPropertyName("sentAt")]
    public required DateTime SentAt { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Utilizador que enviou a mensagem.
    /// </summary>
    public ApplicationUser? ApplicationUser { get; set; }
}