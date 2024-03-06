using System.Security.Claims;
using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Models;
using BookingBuddyTest.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookingBuddyTest;

public class PropertyControllerTest : IClassFixture<ApplicationDbContextFixture>
{
    private readonly ApplicationDbContextFixture _context;
    private readonly UserManagerFixture _userManager;
    private readonly PropertyController _controller;

    public PropertyControllerTest(ApplicationDbContextFixture context)
    {
        _context = context;
        _userManager = new UserManagerFixture(_context);
        _controller = new PropertyController(
            _context.DbContext,
            _userManager.UserManager,
            new Mock<IConfiguration>().Object
        );
    }

    private async Task<Property?> CreateRandomProperty()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        var property = new Property()
        {
            PropertyId = Guid.NewGuid().ToString(),
            ApplicationUserId = user!.Id,
            Name = $"Property {Guid.NewGuid().ToString()}",
            Description = "Test property",
            Location = "123 Test St",
            PricePerNight = 105,
            ImagesUrl = []
        };

        try
        {
            _context.DbContext.Property.Add(property);
            await _context.DbContext.SaveChangesAsync();
        }
        catch
        {
            return null;
        }

        return property;
    }

    [Fact]
    public async void GetProperties_Returns_AllProperties()
    {
        var result = await _controller.GetProperties();
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetProperty_Returns_Property()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var result = await _controller.GetProperty(property!.PropertyId);
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetProperty_Returns_NotFound_When_Property_NotFound()
    {
        var result = await _controller.GetProperty(Guid.NewGuid().ToString());
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetMetrics_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = new PropertyController(
            _context.DbContext,
            _userManager.UserManager,
            new Mock<IConfiguration>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await controller.GetMetrics(property.PropertyId);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void GetMetrics_Returns_NotFound_When_Property_NotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = new PropertyController(
            _context.DbContext,
            _userManager.UserManager,
            new Mock<IConfiguration>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                    {
                        new(ClaimTypes.NameIdentifier, user.Id),
                    }, "TestAuthentication")),
                }
            }
        };

        var result = await controller.GetMetrics(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetMetrics_Returns_Forbidden_When_User_NotOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = new PropertyController(
            _context.DbContext,
            _userManager.UserManager,
            new Mock<IConfiguration>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                    {
                        new(ClaimTypes.NameIdentifier, user.Id),
                    }, "TestAuthentication")),
                }
            }
        };

        var result = await controller.GetMetrics(property!.PropertyId);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void GetMetrics_Returns_Metrics_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");

        var controller = new PropertyController(
            _context.DbContext,
            _userManager.UserManager,
            new Mock<IConfiguration>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                    {
                        new(ClaimTypes.NameIdentifier, user!.Id),
                    }, "TestAuthentication")),
                }
            }
        };

        var result = await controller.GetMetrics(property!.PropertyId);
        Assert.IsType<OkObjectResult>(result);
    }
}