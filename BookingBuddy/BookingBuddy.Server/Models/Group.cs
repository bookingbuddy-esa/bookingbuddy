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
    public class Group
    {
        [Key] [JsonPropertyName("groupId")] public required string GroupId { get; set; }

        [JsonPropertyName("groupOwnerId")] public required string GroupOwnerId { get; set; }

        [Required]
        [MaxLength(25)]
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("membersId")] public List<string> MembersId { get; set; }

        [JsonPropertyName("addedPropertyIds")] public List<string> AddedPropertyIds { get; set; } = [];

        [JsonPropertyName("chosenProperty")] public string? ChosenProperty { get; set; }

        [JsonPropertyName("chatId")] public string? ChatId { get; set; }

        [JsonPropertyName("chat")] public Chat? Chat { get; set; }

        [JsonPropertyName("addedProperties")] public List<UserAddedProperty>? AddedProperties { get; set; } = [];

        [JsonPropertyName("members")] public List<ReturnUser>? Members { get; set; }

        [JsonPropertyName("groupAction")] public GroupAction GroupAction { get; set; }

        [JsonPropertyName("groupBookingId")] public string? GroupBookingId { get; set; }
    }

    public class UserAddedProperty
    {
        [Key]
        [JsonPropertyName("userAddedPropertyId")]
        public required string UserAddedPropertyId { get; set; }

        [JsonPropertyName("userId")] public required string ApplicationUserId { get; set; }

        [JsonPropertyName("propertyId")] public required string PropertyId { get; set; }

        [JsonPropertyName("ApplicationUser")] public ApplicationUser? ApplicationUser { get; set; }

        [JsonPropertyName("Property")] public Property? Property { get; set; }
    }

    /// <summary>
    /// Classe que representa as mensagens de chat de um grupo.
    /// </summary>
    public class GroupMessage
    {
        [Key] [JsonPropertyName("messageId")] public string MessageId { get; set; }

        [JsonPropertyName("userName")] public string UserName { get; set; }

        [JsonPropertyName("message")] public string Message { get; set; }

        [JsonPropertyName("groupId")] public string GroupId { get; set; }
    }

    /// <summary>
    /// Enum que representa as ações (estados) de um grupo.
    /// </summary>
    public enum GroupAction
    {
        [JsonPropertyName("none")] None,
        [JsonPropertyName("voting")] Voting,
        [JsonPropertyName("booking")] Booking,
        [JsonPropertyName("paying")] Paying
    }

    public static class GroupActionExtension
    {
        public static string AsString(this GroupAction groupAction) => groupAction switch
        {
            GroupAction.Voting => "Voting",
            GroupAction.Booking => "Booking",
            GroupAction.Paying => "Paying",
            _ => "None"
        };

        public static string GetDescription(this GroupAction groupAction) => groupAction switch
        {
            GroupAction.Voting => "A votar",
            GroupAction.Booking => "A reservar",
            GroupAction.Paying => "A pagar",
            _ => "Nenhuma ação"
        };
    }
}