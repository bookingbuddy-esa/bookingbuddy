using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookingBuddy.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa os Grupos de Viagem
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

        public string GetPrimaryKey()
        {
            return GroupId;
        }
    }

    public class GroupMessage {
        [Key]
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }
        
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
