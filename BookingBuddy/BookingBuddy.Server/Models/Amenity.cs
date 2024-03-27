using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa as comodidades de uma propriedade física na plataforma.
    /// </summary>
    public class Amenity
    {
        /// <summary>
        /// Identificador de uma comodidade de uma propriedade física a anunciar.
        /// </summary>
        [Key]
        public string AmenityId { get; set; }

        /// <summary>
        /// Nome de uma comodidade de uma propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "O nome da comodidade é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        
        /// <summary>
        /// Nome de exibição de uma comodidade de uma propriedade física a anunciar.
        /// </summary>
        public string DisplayName { get; set; }
    }
}
