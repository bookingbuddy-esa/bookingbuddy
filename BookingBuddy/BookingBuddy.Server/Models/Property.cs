using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa uma propriedade física a anunciar na plataforma.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Identificador da propriedade física a anunciar.
        /// </summary>
        [Key]
        [JsonPropertyName("propertyId")]
        public string PropertyId { get; set; }

        /// <summary>
        /// Identificador do proprietário da propriedade.
        /// </summary>
        [JsonPropertyName("applicationUserId")]
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// Lista de identificadores das comodidades da propriedade.
        /// </summary>
        [JsonPropertyName("amenityIds")]
        public List<string>? AmenityIds { get; set; }

        /// <summary>
        /// Nome da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "O nome da propriedade é obrigatório")]
        [Display(Name = "Nome")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Descrição da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "A descrição da propriedade é obrigatória")]
        [MaxLength(500)]
        [Display(Name = "Descrição")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Preço por noite da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "O preço por noite da propriedade é obrigatório")]
        [Range(1, 1000000)]
        [Display(Name = "Preço por noite")]
        [JsonPropertyName("pricePerNight")]
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Lista de comodidades da propriedade física a anunciar.
        /// </summary>
        [Display(Name = "Comodidades")]
        [JsonPropertyName("amenities")]
        public List<Amenity>? Amenities { get; set; }

        /// <summary>
        /// Localização da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "A localização da propriedade é obrigatória")]
        [MaxLength(100)]
        [Display(Name = "Localização")]
        [JsonPropertyName("location")]
        public string Location { get; set; }

        /// <summary>
        /// Lista de URLs de imagens da propriedade física a anunciar.
        /// </summary>
        [Required(ErrorMessage = "As fotografias da propriedade são obrigatórias")]
        [Display(Name = "Fotografias")]
        [JsonPropertyName("imagesUrl")]
        public List<string> ImagesUrl { get; set; }

        /// <summary>
        /// Proprietário da propriedade física a anunciar.
        /// </summary>
        [Display(Name = "Proprietário")]
        [JsonPropertyName("applicationUser")]
        public ReturnUser? ApplicationUser { get; set; }

        /// <summary>
        /// Lista das datas bloqueadas da propriedade física a anunciar.
        /// </summary>
        [JsonPropertyName("blockedDates")]
        public List<BlockedDate>? BlockedDates { get; set; }

        /// <summary>
        /// Lista dos descontos da propriedade física a anunciar.
        /// </summary>
        [JsonPropertyName("discounts")]
        public List<Discount>? Discounts { get; set; }

        /// <summary>
        /// Número de cliques que a propriedade física a anunciar teve.
        /// </summary>
        [JsonPropertyName("clicks")]
        public int Clicks { get; set; }
    }

    /// <summary>
    /// Classe que representa informações reduzidas do utilizador (apenas o seu nome e identificador).
    /// </summary>
    [NotMapped]
    public class ReturnUser
    {
        /// <summary>
        /// Nome do utilizador. 
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Identificador do utilizador.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
