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
    /// Classe que representa o controlador de gestão de contas.
    /// </summary>
    /// <remarks>
    /// Construtor da classe AccountController.
    /// </remarks>
    /// <param name="userManager">Gestor de Utilizadores</param>
    /// <param name="signInManager">Gestor de logins</param>
    /// <param name="configuration">Configurações globais</param>
    [ApiController]
    [Route("api")]
    public class AccountController(
        BookingBuddyServerContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        /// <summary>
        /// Regista um utilizador e envia um e-mail de confirmação.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// </remarks>
        /// <param name="model">Modelo de registo de utilizador</param>
        /// <param name="sendConfirmationEmail">Opção para enviar email de confirmação de conta</param>
        /// <returns>Resposta do pedido de criar conta de utilizador, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterModel model,
            bool sendConfirmationEmail = true)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new[] { new PortugueseIdentityErrorDescriber().DuplicateEmail(model.Email) });
            }

            var provider = context.AspNetProviders.FirstOrDefault(p => p.NormalizedName == "LOCAL");
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                ProviderId = provider!.AspNetProviderId
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                if (!sendConfirmationEmail)
                {
                    return Ok();
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink =
                    $"{configuration.GetSection("Front-End-Url").Value!}/confirm-email?token={HttpUtility.UrlEncode(token)}&uid={user.Id}";
                await EmailSender.SendTemplateEmail(configuration.GetSection("MailAPIKey").Value!,
                    "d-a8fe3a81f5d44b4f9a3602650d0f8c8a", user.Email, user.Name,
                    new { confirmationLink });
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Reenvia o email de confirmação da conta.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// A conta do utilizador já deve ter sido criada previamente.
        /// 
        /// NOTA: A conta não se pode estar com o e-mail confirmado.
        /// </remarks>
        /// <param name="model">Modelo de reenvio de email</param>
        /// <returns>Reenvia o email de confirmação da conta, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("resendConfirmation")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] EmailResendModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                if (existingUser.EmailConfirmed)
                {
                    return BadRequest(new[]
                    {
                        new IdentityError()
                            { Code = "EmailAlreadyConfirmed", Description = "O email já se encontra confirmado." }
                    });
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                var confirmationLink =
                    $"{configuration.GetSection("Front-End-Url").Value!}/confirm-email?token={HttpUtility.UrlEncode(token)}&uid={existingUser.Id}";
                await EmailSender.SendTemplateEmail(configuration.GetSection("MailAPIKey").Value!,
                    "d-a8fe3a81f5d44b4f9a3602650d0f8c8a", existingUser.Email!,
                    existingUser.Name, new { confirmationLink });
                return Ok();
            }

            return BadRequest(new[]
            {
                new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
            });
        }

        /// <summary>
        /// Confirma o email.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// 
        /// NOTA: O utilizador tem de estar previamente registado na plataforma.
        /// </remarks>
        /// <param name="model">Modelo de confirmação de e-mail</param>
        /// <returns>Resposta do pedido de confirmação de e-mail, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Uid);
            if (user == null)
            {
                return NotFound(new[]
                {
                    new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
                });
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Verifica se o token de confirmação do e-mail é válido.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// </remarks>
        /// <param name="model">Modelo de confirmação de e-mail</param>
        /// <returns>Resposta do pedido de verificar o token de confirmação de e-mail, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("checkConfirmation")]
        public async Task<IActionResult> CheckConfirmationToken([FromBody] EmailConfirmModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Uid);
            if (existingUser != null)
            {
                var result = await _userManager.VerifyUserTokenAsync(existingUser,
                    _userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation", model.Token);
                if (result)
                {
                    return Ok();
                }

                return BadRequest(new[] { new PortugueseIdentityErrorDescriber().InvalidToken() });
            }

            return BadRequest(new[]
            {
                new IdentityError() { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
            });
        }

        /// <summary>
        /// Login com um email e uma palavra-passe.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// </remarks>
        /// <example>
        ///    POST /api/login
        /// 
        ///    {
        ///     "email": bookingbuddy.user@bookingbuddy.com,
        ///     "password": userBB123!
        ///    }
        /// </example>
        /// <param name="model">Modelo de login</param>
        /// <param name="isPersistent">Opção que define se é guardada uma cookie de sessão</param>
        /// <returns>Resposta do pedido de login, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model, bool isPersistent = true)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed == false)
                {
                    return BadRequest(new[]
                    {
                        new IdentityError
                            { Code = "EmailNotConfirmed", Description = "O e-mail não se encontra confirmado." }
                    });
                }

                if (!isPersistent)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                }
            }

            return BadRequest(new[]
                { new IdentityError { Code = "InvalidLogin", Description = "Credenciais inválidas." } });
        }

        /// <summary>
        /// Login com um fornecedor.
        /// </summary>
        /// <param name="model">Modelo de login com fornecedor</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("loginProviders")]
        public async Task<IActionResult> LoginProviders([FromBody] LoginWithProviderModel model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(model.Token);
            var email = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var name = jwtSecurityToken.Claims.First(claim => claim.Type == "name").Value;
                var provider = context.AspNetProviders.FirstOrDefault(p => p.AspNetProviderId == model.ProviderId);
                string? photoUrl = null;
                if (provider!.NormalizedName == "GOOGLE")
                {
                    photoUrl = jwtSecurityToken.Claims.First(claim => claim.Type == "picture").Value;
                }
                else if (provider.NormalizedName == "MICROSOFT")
                {
                    // TODO: Get photo from Microsoft Graph
                }

                var user = new ApplicationUser
                {
                    ProviderId = model.ProviderId,
                    Email = email,
                    UserName = email,
                    Name = name,
                    EmailConfirmed = true,
                    PictureUrl = photoUrl
                };
                var userCreateResult = await _userManager.CreateAsync(user);
                if (!userCreateResult.Succeeded) return BadRequest(userCreateResult.Errors);
                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, true);
                return Ok();
            }

            await _signInManager.SignInAsync(existingUser, true);
            return Ok();
        }

        /// <summary>
        /// Login com uma conta Google.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// </remarks>
        /// <example>
        ///     POST /api/google
        ///     
        ///     Form:
        ///         clientId=780...5nv.apps.googleusercontent.com&amp;
        ///         client_id=780...5nv.apps.googleusercontent.com&amp;
        ///         credential=eyJ...xrQ&amp;
        ///         select_by=btn&amp;
        ///         g_csrf_token=b228...ab5
        /// </example>
        /// <param name="model">Modelo de login com a Google</param>
        /// <returns>Redireciona para o link fornecido, se o pedido for concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("google")]
        public IActionResult GoogleLogin([FromForm] GoogleSignInModel model)
        {
            var token = model.Credential;
            var provider = context.AspNetProviders.FirstOrDefault(p => p.NormalizedName == "GOOGLE");
            return Redirect(
                $"{configuration.GetSection("Front-End-Url").Value}/signin?providerId={provider!.AspNetProviderId}&token={token}");
        }

        /// <summary>
        /// Login com uma conta Microsoft.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// </remarks>
        /// <example>
        ///     POST /api/microsoft
        ///     
        ///     Form:
        ///         id_token=eyJ...Iew&amp;
        ///         session_state=5d4...6ac
        ///       
        /// </example>
        /// <param name="model">Modelo de login com a Microsoft</param>
        /// <returns>Redireciona para o link fornecido, se o pedido for concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("microsoft")]
        public IActionResult MicrosoftLogin([FromForm] MicrosoftSignInModel model)
        {
            var token = model.Id_token;
            var provider = context.AspNetProviders.FirstOrDefault(p => p.NormalizedName == "MICROSOFT");
            return Redirect(
                $"{configuration.GetSection("Front-End-Url").Value}/signin?providerId={provider!.AspNetProviderId}&token={token}");
        }

        /// <summary>
        /// Recuperação da password através de um email.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// 
        /// NOTA: É enviado um email com um link para dar reset à password para o email fornecido, se este estiver associado a um utilizador.
        /// </remarks>
        /// <example>
        ///     POST /api/forgotPassword
        ///     
        ///     {
        ///        "email": bookingbuddy@bookingbuddy.com,
        ///     } 
        ///     
        /// </example>
        /// <param name="model">Modelo de recuperação de palavra-passe</param>
        /// <returns>Resposta do pedido de recuperar a palavra-passe, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordRecoveryModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var recoverLink =
                    $"{configuration.GetSection("Front-End-Url").Value!}/reset-password?token={HttpUtility.UrlEncode(token)}&uid={existingUser.Id}";
                await EmailSender.SendTemplateEmail(configuration.GetSection("MailAPIKey").Value!,
                    "d-1a60ea506e2d4e26b3221bd331286533", existingUser.Email!,
                    existingUser.Name, new { recoverLink });
                return Ok();
            }

            return BadRequest(new[] { new PortugueseIdentityErrorDescriber().InvalidEmail(model.Email) });
        }

        /// <summary>
        /// Reset da password através de um token e do id do utilizador.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null. 
        /// </remarks>
        /// <example>
        ///     POST /api/resetPassword
        ///     
        ///     {
        ///        "uid": 9ba9b407-...-0e8576fc82eb,
        ///        "token": CfDJ8F07Dx+8jTJPhU/.../ICT9WBiXAN43PM1Da5Na,
        ///        "newPassword": novapassword123!
        ///     } 
        /// </example>
        /// <param name="model">Modelo de reset da palavra-passe</param>
        /// <returns>Resposta do pedido de reset da password, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Uid);
            if (existingUser != null)
            {
                var result = await _userManager.ResetPasswordAsync(existingUser, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(existingUser);
                    return Ok();
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(new[]
            {
                new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
            });
        }

        /// <summary>
        /// Verifica se o token de reset da palavra-passe é válido.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// </remarks>
        /// <param name="model">Modelo de reset da palavra-passe</param>
        /// <returns>Resposta do pedido de verificar o token de reset da palavra-passe, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("checkResetPassword")]
        public async Task<IActionResult> CheckResetPasswordToken([FromBody] PasswordResetModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Uid);
            if (existingUser != null)
            {
                var result = await _userManager.VerifyUserTokenAsync(existingUser,
                    _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", model.Token);
                if (result)
                {
                    return Ok();
                }

                return BadRequest(new[] { new PortugueseIdentityErrorDescriber().InvalidToken() });
            }

            return BadRequest(new[]
            {
                new IdentityError() { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
            });
        }

        /// <summary>
        /// Obter informação do utilizador autenticado.
        /// </summary>
        /// <remarks>
        /// NOTA: É necessário estar autenticado para fazer este pedido.
        /// </remarks>
        /// <example>
        ///     GET /api/manage/info
        /// </example>
        /// <returns>Infromação do utilizador autenticado.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("manage/info")]
        public async Task<IActionResult> ManageInfo()
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser == null)
                return BadRequest(new[]
                {
                    new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
                });
            var provider = context.AspNetProviders.FirstOrDefault(p => p.AspNetProviderId == existingUser.ProviderId);
            var roles = await _userManager.GetRolesAsync(existingUser);
            return Ok(new UserInfoModel
                (
                    existingUser.Id,
                    provider!.Name,
                    roles.ToList(),
                    existingUser.Name,
                    existingUser.UserName!,
                    existingUser.Email!,
                    existingUser.EmailConfirmed
                )
            );
        }

        /// <summary>
        /// Alterar informação do utilizador autenticado.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null.
        /// 
        /// NOTA: É necessário estar autenticado para fazer este pedido.
        /// </remarks>
        /// <example>
        ///     POST /api/manage/info
        ///     
        ///     {
        ///         "newName": Booking Buddy,
        ///         "newUserName": bookingbuddy@bookingbuddy,
        ///         "newEmail": bookingbuddy@bookingbuddy,
        ///         "newPassword": BookingBuddy123!,
        ///         "oldPassword": BookingBuddy123!
        ///     }
        /// </example>
        /// <param name="model">Modelo de alteração de informação do utilizador</param>
        /// <returns>Ressposta do pedido de alteração de informação, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("manage/info")]
        public async Task<IActionResult> ManageInfo([FromBody] AccountManageModel model)
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                var resultPasswordChange =
                    await _userManager.ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
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
                    var resultEmailChange =
                        await _userManager.ChangeEmailAsync(existingUser, model.NewEmail, emailToken);
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

                return Ok(new
                {
                    existingUser.Name, existingUser.UserName, existingUser.Email,
                    isEmailConfirmed = existingUser.EmailConfirmed
                });
            }

            return BadRequest(new[]
            {
                new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
            });
        }

        /// <summary>
        /// Mudar de palavra-passe.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser null. 
        /// 
        /// NOTA: É necessário estar autenticado para fazer este pedido.
        /// </remarks>
        /// <example>
        ///     POST /api/manage/changePassword
        ///     
        ///     {
        ///        "newPassword": BookingBuddy123#,
        ///        "oldPassword": BookingBuddy123!
        ///     } 
        /// </example>
        /// <param name="model">Modelo de mudança de palavra-passe</param>
        /// <returns>Resposta do pedido de mudança da palavra-passe, OK(200) se concluido com sucesso.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("manage/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel model)
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                var resultPasswordChange =
                    await _userManager.ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
                if (!resultPasswordChange.Succeeded)
                {
                    return BadRequest(resultPasswordChange.Errors);
                }

                return Ok();
            }

            return BadRequest(new[]
            {
                new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
            });
        }

        /// <summary>
        /// Sair da conta.
        /// </summary>
        /// <remarks>
        /// NOTA: É necessário estar autenticado para fazer este pedido.
        /// </remarks>
        /// <example>
        /// POST /api/logout
        /// </example>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        }
    }

    /// <summary>
    /// Modelo de registo de utilizador.
    /// </summary>
    /// <param name="Name">Nome do utilizador</param>
    /// <param name="Email">E-mail do utilizador</param>
    /// <param name="Password">Palavra-passe do utilizador</param>
    public record AccountRegisterModel(string Name, string Email, string Password);

    /// <summary>
    /// Modelo de reenvio de email.
    /// </summary>
    /// <param name="Email">Email do utilizador</param>
    public record EmailResendModel(string Email);

    /// <summary>
    /// Modelo de confirmação do e-mail.
    /// </summary>
    /// <param name="Uid">Id do utilizador</param>
    /// <param name="Token">Token de confirmação de e-mail</param>
    public record EmailConfirmModel(string Uid, string Token);

    /// <summary>
    /// Modelo de login.
    /// </summary>
    /// <param name="Email">E-mail do utilizador</param>
    /// <param name="Password">Palavra-passe do utilizador</param>
    public record LoginModel(string Email, string Password);

    /// <summary>
    /// Modelo de login com fornecedor.
    /// </summary>
    /// <param name="ProviderId">Id do fornecedor</param>
    /// <param name="Token">Token de login</param>
    public record LoginWithProviderModel(string ProviderId, string Token);

    /// <summary>
    /// Modelo de recuperação de palavra-passe.
    /// </summary>
    /// <param name="Email">Email do utilizador</param>
    public record PasswordRecoveryModel(string Email);

    /// <summary>
    /// Modelo de reset da palavra-passe.
    /// </summary>
    /// <param name="Uid">Id do utilizador</param>
    /// <param name="Token">Token de reset da palavra-passe</param>
    /// <param name="NewPassword">Nova palavra-passe</param>
    public record PasswordResetModel(string Uid, string Token, string NewPassword);

    /// <summary>
    /// Modelo de gestão de conta.
    /// </summary>
    /// <param name="NewName">Novo nome</param>
    /// <param name="NewUserName">Novo nome de utilizador</param>
    /// <param name="NewEmail">Novo email</param>
    /// <param name="NewPassword">Nova palavra-passe</param>
    /// <param name="OldPassword">Palavra-passe antiga</param>
    public record AccountManageModel(
        string NewName,
        string NewUserName,
        string NewEmail,
        string NewPassword,
        string OldPassword);

    /// <summary>
    /// Modelo de mudança de palavra-passe.
    /// </summary>
    /// <param name="NewPassword">Nova palavra-passe</param>
    /// <param name="OldPassword">Palavra-passe antiga</param>
    public record PasswordChangeModel(string NewPassword, string OldPassword);

    /// <summary>
    /// Modelo de login com a Google.
    /// </summary>
    /// <param name="ClientId">Id do cliente</param>
    /// <param name="Client_id">Id do cliente</param>
    /// <param name="Credential">Credencial</param>
    /// <param name="Select_by">Selecionado por</param>
    /// <param name="G_csrf_token">Token CSRF</param>
    public record GoogleSignInModel(
        string ClientId,
        string Client_id,
        string Credential,
        string Select_by,
        string G_csrf_token);

    /// <summary>
    /// Modelo de login com a Microsoft.
    /// </summary>
    /// <param name="Id_token">Token de login</param>
    /// <param name="Session_state">Estado da sessão</param>
    public record MicrosoftSignInModel(string Id_token, string Session_state);

    /// <summary>
    /// Modelo de informação do utilizador
    /// </summary>
    /// <param name="UserId">Id do utilizador</param>
    /// <param name="Provider">Nome do fornecedor</param>
    /// <param name="Role">Tipo de utilizador</param>
    /// <param name="Name">Nome do utilizador</param>
    /// <param name="UserName">Nome da conta do utilizador</param>
    /// <param name="Email">E-mail do utilizador</param>
    /// <param name="IsEmailConfirmed">Estado da confirmação do e-mail</param>
    public record UserInfoModel(
        string UserId,
        string Provider,
        List<string> Roles,
        string Name,
        string UserName,
        string Email,
        bool IsEmailConfirmed);
}