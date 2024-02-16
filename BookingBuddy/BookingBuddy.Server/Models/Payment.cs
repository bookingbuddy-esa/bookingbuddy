using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa um pagamento.
    /// </summary>
    public class Payment {
        [Key]
        public string PaymentId { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}