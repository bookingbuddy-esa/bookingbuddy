using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingBuddy.Server.Services;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa um pagamento.
    /// </summary>
    public class Payment
    {
        [Key] public string PaymentId { get; set; }
        public string Method { get; set; }
        public string? Entity { get; set; }
        public string? Reference { get; set; }
        public string? ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}