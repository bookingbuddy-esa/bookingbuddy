using System.ComponentModel.DataAnnotations;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa um favorito (de uma propriedade).
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// Identificador do favorito.
        /// </summary>
        [Key]
        public int FavoriteId { get; set; }

        /// <summary>
        /// Identificador do utilizador que adicionou a propriedade aos favoritos.
        /// </summary>
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// Identificador da propriedade que foi adicionada aos favoritos.
        /// </summary>
        public string PropertyId { get; set; }

        /// <summary>
        /// Utilizador que adicionou a propriedade aos favoritos (propriedade de navegação).
        /// </summary>
        public ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// Propriedade que foi adicionada aos favoritos (propriedade de navegação).
        /// </summary>
        public Property Property { get; set; }
    }
}
