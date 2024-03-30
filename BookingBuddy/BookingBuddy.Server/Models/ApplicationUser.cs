using Microsoft.AspNetCore.Identity;

namespace BookingBuddy.Server.Models
{
    /// <summary>
    /// Classe que representa um utilizador da plataforma.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Nome do utilizador.
        /// </summary>
        [PersonalData]
        public string Name { get; set; }

            
        /// <summary>
        /// Descrição de perfil do utilizador.
        /// </summary>
        [PersonalData]
        public string? Description { get; set; }
        
        /// <summary>
        /// URL da imagem de perfil do utilizador.
        /// </summary>
        [PersonalData]
        public string? PictureUrl { get; set; }
        
        
        /// <summary>
        /// Identificador do fornecedor de login do utilizador.
        /// </summary>
        public string ProviderId { get; set; }
        
        /// <summary>
        /// Propriedade de navegação do fornecedor de login do utilizador.
        /// </summary>
        public AspNetProvider? Provider { get; set; }
    }
}
