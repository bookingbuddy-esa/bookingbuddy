using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    public class Discount
    {
        /// <summary>
        /// Propriedade que diz respeito ao identificador do desconto de uma propriedade.
        /// </summary>
        [Key]
        public int DiscountId { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao identificador do desconto de uma propriedade.
        /// </summary>
        [Required(ErrorMessage = "A quantia do desconto é obrigatória")]
        public int DiscountAmount { get; set; }

        /// <summary>
        /// Propriedade que diz respeito à data de incio.
        /// </summary>
        [Required(ErrorMessage = "A data de inicio é obrigatória")]
        public String StartDate { get; set; }

        /// <summary>
        /// Propriedade que diz respeito à data final.
        /// </summary>
        [Required(ErrorMessage = "A data de fim é obrigatória")]
        public String EndDate { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao identificador da propriedade do desconto.
        /// </summary>
        public String PropertyId { get; set; }

    }
}
