using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa um utilizador da plataforma.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Propriedade que diz respeito ao nome do utilizador.
        /// </summary>
        [PersonalData]
        public string Name { get; set; }

        /// <summary>
        /// Propriedade que diz respeito ao url da imagem de perfil do utilizador.
        /// </summary>
        [PersonalData]
        public string? PictureUrl { get; set; }
    }
}
