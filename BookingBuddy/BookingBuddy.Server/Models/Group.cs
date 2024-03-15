using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookingBuddy.Server.Services;

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

        [JsonPropertyName("members")]
        public List<string> Members { get; set; }

        [JsonPropertyName("properties")]
        public List<string> Properties {  get; set; }

        [JsonPropertyName("choosenProperty")]
        public string? ChoosenProperty { get; set; } 
        public string GetPrimaryKey()
        {
            return GroupId;
        }

    }
}
