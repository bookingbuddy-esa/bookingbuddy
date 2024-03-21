using System.Security.Claims;
using BookingBuddy.Server.Controllers;
using BookingBuddyServerTest.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingBuddyServerTest;

public class BookingControllerTest : IClassFixture<ApplicationDbContextFixture>
{
    private readonly ApplicationDbContextFixture _context;
    private readonly UserManagerFixture _userManager;

    public BookingControllerTest(ApplicationDbContextFixture context)
    {
        _context = context;
        _userManager = new UserManagerFixture(_context);
    }

    private BookingController CreateController(string? userId = null)
    {
        return new BookingController(
            _context.DbContext,
            _userManager.UserManager
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userId != null
                        ? new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                        {
                            new(ClaimTypes.NameIdentifier, userId),
                        }, "TestAuthentication"))
                        : new ClaimsPrincipal(),
                }
            }
        };
    }
    
    [Fact]
    public async Task GetBookings_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var controller = CreateController();
        Assert.NotNull(controller);
        
        var result = await controller.GetBookings();
        Assert.IsType<UnauthorizedResult>(result);
    }
    
    [Fact]
    public async Task GetBookings_Returns_Ok_When_User_Authenticated()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);
        
        var controller = CreateController(user.Id);
        Assert.NotNull(controller);
        
        var result = await controller.GetBookings();
        Assert.IsType<OkObjectResult>(result);
    }
}