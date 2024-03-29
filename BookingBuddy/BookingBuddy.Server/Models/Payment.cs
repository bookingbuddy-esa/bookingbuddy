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
        /// <summary>
        /// Identificador do pagamento.
        /// </summary>
        [Key] public string PaymentId { get; set; }

        /// <summary>
        /// Método de pagamento.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Entidade.
        /// </summary>
        public string? Entity { get; set; }

        /// <summary>
        /// Referência.
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        /// Data de expiração/validade da referência.
        /// </summary>
        public string? ExpiryDate { get; set; }

        /// <summary>
        /// Montante.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Estado do pagamento.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Data de criação do pagamento.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}