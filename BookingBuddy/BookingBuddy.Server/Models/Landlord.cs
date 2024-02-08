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
        public string LandlordId { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao identificador do utilizador do proprietário.
        /// </summary>
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// Propriedade que diz respeito aos identificadores das propriedades do proprietário.
        /// </summary>
        public List<string>? PropertyIds { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao nome do proprietário.
        /// </summary>
        [Required(ErrorMessage = "O nome do proprietário é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        /// <summary>
        /// Propriedade de navegação que diz respeito às propriedades físicas do proprietário.
        /// </summary>
        [Display(Name = "Propriedades")]
        public List<Property>? Properties { get; set; }

        /// <summary>
        /// Propriedade de navegação que diz respeito ao utilizador associado ao proprietário.
        /// </summary>
        [Display(Name = "Utilizador")]
        public ApplicationUser? User { get; set; }
    }
}
