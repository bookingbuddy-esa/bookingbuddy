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
                return BadRequest("E-mail já está em uso.");
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);
                EmailSender emailSender = new EmailSender();
                await emailSender.SendEmail("Confirmação Email", user.Email, user.Name, confirmationLink);
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("api/signin")]
        public async Task<IActionResult> SignIn([FromBody] AccountLoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful" });
            }

            return BadRequest(new { message = "Login failed" });
        }

        [HttpPost]
        [Route("api/forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordRecoveryModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var recoverLink = $"https://localhost:4200/reset-password?token={token}&uid={existingUser.Id}";
                EmailSender emailSender = new();
                await emailSender.SendEmail("Recuperar Password", existingUser.Email, existingUser.Name, recoverLink);
                return Ok();
            }
            return BadRequest("Email fornecido não está registado!");
        }

        [HttpPost]
        [Route("api/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Id);
            if (existingUser != null)
            {
                var result = await _userManager.ResetPasswordAsync(existingUser, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest(result.Errors);
            }
            return BadRequest("Email fornecido não está registado!");
        }


        [HttpPost("api/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // Retorna uma resposta apropriada, como um status 200 OK
            return Ok(new { message = "Logout successful" });
        }
    }
}

public class AccountLoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
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
    public string Id { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}
