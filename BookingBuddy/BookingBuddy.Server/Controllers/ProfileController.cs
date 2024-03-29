using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using BookingBuddy.Server.Data;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Classe que representa o controlador de gestão de perfil.
    /// </summary>
    /// <remarks>
    /// Construtor da classe ProfileController.
    /// </remarks>
    /// <param name="userManager">Gestor de Utilizadores</param>
    /// <param name="signInManager">Gestor de logins</param>
    [ApiController]
    [Route("api/profile")]
    public class ProfileController(
        BookingBuddyServerContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        /// <summary>
        /// Obtém as informações do perfil de um utilizador com base no seu identificador único.
        /// </summary>
        /// <param name="userId">O identificador único do utilizador para o qual se pretende obter o perfil.</param>
        /// <returns>
        /// As informações do perfil do utilizador especificado, incluindo nome, email, funções, URL da imagem e descrição.
        /// Retorna um código de estado 200 (OK) se as informações do perfil forem obtidas com sucesso.
        /// Retorna um código de estado 404 (Não Encontrado) se o utilizador não for encontrado.
        /// </returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Utilizador não encontrado!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var profileInfo = new ProfileInfoModel(
                user.Id,
                user.Name,
                user.Email,
                [.. roles],
                user.PictureUrl,
                user.Description
            );

            return Ok(profileInfo);
        }

        /// <summary>
        /// Atualiza a descrição de perfil de um utilizador autenticado.
        /// </summary>
        /// <param name="model">O modelo que contém a nova descrição de perfil.</param>
        /// <returns>
        /// Retorna um código de estado 200 (OK) se a descrição for atualizada com sucesso.
        /// Retorna um código de estado 404 (Não Encontrado) se o utilizador autenticado não for encontrado.
        /// Retorna um código de estado 400 (Pedido Inválido) se ocorrer um erro ao atualizar a descrição.
        /// </returns>
        [HttpPut]
        [Authorize]
        [Route("updateDescription")]
        public async Task<IActionResult> UpdateDescription([FromBody] UpdateDescriptionModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Utilizador não encontrado!");
            }

            user.Description = model.Description;
            try {
                await _userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            } catch (Exception e) {
                return BadRequest("Erro ao atualizar descrição!");
            }

            return Ok("Descrição atualizada com sucesso!");
        }

        /// <summary>
        /// Atualiza a imagem de perfil de um utilizador autenticado.
        /// </summary>
        /// <param name="model">O modelo que contém a URL da nova imagem de perfil.</param>
        /// <returns>
        /// Retorna um código de estado 200 (OK) se a imagem de perfil for atualizada com sucesso.
        /// Retorna um código de estado 404 (Não Encontrado) se o utilizador autenticado não for encontrado.
        /// Retorna um código de estado 400 (Pedido Inválido) se ocorrer um erro ao atualizar a imagem de perfil.
        /// </returns>
        [HttpPut]
        [Authorize]
        [Route("updateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] UpdateProfilePictureModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Utilizador não encontrado!");
            }

            user.PictureUrl = model.ImageUrl;
            try {
                await _userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            } catch (Exception e) {
                return BadRequest("Erro ao atualizar imagem de perfil!");
            }

            return Ok("Imagem de perfil atualizada com sucesso!");
        }
    }

    /// <summary>
    /// Modelo que representa as informações do perfil de um utilizador.
    /// </summary>
    public record ProfileInfoModel(
        string UserId,
        string Name,
        string Email,
        List<string> Roles,
        string PictureUrl,
        string Description
    );

    /// <summary>
    /// Modelo para atualização da descrição do perfil de um utilizador.
    /// </summary>
    public record UpdateDescriptionModel(
        string Description
    );

    /// <summary>
    /// Modelo para atualização da imagem de perfil de um utilizador.
    /// </summary>
    public record UpdateProfilePictureModel(
        string ImageUrl
    );
}