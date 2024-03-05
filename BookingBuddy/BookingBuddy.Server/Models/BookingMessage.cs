using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa as mensagens de uma reserva.
    /// </summary>
    public class BookingMessage {
        [Key]
        public string BookingMessageId { get; set; }
        public string BookingOrderId { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}