using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa um proprietário que anuncia propriedades físicas na plataforma.
    /// </summary>
    public class Landlord
    {
        /// <summary>
        /// Propriedade que diz respeito ao identificador do proprietário.
        /// </summary>
        [Key]
        public Guid LandlordId { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao nome do proprietário.
        /// </summary>
        [Required(ErrorMessage = "O nome do proprietário é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        /// <summary>
        /// Propriedade que diz respeito às propriedades físicas do proprietário.
        /// </summary>
        [Display(Name = "Propriedades")]
        public List<Property> Properties { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao utilizador associado ao proprietário.
        /// </summary>
        [Required(ErrorMessage = "O utilizador associado ao proprietário é obrigatório")]
        [Display(Name = "Utilizador")]
        public ApplicationUser User { get; set; }
    }
}
