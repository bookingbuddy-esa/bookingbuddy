using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookingBuddy.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa os Grupos de Reserva.
    /// </summary>
    public class Group : IPrimaryKey
    {
        [Key]
        [JsonPropertyName("groupId")]
        public string GroupId {  get; set; }

        [JsonPropertyName("groupOwnerId")]
        public string GroupOwnerId { get; set; }

        [Required]
        [StringLength(25)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("membersId")]
        public List<string> MembersId { get; set; }

        [JsonPropertyName("propertiesId")]
        public List<string> PropertiesId {  get; set; }

        [JsonPropertyName("choosenProperty")]
        public string? ChoosenProperty { get; set; } 

        [JsonPropertyName("properties")]
        public List<Property>? Properties { get; set; }  

        [JsonPropertyName("members")]
        public List<ReturnUser>? Members {  get; set; }

        [JsonPropertyName("messagesId")]
        public List<string> MessagesId { get; set; }

        [JsonPropertyName("messages")]
        public List<GroupMessage>? Messages { get; set; }

        [JsonPropertyName("groupAction")]
        public GroupAction GroupAction { get; set; }

        public string GetPrimaryKey()
        {
            return GroupId;
        }
    }


    /// <summary>
    /// Classe que representa as mensagens de chat de um grupo.
    /// </summary>
    public class GroupMessage {
        [Key]
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }
        
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("groupId")]
        public string GroupId {  get; set; }
    }

    /// <summary>
    /// Enum que representa as ações (estados) de um grupo.
    /// </summary>
    public enum GroupAction
    {
        [JsonPropertyName("none")]
        None,
        [JsonPropertyName("voting")]
        Voting,
        [JsonPropertyName("paying")]
        Paying
    }

    public static class GroupActionExtension
    {
        public static string AsString(this GroupAction groupAction) => groupAction switch
        {
            GroupAction.None => "Nenhuma",
            GroupAction.Voting => "Votação",
            GroupAction.Paying => "Pagamento",
            _ => "Nenhuma"
        };
    }
}
