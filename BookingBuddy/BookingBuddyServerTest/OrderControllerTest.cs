using System.Security.Claims;
using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Models;
using BookingBuddyServerTest.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookingBuddyServerTest;

public class OrderControllerTest : IClassFixture<ApplicationDbContextFixture>
{
    private readonly ApplicationDbContextFixture _context;
    private readonly UserManagerFixture _userManager;

    public OrderControllerTest(ApplicationDbContextFixture context)
    {
        _context = context;
        _userManager = new UserManagerFixture(_context);
    }

    private OrderController CreateController(string? userId = null)
    {
        return new OrderController(
            _context.DbContext,
            _userManager.UserManager,
            new PaymentController(
                _context.DbContext,
                _userManager.UserManager,
                new Mock<IConfiguration>().Object
            )
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

    private async Task<Property?> CreateRandomProperty(string email = "bookingbuddy.landlord@bookingbuddy.com")
    {
        var user = await _userManager.UserManager.FindByEmailAsync(email);
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

    private async Task<OrderBase?> CreateRandomOrder(
        Type orderType,
        string email = "bookingbuddy.user@bookingbuddy.com",
        string method = "Multibanco"
    )
    {
        var user = await _userManager.UserManager.FindByEmailAsync(email);

        var rnd = new Random();

        var payment = (await _context.DbContext.Payment.AddAsync(new Payment
        {
            PaymentId = Guid.NewGuid().ToString(),
            Method = method,
            Entity = method == "Multibanco" ? $"{rnd.Next(10000, 99999)}" : null,
            Reference = method == "Multibanco" ? $"{rnd.Next(10000000, 99999999)}" : null,
            Amount = rnd.Next(),
            Status = "Paid",
            CreatedAt = DateTime.Now,
            ExpiryDate = DateTime.Now.AddDays(1).ToLongDateString(),
        })).Entity;

        var property = (await _context.DbContext.Property.AddAsync(new Property
        {
            PropertyId = Guid.NewGuid().ToString(),
            ApplicationUserId = user!.Id,
            Name = $"Property {Guid.NewGuid().ToString()}",
            Description = "Test property",
            Location = "123 Test St",
            PricePerNight = 105,
            ImagesUrl = []
        })).Entity;

        OrderBase? order = null;

        if (orderType == typeof(PromoteOrder))
        {
            order = _context.DbContext.PromoteOrder.Add(new PromoteOrder
            {
                OrderId = Guid.NewGuid().ToString(),
                PaymentId = payment.PaymentId,
                ApplicationUserId = user.Id,
                PropertyId = property.PropertyId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                State = OrderState.Paid,
                ApplicationUser = user,
                Payment = payment,
                Property = property
            }).Entity;
            _context.DbContext.Order.Add(new Order { OrderId = order.OrderId, Type = "Promote" });
        }
        else if (orderType == typeof(BookingOrder))
        {
            order = _context.DbContext.BookingOrder.Add(new BookingOrder
            {
                OrderId = Guid.NewGuid()
                    .ToString(),
                PaymentId = payment.PaymentId,
                ApplicationUserId = user.Id,
                PropertyId = property.PropertyId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                State = OrderState.Paid,
                ApplicationUser = user,
                Payment = payment,
                Property = property,
                NumberOfGuests = 5
            }).Entity;
            _context.DbContext.Order.Add(new Order { OrderId = order.OrderId, Type = "Booking" });
        }

        try
        {
            await _context.DbContext.SaveChangesAsync();
        }
        catch
        {
            return null;
        }

        return order;
    }

    [Fact]
    public async void GetOrder_Returns_NotFound_When_Order_Not_Exist()
    {
        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetOrder(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetOrder_Returns_Order_When_Order_Exist()
    {
        var order = await CreateRandomOrder(typeof(PromoteOrder));
        Assert.NotNull(order);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetOrder(order.OrderId);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void CreateOrderPromote_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var controller = CreateController();
        Assert.NotNull(controller);

        var order = await CreateRandomOrder(typeof(PromoteOrder));
        Assert.NotNull(order);
        if (order is PromoteOrder promoteOrder)
        {
            var result = await controller.CreateOrderPromote(new PropertyPromoteModel(
                promoteOrder.PropertyId,
                promoteOrder.StartDate,
                promoteOrder.EndDate,
                promoteOrder.Payment!.Method
            ), false);
            Assert.IsType<UnauthorizedResult>(result);
        }
        else
        {
            Assert.True(false);
        }
    }

    [Fact]
    public async void CreateOrderPromote_Returns_NotFound_When_Property_NotExist()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var order = await CreateRandomOrder(typeof(PromoteOrder));
        Assert.NotNull(order);

        if (order is PromoteOrder promoteOrder)
        {
            var result = await controller.CreateOrderPromote(new PropertyPromoteModel(
                Guid.NewGuid().ToString(),
                promoteOrder.StartDate,
                promoteOrder.EndDate,
                promoteOrder.Payment!.Method
            ), false);
            Assert.IsType<NotFoundResult>(result);
        }
        else
        {
            Assert.True(false);
        }
    }

    [Fact]
    public async void CreateOrderPromote_Returns_Ok_When_Payment_Success()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var result = await controller.CreateOrderPromote(new PropertyPromoteModel(
            property.PropertyId,
            DateTime.Now,
            DateTime.Now.AddDays(1),
            "Multibanco"
        ), false);
        Assert.IsType<CreatedAtActionResult>(result);
    }
}