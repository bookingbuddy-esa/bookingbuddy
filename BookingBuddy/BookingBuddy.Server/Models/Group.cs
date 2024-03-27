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
        /// <summary>
        /// Identificador do grupo.
        /// </summary>
        [Key]
        [JsonPropertyName("groupId")]
        public string GroupId {  get; set; }

        /// <summary>
        ///  Identificador do dono do grupo.
        /// </summary>
        [JsonPropertyName("groupOwnerId")]
        public string GroupOwnerId { get; set; }

        /// <summary>
        /// Nome do grupo.
        /// </summary>
        [Required]
        [StringLength(25)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Lista de identificadores dos utilizadores (membros) que fazem parte do grupo.
        /// </summary>
        [JsonPropertyName("membersId")]
        public List<string> MembersId { get; set; }

        /// <summary>
        /// Lista de identificadores das propriedades que vão ser votadas.
        /// </summary>
        [JsonPropertyName("propertiesId")]
        public List<string> PropertiesId {  get; set; }

        /// <summary>
        /// Identificador da propriedade escolhida.
        /// </summary>
        [JsonPropertyName("choosenProperty")]
        public string? ChoosenProperty { get; set; } 

        /// <summary>
        /// Lista de propriedades que vão ser votadas.
        /// </summary>
        [JsonPropertyName("properties")]
        public List<Property>? Properties { get; set; }  

        /// <summary>
        /// Lista de utilizadores (membros) que fazem parte do grupo.
        /// </summary>
        [JsonPropertyName("members")]
        public List<ReturnUser>? Members {  get; set; }

        /// <summary>
        /// Lista de identificadores das mensagens do grupo.
        /// </summary>
        [JsonPropertyName("messagesId")]
        public List<string> MessagesId { get; set; }

        /// <summary>
        /// Lista de mensagens do grupo.
        /// </summary>
        [JsonPropertyName("messages")]
        public List<GroupMessage>? Messages { get; set; }

        /// <summary>
        /// Lista de identificadores dos votos nas propriedades.
        /// </summary>
        [JsonPropertyName("votesId")]
        public List<string> VotesId { get; set; }

        /// <summary>
        /// Lista de votos nas propriedades.
        /// </summary>
        [JsonPropertyName("votes")]
        public List<GroupVote>? Votes { get; set; }

        /// <summary>
        /// Estado do grupo (ação - votação, reserva, pagamento)
        /// </summary>
        [JsonPropertyName("groupAction")]
        public GroupAction GroupAction { get; set; }

        /// <summary>
        /// Identificador da reserva do grupo.
        /// </summary>
        [JsonPropertyName("groupBookingId")]
        public string? GroupBookingId { get; set; }
    }


    /// <summary>
    /// Classe que representa as mensagens de chat de um grupo.
    /// </summary>
    public class GroupMessage {

        /// <summary>
        /// Identificador da mensagem.
        /// </summary>
        [Key]
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }

        /// <summary>
        /// Nome do utilizador que enviou a mensagem.
        /// </summary>
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Conteúdo da mensagem.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

    	/// <summary>
        /// Identificador do grupo.
        /// </summary>
        [JsonPropertyName("groupId")]
        public string GroupId {  get; set; }
    }

    /// <summary>
    /// Classe que representa os votos nas propriedades de um grupo.
    /// </summary>
    public class GroupVote
    {
        /// <summary>
        /// Identificador do voto.
        /// </summary>
        [Key]
        [JsonPropertyName("voteId")]
        public string VoteId { get; set; }

        /// <summary>
        /// Identificador da propriedade votada.
        /// </summary>
        [JsonPropertyName("propertyId")]
        public string PropertyId { get; set; }

        /// <summary>
        /// Identificador do utilizador que votou.
        /// </summary>
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Identificador do grupo.
        /// </summary>
        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }
    }

    /// <summary>
    /// Enum que representa as ações (estados) de um grupo.
    /// </summary>
    public enum GroupAction
    {
        /// <summary>
        /// Nenhuma ação.
        /// </summary>
        [JsonPropertyName("none")]
        None,

        /// <summary>
        /// Votação.
        /// </summary>
        [JsonPropertyName("voting")]
        Voting,

        /// <summary>
        /// Reserva.
        /// </summary>
        [JsonPropertyName("booking")]
        Booking,

        /// <summary>
        /// Pagamento.
        /// </summary>
        [JsonPropertyName("paying")]
        Paying
    }

    /// <summary>
    /// Classe de extensão para o enumerado <see cref="GroupAction"/>.
    /// </summary>
    public static class GroupActionExtension
    {
        /// <summary>
        /// Método que retorna o nome de uma ação de um grupo.
        /// </summary>
        /// <param name="groupAction"></param>
        /// <returns></returns>
        public static string AsString(this GroupAction groupAction) => groupAction switch
        {
            GroupAction.None => "Nenhuma",
            GroupAction.Voting => "Votação",
            GroupAction.Booking => "Reserva",
            GroupAction.Paying => "Pagamento",
            _ => "Nenhuma"
        };
    }
}
