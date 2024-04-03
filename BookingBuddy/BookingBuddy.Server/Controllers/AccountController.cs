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
        /// <summary>
        /// Regista um novo utilizador na plataforma.
        /// </summary>
        /// <param name="model">O modelo contendo os dados do utilizador a ser registado.</param>
        /// <param name="sendConfirmationEmail">Indica se deve ser enviado um email de confirmação (opcional, padrão é verdadeiro).</param>
        /// <returns>
        /// Um código de estado 200 (OK) se o utilizador for registado com sucesso.
        /// Um código de estado 400 (Bad Request) se o email fornecido já estiver em uso.
        /// Um código de estado 400 (Bad Request) se ocorrerem erros durante o registo do utilizador.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterModel model,
            bool sendConfirmationEmail = true)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);
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
                ProviderId = provider!.AspNetProviderId,
                Description = "",
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "user");
                if (!sendConfirmationEmail)
                {
                    return Ok();
                }

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink =
                    $"{configuration["ClientUrl"]}/confirm-email?token={HttpUtility.UrlEncode(token)}&uid={user.Id}";
                await EmailSender.SendTemplateEmail(configuration["MailAPIKey"] ?? "",
                    "d-a8fe3a81f5d44b4f9a3602650d0f8c8a", user.Email, user.Name,
                    new { confirmationLink });
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Reenvia o email de confirmação para um utilizador registado não confirmado.
        /// </summary>
        /// <param name="model">O modelo contendo o email do utilizador para o qual deve ser reenviado o email de confirmação.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se o email de confirmação for reenviado com sucesso.
        /// Um código de estado 400 (Bad Request) se o utilizador não estiver registado ou o email já estiver confirmado.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("resendConfirmation")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] EmailResendModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);
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

                var token = await userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                var confirmationLink =
                    $"{configuration["ClientUrl"]}/confirm-email?token={HttpUtility.UrlEncode(token)}&uid={existingUser.Id}";
                await EmailSender.SendTemplateEmail(configuration["MailAPIKey"] ?? "",
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
        /// Confirma o email de um utilizador registado com base no token fornecido.
        /// </summary>
        /// <param name="model">O modelo contendo o identificador único do utilizador (UID) e o token de confirmação.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se o email do utilizador for confirmado com sucesso.
        /// Um código de estado 404 (Not Found) se o utilizador não estiver registado.
        /// Um código de estado 400 (Bad Request) se ocorrerem erros durante a confirmação do email.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmModel model)
        {
            var user = await userManager.FindByIdAsync(model.Uid);
            if (user == null)
            {
                return NotFound(new[]
                {
                    new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
                });
            }

            var result = await userManager.ConfirmEmailAsync(user, model.Token);
            if (result.Succeeded)
            {
                await userManager.UpdateSecurityStampAsync(user);
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Verifica se o token de confirmação de email é válido para um utilizador registado.
        /// </summary>
        /// <param name="model">O modelo contendo o identificador único do utilizador (UID) e o token de confirmação.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se o token de confirmação for válido.
        /// Um código de estado 400 (Bad Request) se o token de confirmação for inválido ou o utilizador não estiver registado.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("checkConfirmation")]
        public async Task<IActionResult> CheckConfirmationToken([FromBody] EmailConfirmModel model)
        {
            var existingUser = await userManager.FindByIdAsync(model.Uid);
            if (existingUser != null)
            {
                var result = await userManager.VerifyUserTokenAsync(existingUser,
                    userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation", model.Token);
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
        /// Autentica um utilizador com base nas credenciais fornecidas.
        /// </summary>
        /// <param name="model">O modelo contendo o email e a senha do utilizador.</param>
        /// <param name="isPersistent">Indica se a sessão do utilizador deve ser persistente (opcional, padrão é verdadeiro).</param>
        /// <returns>
        /// Um código de estado 200 (OK) se as credenciais do utilizador forem válidas e a autenticação for bem-sucedida.
        /// Um código de estado 400 (Bad Request) se o email do utilizador não estiver confirmado ou as credenciais forem inválidas.
        /// </returns>
        /// <example>
        /// POST /api/login
        /// {
        ///     "email": "bookingbuddy.user@bookingbuddy.com",
        ///     "password": "userBB123!"
        /// }
        /// </example>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model, bool isPersistent = true)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
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
                    var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                }
                else
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, true, false);
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
        /// Autentica um utilizador usando um provedor de autenticação externo, como Google ou Microsoft.
        /// </summary>
        /// <param name="model">O modelo contendo o token de autenticação do provedor.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se a autenticação for bem-sucedida.
        /// Um código de estado 400 (Bad Request) se ocorrerem erros durante o processo de autenticação.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("loginProviders")]
        public async Task<IActionResult> LoginProviders([FromBody] LoginWithProviderModel model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(model.Token);
            var email = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;
            var existingUser = await userManager.FindByEmailAsync(email);
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
                    PictureUrl = photoUrl,
                    Description = ""
                };
                var userCreateResult = await userManager.CreateAsync(user);
                if (!userCreateResult.Succeeded) return BadRequest(userCreateResult.Errors);
                await userManager.AddToRoleAsync(user, "user");
                await signInManager.SignInAsync(user, true);
                return Ok();
            }

            await signInManager.SignInAsync(existingUser, true);
            return Ok();
        }

        /// <summary>
        /// Inicia o processo de autenticação usando o Google como provedor de autenticação externo.
        /// </summary>
        /// <param name="credential">O token de credencial fornecido pelo Google.</param>
        /// <returns>
        /// Um redirecionamento (código de estado 302) para a URL de autenticação no frontend.
        /// Um código de estado 400 (Bad Request) se ocorrerem erros durante o processo.
        /// </returns>
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
        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("google")]
        public IActionResult GoogleLogin([FromForm] string credential)
        {
            var provider = context.AspNetProviders.FirstOrDefault(p => p.NormalizedName == "GOOGLE");
            return Redirect(
                $"{configuration["ClientUrl"]}/signin?providerId={provider!.AspNetProviderId}&token={credential}");
        }

        /// <summary>
        /// Inicia o processo de autenticação usando a Microsoft como provedor de autenticação externo.
        /// </summary>
        /// <param name="id_token">O token de ID fornecido pela Microsoft.</param>
        /// <returns>
        /// Um redirecionamento (código de estado 302) para a URL de autenticação no frontend.
        /// Um código de estado 400 (Bad Request) se ocorrerem erros durante o processo.
        /// </returns>
        /// <example>
        ///     POST /api/microsoft
        ///     
        ///     Form:
        ///         id_token=eyJ...Iew&amp;
        ///         session_state=5d4...6ac
        ///       
        /// </example>
        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("microsoft")]
        // ReSharper disable once InconsistentNaming
        public IActionResult MicrosoftLogin([FromForm] string id_token)
        {
            var provider = context.AspNetProviders.FirstOrDefault(p => p.NormalizedName == "MICROSOFT");
            return Redirect(
                $"{configuration["ClientUrl"]}/signin?providerId={provider!.AspNetProviderId}&token={id_token}");
        }

        /// <summary>
        /// Inicia o processo de recuperação de palavra-passe para um utilizador com base no email fornecido.
        /// É enviado um email com um link para dar reset à password para o email fornecido, se este estiver associado a um utilizador.
        /// </summary>
        /// <param name="model">Os dados necessários para iniciar o processo de recuperação de palavra-passe.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se o email de recuperação for enviado com sucesso.
        /// Um código de estado 400 (Pedido Inválido) se ocorrerem erros durante o processo.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordRecoveryModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);
                var recoverLink =
                    $"{configuration["ClientUrl"]}/reset-password?token={HttpUtility.UrlEncode(token)}&uid={existingUser.Id}";
                await EmailSender.SendTemplateEmail(configuration["MailAPIKey"] ?? "",
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
        /// Nenhum dos parâmetros pode ser nulo. 
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
            var existingUser = await userManager.FindByIdAsync(model.Uid);
            if (existingUser != null)
            {
                var result = await userManager.ResetPasswordAsync(existingUser, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(existingUser);
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
        /// Nenhum dos parâmetros pode ser nulo.
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
            var existingUser = await userManager.FindByIdAsync(model.Uid);
            if (existingUser != null)
            {
                var result = await userManager.VerifyUserTokenAsync(existingUser,
                    userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", model.Token);
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
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Unauthorized();
            }

            var existingUser = await userManager.GetUserAsync(User);
            if (existingUser == null)
                return BadRequest(new[]
                {
                    new IdentityError { Code = "UserNotFound", Description = "O utilizador não se encontra registado." }
                });
            var provider = context.AspNetProviders.FirstOrDefault(p => p.AspNetProviderId == existingUser.ProviderId);
            var roles = await userManager.GetRolesAsync(existingUser);
            return Ok(new UserInfoModel
                (
                    existingUser.Id,
                    provider!.Name,
                    roles.ToList(),
                    existingUser.Name,
                    existingUser.UserName!,
                    existingUser.Email!,
                    existingUser.EmailConfirmed,
                    existingUser.Description
                )
            );
        }

        /// <summary>
        /// Alterar informação do utilizador autenticado.
        /// </summary>
        /// <remarks>
        /// Nenhum dos parâmetros pode ser nulo.
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
            var existingUser = await userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                var resultPasswordChange =
                    await userManager.ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
                if (!resultPasswordChange.Succeeded)
                {
                    return BadRequest(resultPasswordChange.Errors);
                }

                if (existingUser.UserName != model.NewUserName)
                {
                    var resultUserNameChange = await userManager.SetUserNameAsync(existingUser, model.NewUserName);
                    if (!resultUserNameChange.Succeeded)
                    {
                        return BadRequest(resultUserNameChange.Errors);
                    }
                }

                if (existingUser.Email != model.NewEmail)
                {
                    var emailToken = await userManager.GenerateChangeEmailTokenAsync(existingUser, model.NewEmail);
                    var resultEmailChange =
                        await userManager.ChangeEmailAsync(existingUser, model.NewEmail, emailToken);
                    if (!resultEmailChange.Succeeded)
                    {
                        return BadRequest(resultEmailChange.Errors);
                    }
                }

                if (existingUser.Name != model.NewName)
                {
                    existingUser.Name = model.NewName;
                    await userManager.UpdateAsync(existingUser);
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
        /// Nenhum dos parâmetros pode ser nulo. 
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
            var existingUser = await userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                var resultPasswordChange =
                    await userManager.ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
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
    /// Modelo de informação do utilizador
    /// </summary>
    /// <param name="UserId">Id do utilizador</param>
    /// <param name="Provider">Nome do fornecedor</param>
    /// <param name="Roles">Tipo de utilizador</param>
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
        bool IsEmailConfirmed,
        string Description);
}