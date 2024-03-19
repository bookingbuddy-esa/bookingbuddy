using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BookingBuddyServerTest.Fixtures;

public class UserManagerFixture : IDisposable
{
    public UserManager<ApplicationUser> UserManager { get; private set; }

    public UserManagerFixture(ApplicationDbContextFixture context)
    {
        var services = new ServiceCollection();
        services.AddIdentity<ApplicationUser, IdentityUserRole<string>>().AddDefaultTokenProviders();
        services.AddLogging();
        UserManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context.DbContext, new IdentityErrorDescriber()),
            new OptionsWrapper<IdentityOptions>(new IdentityOptions()
            {
                Tokens = new TokenOptions()
                {
                    ProviderMap = new Dictionary<string, TokenProviderDescriptor>()
                    {
                        [TokenOptions.DefaultProvider] = new(typeof(DataProtectorTokenProvider<ApplicationUser>)),
                        [TokenOptions.DefaultEmailProvider] = new(typeof(EmailTokenProvider<ApplicationUser>)),
                        [TokenOptions.DefaultPhoneProvider] = new(typeof(PhoneNumberTokenProvider<ApplicationUser>)),
                        [TokenOptions.DefaultAuthenticatorProvider] =
                            new(typeof(AuthenticatorTokenProvider<ApplicationUser>))
                    }
                }
            }),
            new PasswordHasher<ApplicationUser>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            services.BuildServiceProvider(),
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object
        );
    }

    public void Dispose() => UserManager.Dispose();
}