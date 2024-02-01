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

public class AccountRegisterModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    // todo: mudar depois ?
    public enum AccountType;
}
