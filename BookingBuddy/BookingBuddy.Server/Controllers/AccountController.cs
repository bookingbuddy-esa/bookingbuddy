using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.CodeDom.Compiler;
using BookingBuddy.Server.Services;
using NuGet.Protocol;
using System.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BookingBuddy.Server.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new[] { new { code = "EmailInUse", description = "O e-mail inserido já está em uso." } });
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"https://localhost:4200/confirmation-email?token={HttpUtility.UrlEncode(token)}&uid={user.Id}";
                await EmailSender.SendTemplateEmail("d-a8fe3a81f5d44b4f9a3602650d0f8c8a", user.Email, user.Name, new { confirmationLink });
                Console.WriteLine(confirmationLink);
                return Ok();
            }

            return BadRequest(result.Errors);
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("api/confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Parâmetros inválidos para a confirmação de e-mail.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok("E-mail confirmado com sucesso.");
            }
            else
            {
                return BadRequest("Falha ao confirmar o e-mail.");
            }
        }


        [HttpPost]
        [Route("api/forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordRecoveryModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var recoverLink = $"https://localhost:4200/reset-password?token={HttpUtility.UrlEncode(token)}&uid={existingUser.Id}";
                await EmailSender.SendTemplateEmail("d-1a60ea506e2d4e26b3221bd331286533", existingUser.Email, existingUser.Name, new { recoverLink });
                Console.WriteLine(recoverLink);
                return Ok();
            }
            return BadRequest("Email fornecido não está registado!");
        }

        [HttpPost]
        [Route("api/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Uid);
            if (existingUser != null)
            {
                var result = await _userManager.ResetPasswordAsync(existingUser, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(IdentityResult.Failed().Errors.Append(new IdentityError { Code="UserNotFound", Description="O utilizador não se encontra registado."}));
        }


        [HttpPost("api/logout")]
        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        }
    }
}

public class AccountRegisterModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    // todo: mudar depois ?
    public enum AccountType;
}

public class PasswordRecoveryModel
{
    public string Email { get; set; }
}

public class PasswordResetModel
{
    public string Uid { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}
