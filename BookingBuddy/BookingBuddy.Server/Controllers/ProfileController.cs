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
                return NotFound();
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
    }

    public record ProfileInfoModel(
        string UserId,
        string Name,
        string Email,
        List<string> Roles,
        string PictureUrl,
        string Description
    );
}