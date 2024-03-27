using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa as mensagens de uma reserva.
    /// </summary>
    public class BookingMessage {
        /// <summary>
        /// Identificador da mensagem.
        /// </summary>
        [Key]
        public string BookingMessageId { get; set; }

        /// <summary>
        /// Identificador da reserva.
        /// </summary>
        public string BookingOrderId { get; set; }

        /// <summary>
        /// Identificador do utilizador que enviou a mensagem.
        /// </summary>
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// Utilizador que enviou a mensagem.
        /// </summary>
        public ApplicationUser? ApplicationUser { get; set; }

        /// <summary>
        /// Conteúdo da mensagem.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Data de envio da mensagem.
        /// </summary>
        public DateTime SentAt { get; set; }
    }
}