using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa uma propriedade física a anunciar na plataforma.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Propriedade que diz respeito ao identificador da propriedade física a anunciar.
        /// </summary>
        [Key]
        public string PropertyId { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao identificador do proprietário da propriedade.
        /// </summary>
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// Propriedade que diz respeito aos identificadores das comodidades da propriedade.
        /// </summary>
        public List<string>? AmenityIds { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao nome da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "O nome da propriedade é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        /// <summary>
        /// Propriedade que diz respeito à descrição da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "A descrição da propriedade é obrigatória")]
        [MaxLength(500)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao preço por noite da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "O preço por noite da propriedade é obrigatório")]
        [Range(1, 1000000)]
        [Display(Name = "Preço por noite")]
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Propriedade de navegação que diz respeito às comodidades da propriedade física a anunciar.
        /// </summary>
        [Display(Name = "Comodidades")]
        public List<Amenity>? Amenities { get; set; }

        /// <summary>
        /// Propriedade que diz respeito à localização da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "A localização da propriedade é obrigatória")]
        [MaxLength(100)]
        [Display(Name = "Localização")]
        public string Location { get; set; }

        /// <summary>
        /// Propriedade que diz respeito às fotografias da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "As fotografias da propriedade são obrigatórias")]
        [Display(Name = "Fotografias")]
        public List<string> ImagesUrl { get; set; }

        /// <summary>
        /// Propriedade de navegação que diz respeito ao proprietário da propriedade física a anunciar.
        /// </summary>
        [Display(Name = "Proprietário")]
        public ReturnUser? ApplicationUser { get; set; }

        /// <summary>
        /// Propriedade que diz respeito às datas bloqueadas da propriedade física a anunciar.
        /// </summary>
        public List<BlockedDate>? BlockedDates { get; set; }

        /// <summary>
        /// Propriedade que diz respeito aos descontos da propriedade física a anunciar.
        /// </summary>
        public List<Discount>? Discounts { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao número de cliques que a propriedade física a anunciar teve.
        /// </summary>
        public int Clicks { get; set; }
    }

    [NotMapped]
    public class ReturnUser
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
