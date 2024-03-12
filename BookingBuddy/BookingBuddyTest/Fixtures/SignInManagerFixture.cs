using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BookingBuddyTest.Fixtures;

public class SignInManagerFixture(UserManagerFixture userManagerFixture)
{
    public SignInManager<ApplicationUser> SignInManager { get; private set; } = new(
        userManagerFixture.UserManager,
        new HttpContextAccessor(),
        new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
        new Mock<IOptions<IdentityOptions>>().Object,
        new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
        new Mock<IAuthenticationSchemeProvider>().Object,
        new DefaultUserConfirmation<ApplicationUser>()
    );
}