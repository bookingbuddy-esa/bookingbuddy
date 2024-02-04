using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using System.Web;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using NuGet.Common;

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
        [AllowAnonymous]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(IdentityResult.Failed().Errors.Append(new PortugueseIdentityErrorDescriber().DuplicateEmail(model.Email)));
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"https://localhost:4200/confirm-email?token={HttpUtility.UrlEncode(token)}&uid={user.Id}";
                await EmailSender.SendTemplateEmail("d-a8fe3a81f5d44b4f9a3602650d0f8c8a", user.Email, user.Name, new { confirmationLink });
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Uid);
            if (user == null)
            {
                return NotFound(IdentityResult.Failed().Errors.Append(new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }));
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("api/google")]
        public async Task<IActionResult> GoogleLogin([FromForm] GoogleSignInModel model)
        {
            var token = model.Credential;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var email = jwtSecurityToken.Claims.Where(claim => claim.Type == "email").First().Value;
            var exsistingUser = await _userManager.FindByEmailAsync(email);
            if (exsistingUser == null)
            {
                var name = jwtSecurityToken.Claims.Where(claim => claim.Type == "name").First().Value;
                var user = new ApplicationUser() { Email = email, UserName = email, Name = name };
                var userCreateResult = await _userManager.CreateAsync(user);
                if (userCreateResult.Succeeded)
                {
                    var addClaimsResult = await _userManager.AddClaimsAsync(user, jwtSecurityToken.Claims);
                    if (addClaimsResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, true);
                        return Redirect("https://localhost:4200/");
                    }
                    BadRequest(addClaimsResult.Errors);
                }
                return BadRequest(userCreateResult.Errors);
            }
            else
            {
                await _signInManager.SignInAsync(exsistingUser, true);
                return Redirect("https://localhost:4200/");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("api/microsoft")]
        public async Task<IActionResult> MicrosoftLogin([FromForm] MicrosoftSignInModel model)
        {
            var token = model.Id_token;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var email = jwtSecurityToken.Claims.Where(claim => claim.Type == "email").First().Value;
            var exsistingUser = await _userManager.FindByEmailAsync(email);
            if (exsistingUser == null)
            {
                var name = jwtSecurityToken.Claims.Where(claim => claim.Type == "name").First().Value;
                var user = new ApplicationUser() { Email = email, UserName = email, Name = name };
                var userCreateResult = await _userManager.CreateAsync(user);
                if (userCreateResult.Succeeded)
                {
                    var addClaimsResult = await _userManager.AddClaimsAsync(user, jwtSecurityToken.Claims);
                    if (addClaimsResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, true);
                        return Redirect("https://localhost:4200/");
                    }
                    BadRequest(addClaimsResult.Errors);
                }
                return BadRequest(userCreateResult.Errors);
            }
            else
            {
                await _signInManager.SignInAsync(exsistingUser, true);
                return Redirect("https://localhost:4200/");
            }
        }

        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
            return BadRequest(IdentityResult.Failed().Errors.Append(new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }));
        }

        [HttpGet]
        [Authorize]
        [Route("api/manage/info")]
        public async Task<IActionResult> ManageInfo()
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                return Ok(new { existingUser.Name, existingUser.UserName, existingUser.Email, isEmailConfirmed = existingUser.EmailConfirmed });
            }
            return BadRequest(IdentityResult.Failed().Errors.Append(new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }));
        }

        [HttpPost]
        [Authorize]
        [Route("api/manage/info")]
        public async Task<IActionResult> ManageInfo([FromBody] AccountManageModel model)
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                var resultPasswordChange = await _userManager.ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
                if (!resultPasswordChange.Succeeded)
                {
                    return BadRequest(resultPasswordChange.Errors);
                }
                if (existingUser.UserName != model.NewUserName)
                {
                    var resultUserNameChange = await _userManager.SetUserNameAsync(existingUser, model.NewUserName);
                    if (!resultUserNameChange.Succeeded)
                    {
                        return BadRequest(resultUserNameChange.Errors);
                    }
                }
                if (existingUser.Email != model.NewEmail)
                {
                    var emailToken = await _userManager.GenerateChangeEmailTokenAsync(existingUser, model.NewEmail);
                    var resultEmailChange = await _userManager.ChangeEmailAsync(existingUser, model.NewEmail, emailToken);
                    if (!resultEmailChange.Succeeded)
                    {
                        return BadRequest(resultEmailChange.Errors);
                    }
                }
                if (existingUser.Name != model.NewName)
                {
                    existingUser.Name = model.NewName;
                    await _userManager.UpdateAsync(existingUser);
                }
                return Ok(new { existingUser.Name, existingUser.UserName, existingUser.Email, isEmailConfirmed = existingUser.EmailConfirmed });
            }
            return BadRequest(IdentityResult.Failed().Errors.Append(new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }));
        }

        [HttpPost]
        [Authorize]
        [Route("api/manage/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel model)
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                var resultPasswordChange = await _userManager.ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
                if (!resultPasswordChange.Succeeded)
                {
                    return BadRequest(resultPasswordChange.Errors);
                }
                return Ok();
            }
            return BadRequest(IdentityResult.Failed().Errors.Append(new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }));
        }

        [HttpPost]
        [Authorize]
        [Route("api/logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        }
    }
    public record AccountRegisterModel(string Name, string Email, string Password);
    /* // todo: mudar depois ?
       public enum AccountType; */

    public record EmailConfirmModel(string Uid, string Token);

    public record PasswordRecoveryModel(string Email);

    public record PasswordResetModel(string Uid, string Token, string NewPassword);

    public record AccountManageModel(string NewName, string NewUserName, string NewEmail, string NewPassword, string OldPassword);

    public record PasswordChangeModel(string NewPassword, string OldPassword);

    public record GoogleSignInModel(string ClientId, string Client_id, string Credential, string Select_by, string G_csrf_token);

    public record MicrosoftSignInModel(string Id_token,string Session_state);
}