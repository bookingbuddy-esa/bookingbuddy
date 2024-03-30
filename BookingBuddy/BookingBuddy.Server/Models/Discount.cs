using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa um desconto.
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// Identificador do desconto de uma propriedade.
        /// </summary>
        [Key]
        public string DiscountId { get; set; }

        /// <summary>
        /// Valor do desconto (entre 0 e 100).
        /// </summary>
        [Required(ErrorMessage = "A quantia do desconto é obrigatória")]
        public int DiscountAmount { get; set; }

        /// <summary>
        /// Data de início do desconto.
        /// </summary>
        [Required(ErrorMessage = "A data de inicio é obrigatória")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data de fim do desconto.
        /// </summary>
        [Required(ErrorMessage = "A data de fim é obrigatória")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Identificador da propriedade com desconto.
        /// </summary>
        public string PropertyId { get; set; }

        public static implicit operator List<object>(Discount? v)
        {
            throw new NotImplementedException();
        }
    }
}
