using System.Security.Claims;
using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Models;
using BookingBuddyServerTest.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookingBuddyServerTest;

public class AccountControllerTest : IClassFixture<ApplicationDbContextFixture>
{
    private readonly ApplicationDbContextFixture _context;
    private readonly UserManagerFixture _userManager;
    private readonly SignInManagerFixture _signInManager;
    private readonly AccountController _controller;

    public AccountControllerTest(ApplicationDbContextFixture context)
    {
        _context = context;
        _userManager = new UserManagerFixture(_context);
        _signInManager = new SignInManagerFixture(_userManager);
        _controller = new AccountController(
            _context.DbContext,
            _userManager.UserManager,
            _signInManager.SignInManager,
            new Mock<IConfiguration>().Object
        );
    }

    private async Task<ApplicationUser?> CreateRandomUser()
    {
        var provider = _context.DbContext.AspNetProviders.FirstOrDefault(p => p.NormalizedName == "LOCAL");
        var email = $"bookingbuddy.{Guid.NewGuid().ToString()}@bookingbuddy.com";
        var createResult = await _userManager.UserManager.CreateAsync(new ApplicationUser()
        {
            UserName = email,
            Email = email,
            Name = $"User {Guid.NewGuid().ToString()}",
            EmailConfirmed = true,
            ProviderId = provider!.AspNetProviderId,
            Description = "Test User",
        }, "Test123!");

        if (!createResult.Succeeded)
        {
            return null;
        }

        return await _userManager.UserManager.FindByEmailAsync(email);
    }

    [Fact]
    public async void Register_Succeeds_When_User_Not_Registered()
    {
        var model = new AccountRegisterModel(
            $"Test User {Guid.NewGuid().ToString()}",
            $"test-{Guid.NewGuid().ToString()}@bookingbuddy.com",
            "Test123!"
        );
        var result = await _controller.Register(model, false);
        Assert.IsType<OkResult>(result);
        var user = await _context.DbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        Assert.NotNull(user);
    }

    [Fact]
    public async void Register_Fails_When_User_Already_Registered()
    {
        var model = new AccountRegisterModel(
            $"Test User {Guid.NewGuid().ToString()}",
            $"test-{Guid.NewGuid().ToString()}@bookingbuddy.com",
            "Test123!"
        );
        var resultSuccessful = await _controller.Register(model, false);
        Assert.IsType<OkResult>(resultSuccessful);
        var user = await _context.DbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        Assert.NotNull(user);
        var resultFailed = await _controller.Register(model, false);
        Assert.IsType<BadRequestObjectResult>(resultFailed);
    }

    [Fact]
    public async void ConfirmEmail_Succeeds_When_User_Not_Confirmed()
    {
        var user = await CreateRandomUser();
        Assert.NotNull(user);
        var token = await _userManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        var result = await _controller.ConfirmEmail(new EmailConfirmModel(user.Id, token));
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void ConfirmEmail_Fails_When_User_Already_Confirmed_With_Same_Token()
    {
        var user = await CreateRandomUser();
        Assert.NotNull(user);
        var token = await _userManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        var resultSuccessful = await _controller.ConfirmEmail(new EmailConfirmModel(user.Id, token));
        Assert.IsType<OkResult>(resultSuccessful);
        var resultFailed = await _controller.ConfirmEmail(new EmailConfirmModel(user.Id, token));
        Assert.IsType<BadRequestObjectResult>(resultFailed);
    }

    [Theory]
    [InlineData("invalid-token", typeof(BadRequestObjectResult))]
    [InlineData(null, typeof(BadRequestObjectResult))]
    [InlineData("valid-token", typeof(OkResult))]
    public async void CheckConfirmationToken_Returns_Expected_Result(string? token, Type expectedType)
    {
        var user = await CreateRandomUser();
        Assert.NotNull(user);
        if (token?.Equals("valid-token") ?? false)
        {
            token = await _userManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        }

        var result = await _controller.CheckConfirmationToken(new EmailConfirmModel(user.Id, token!));
        Assert.IsType(expectedType, result);
    }

    [Fact]
    public async void Login_Succeeds_When_User_Credentials_Are_Valid()
    {
        var result = await _controller.Login(new LoginModel(
            "bookingbuddy.user@bookingbuddy.com",
            "userBB123!"
        ), false);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void Login_Fails_When_User_Credentials_Are_Invalid()
    {
        var result = await _controller.Login(new LoginModel(
            "bookingbuddy.invalid@bookingbuddy.com",
            "invalid"
        ), false);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void ResetPassword_Succeeds_When_Token_Is_Valid()
    {
        var user = await CreateRandomUser();
        Assert.NotNull(user);
        var token = await _userManager.UserManager.GeneratePasswordResetTokenAsync(user);
        var result = await _controller.ResetPassword(new PasswordResetModel(
            user.Id,
            token,
            "NewPassword123!"
        ));
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void ResetPassword_Fails_When_Token_Is_Invalid()
    {
        var user = await CreateRandomUser();
        Assert.NotNull(user);
        var result = await _controller.ResetPassword(new PasswordResetModel(
            user.Id,
            "invalid-token",
            "NewPassword123!"
        ));
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Theory]
    [InlineData("invalid-token", typeof(BadRequestObjectResult))]
    [InlineData(null, typeof(BadRequestObjectResult))]
    [InlineData("valid-token", typeof(OkResult))]
    public async void CheckPasswordResetToken_Returns_Expected_Result(string token, Type expectedType)
    {
        var user = await CreateRandomUser();
        Assert.NotNull(user);

        if (token?.Equals("valid-token") ?? false)
        {
            token = await _userManager.UserManager.GeneratePasswordResetTokenAsync(user);
        }

        var result = await _controller.CheckResetPasswordToken(new PasswordResetModel(
            user.Id,
            token!,
            "NewPassword123!"
        ));

        Assert.IsType(expectedType, result);
    }

    [Fact]
    public async void ManageInfo_Returns_Info_When_User_Is_Authenticated()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);
        var controller = new AccountController(
            _context.DbContext,
            _userManager.UserManager,
            _signInManager.SignInManager,
            new Mock<IConfiguration>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                    {
                        new(ClaimTypes.NameIdentifier, user.Id),
                    }, "TestAuthentication")),
                }
            }
        };

        var result = await controller.ManageInfo();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void ManageInfo_Returns_Unauthorized_When_User_Is_Not_Authenticated()
    {
        var controller = new AccountController(
            _context.DbContext,
            _userManager.UserManager,
            _signInManager.SignInManager,
            new Mock<IConfiguration>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await controller.ManageInfo();
        Assert.IsType<UnauthorizedResult>(result);
    }
}