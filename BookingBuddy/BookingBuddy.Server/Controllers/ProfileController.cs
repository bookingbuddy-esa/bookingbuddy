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

    public record ProfileInfoModel(
        string UserId,
        string Name,
        string Email,
        List<string> Roles,
        string PictureUrl,
        string Description
    );

    public record UpdateDescriptionModel(
        string Description
    );

    public record UpdateProfilePictureModel(
        string ImageUrl
    );
}