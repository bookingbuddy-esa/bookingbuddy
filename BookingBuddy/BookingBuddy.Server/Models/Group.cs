using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookingBuddy.Server.Models;

/// <summary>
/// Classe que representa os Grupos de Reserva.
/// </summary>
public class Group
{
    /// <summary>
    /// Propriedade que diz respeito ao identificador do grupo.
    /// </summary>
    [Key]
    [JsonPropertyName("groupId")]
    public required string GroupId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao identificador do dono do grupo.
    /// </summary>
    [JsonPropertyName("groupOwnerId")]
    public required string GroupOwnerId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao nome do grupo.
    /// </summary>
    [Required]
    [MaxLength(25)]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Propriedade que diz respeito aos identificadores dos membros do grupo.
    /// </summary>
    [JsonPropertyName("membersId")]
    public List<string> MembersId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito aos identificadores das propriedades adicionadas ao grupo.
    /// </summary>
    [JsonPropertyName("addedPropertyIds")]
    public List<string> AddedPropertyIds { get; set; } = [];

    /// <summary>
    /// Propriedade que diz respeito aos identificadores dos votos dos utilizadores.
    /// </summary>
    [JsonPropertyName("userVoteIds")]
    public List<string> UserVoteIds { get; set; } = [];

    /// <summary>
    /// Propriedade que diz respeito ao identificador da propriedade escolhida.
    /// </summary>
    [JsonPropertyName("chosenProperty")]
    public string? ChosenProperty { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao identificador do chat do grupo.
    /// </summary>
    [JsonPropertyName("chatId")]
    public string? ChatId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao chat do grupo.
    /// </summary>
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }

    /// <summary>
    /// Propriedade que diz respeito às propriedades adicionadas ao grupo.
    /// </summary>
    [JsonPropertyName("addedProperties")]
    public List<UserAddedProperty>? AddedProperties { get; set; } = [];

    /// <summary>
    /// Propriedade que diz respeito aos votos dos utilizadores.
    /// </summary>
    [JsonPropertyName("userVotes")]
    public List<UserVote>? UserVotes { get; set; } = [];

    /// <summary>
    /// Propriedade que diz respeito aos membros do grupo.
    /// </summary>
    [JsonPropertyName("members")]
    public List<ReturnUser>? Members { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao dono do grupo.
    /// </summary>
    [JsonPropertyName("groupAction")]
    public GroupAction GroupAction { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao identificador da reserva de grupo.
    /// </summary>
    [JsonPropertyName("groupBookingId")]
    public string? GroupBookingId { get; set; }
}

/// <summary>
/// Classe que representa as propriedades adicionadas por um utilizador a um grupo.
/// </summary>
public class UserAddedProperty
{
    /// <summary>
    /// Propriedade que diz respeito ao identificador da propriedade adicionada por um utilizador a um grupo.
    /// </summary>
    [Key]
    [JsonPropertyName("userAddedPropertyId")]
    public required string UserAddedPropertyId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao identificador do utilizador que adicionou a propriedade ao grupo.
    /// </summary>
    [JsonPropertyName("userId")]
    public required string ApplicationUserId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao identificador da propriedade adicionada ao grupo.
    /// </summary>
    [JsonPropertyName("propertyId")]
    public required string PropertyId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao utilizador que adicionou a propriedade ao grupo.
    /// </summary>
    [JsonPropertyName("ApplicationUser")]
    public ApplicationUser? ApplicationUser { get; set; }

    /// <summary>
    /// Propriedade que diz respeito à propriedade adicionada ao grupo.
    /// </summary>
    [JsonPropertyName("Property")]
    public Property? Property { get; set; }
}

/// <summary>
/// Classe que representa o voto de um utilizador numa propriedade.
/// </summary>
public class UserVote
{
    /// <summary>
    /// Propriedade que diz respeito ao identificador do voto de um utilizador numa propriedade.
    /// </summary>
    [Key]
    public required string UserVoteId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao identificador do utilizador que votou numa propriedade.
    /// </summary>
    public required string ApplicationUserId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao identificador da propriedade votada.
    /// </summary>
    public required string PropertyId { get; set; }

    /// <summary>
    /// Propriedade que diz respeito ao utilizador que votou numa propriedade.
    /// </summary>
    public ApplicationUser? ApplicationUser { get; set; }

    /// <summary>
    /// Propriedade que diz respeito à propriedade votada.
    /// </summary>
    public Property? Property { get; set; }
}

/// <summary>
/// Enum que representa as ações (estados) de um grupo.
/// </summary>
public enum GroupAction
{
    /// <summary>
    /// Nenhuma ação.
    /// </summary>
    [JsonPropertyName("none")] None,

    /// <summary>
    /// A votar.
    /// </summary>
    [JsonPropertyName("voting")] Voting,

    /// <summary>
    /// A reservar.
    /// </summary>
    [JsonPropertyName("booking")] Booking,

    /// <summary>
    /// A pagar.
    /// </summary>
    [JsonPropertyName("paying")] Paying
}

/// <summary>
/// Extensão da enumeração GroupAction.
/// </summary>
public static class GroupActionExtension
{
    /// <summary>
    /// Método que retorna a descrição de uma ação de grupo.
    /// </summary>
    /// <param name="groupAction">Ação de grupo</param>
    /// <returns>Descrição da ação de grupo</returns>
    public static string GetDescription(this GroupAction groupAction) => groupAction switch
    {
        GroupAction.Voting => "A votar",
        GroupAction.Booking => "A reservar",
        GroupAction.Paying => "A pagar",
        _ => "Nenhuma ação"
    };
}