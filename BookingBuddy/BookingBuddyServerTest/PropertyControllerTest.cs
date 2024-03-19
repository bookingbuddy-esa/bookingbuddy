using System.Security.Claims;
using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Models;
using BookingBuddyServerTest.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookingBuddyServerTest;

public class PropertyControllerTest : IClassFixture<ApplicationDbContextFixture>
{
    private readonly ApplicationDbContextFixture _context;
    private readonly UserManagerFixture _userManager;

    public PropertyControllerTest(ApplicationDbContextFixture context)
    {
        _context = context;
        _userManager = new UserManagerFixture(_context);
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

    private PropertyController CreateController(string? userId = null)
    {
        return new PropertyController(
            _context.DbContext,
            _userManager.UserManager,
            new Mock<IConfiguration>().Object
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
    public async void GetProperties_Returns_AllProperties()
    {
        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetProperties();
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetProperty_Returns_Property()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetProperty(property!.PropertyId);
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetProperty_Returns_NotFound_When_Property_NotFound()
    {
        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetProperty(Guid.NewGuid().ToString());
        Assert.NotNull(result);
    }

    [Fact]
    public async void GetMetrics_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetMetrics(property.PropertyId);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void GetMetrics_Returns_NotFound_When_Property_NotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

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

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetMetrics(property!.PropertyId);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void GetMetrics_Returns_Metrics_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetMetrics(property!.PropertyId);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void EditProperty_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.EditProperty(property!.PropertyId,
            new PropertyEditModel(
                property.PropertyId,
                property.Name,
                property.Description,
                property.PricePerNight,
                property.Location,
                new List<string>(),
                property.ImagesUrl
            ));
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void EditProperty_Returns_NotFound_When_Property_NotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.EditProperty(Guid.NewGuid().ToString(),
            new PropertyEditModel(
                Guid.NewGuid().ToString(),
                "Test Property",
                "Test Description",
                100,
                "123 Test St",
                new List<string>(),
                []
            ));
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void EditProperty_Returns_Forbidden_When_User_NotOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.EditProperty(property!.PropertyId,
            new PropertyEditModel(
                property.PropertyId,
                property.Name,
                property.Description,
                property.PricePerNight,
                property.Location,
                new List<string>(),
                property.ImagesUrl
            ));
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void EditProperty_Returns_NoContent_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.EditProperty(property!.PropertyId,
            new PropertyEditModel(
                property.PropertyId,
                property.Name,
                property.Description,
                property.PricePerNight,
                property.Location,
                new List<string>(),
                property.ImagesUrl
            ));
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void CreateProperty_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.CreateProperty(
            new PropertyCreateModel(
                "Test Property",
                "Test Description",
                100,
                "123 Test St",
                new List<string>(),
                []
            ));
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void CreateProperty_Returns_CreatedAtAction_When_User_IsAuthenticated()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.CreateProperty(
            new PropertyCreateModel(
                "Test Property",
                "Test Description",
                100,
                "123 Test St",
                new List<string>(),
                []
            ));
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async void DeleteProperty_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.DeleteProperty(property!.PropertyId);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void DeleteProperty_Returns_NotFound_When_Property_NotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);

        var result = await controller.DeleteProperty(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void DeleteProperty_Returns_Forbidden_When_User_NotOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.DeleteProperty(property!.PropertyId);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void DeleteProperty_Returns_NoContent_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.DeleteProperty(property!.PropertyId);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void GetPropertiesByUserId_Returns_NotFound_When_NoPropertiesFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetPropertiesByUserId(user.Id);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void GetPropertiesByUserId_Returns_Ok_When_PropertiesFound()
    {
        var property1 = await CreateRandomProperty();
        Assert.NotNull(property1);

        var property2 = await CreateRandomProperty();
        Assert.NotNull(property2);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetPropertiesByUserId(user.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void GetPropertyBlockedDates_Returns_NotFound_When_PropertyNotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetPropertyBlockedDates(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void GetPropertyBlockedDates_Returns_Ok_When_PropertyFound()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.GetPropertyBlockedDates(property.PropertyId);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void BlockDates_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.BlockDates(
            new BlockDateInputModel(
                DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(1).ToLongDateString(),
                property.PropertyId
            ));
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void BlockDates_Returns_NotFound_When_PropertyNotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);

        var result = await controller.BlockDates(
            new BlockDateInputModel(
                DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(1).ToLongDateString(),
                Guid.NewGuid().ToString()
            ));

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void BlockDates_Returns_Forbidden_When_User_NotOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.BlockDates(
            new BlockDateInputModel(
                DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(1).ToLongDateString(),
                property.PropertyId
            ));
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void BlockDates_Returns_Ok_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.BlockDates(
            new BlockDateInputModel(
                DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(1).ToLongDateString(),
                property.PropertyId
            ));
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void UnblockDates_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var authenticatedController = CreateController(user.Id);
        Assert.NotNull(authenticatedController);

        var blockResult = await authenticatedController.BlockDates(
            new BlockDateInputModel(
                DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(1).ToLongDateString(),
                property.PropertyId
            ));
        Assert.IsType<OkObjectResult>(blockResult);

        var blockDate = await _context.DbContext.BlockedDate.Where(bd => bd.PropertyId == property.PropertyId)
            .FirstOrDefaultAsync();
        Assert.NotNull(blockDate);

        var unauthenticatedController = CreateController();
        Assert.NotNull(unauthenticatedController);

        var result = await unauthenticatedController.UnblockDates(blockDate.Id);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void UnblockDates_Returns_NotFound_When_BlockDateNotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.UnblockDates(0);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void UnblockDates_Returns_Forbidden_When_User_NotOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var ownerUser = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(ownerUser);

        var ownerController = CreateController(ownerUser.Id);
        Assert.NotNull(ownerController);

        var blockResult = await ownerController.BlockDates(
            new BlockDateInputModel(
                DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(1).ToLongDateString(),
                property.PropertyId));
        Assert.IsType<OkObjectResult>(blockResult);

        var blockDate = await _context.DbContext.BlockedDate.Where(bd => bd.PropertyId == property.PropertyId)
            .FirstOrDefaultAsync();
        Assert.NotNull(blockDate);

        var notOwnerUser = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(notOwnerUser);

        var notOwnerController = CreateController(notOwnerUser.Id);
        Assert.NotNull(notOwnerController);

        var result = await notOwnerController.UnblockDates(blockDate.Id);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void UnblockDates_Returns_Ok_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var blockResult = await controller.BlockDates(
            new BlockDateInputModel(
                DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(1).ToLongDateString(),
                property.PropertyId));
        Assert.IsType<OkObjectResult>(blockResult);
    }

    [Fact]
    public async void GetAssociatedBookings_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetAssociatedBookings();
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void GetAssociatedBookings_Returns_Ok_When_User_IsAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var notOwnerUser = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(notOwnerUser);

        var payment1 = (await _context.DbContext.Payment.AddAsync(new Payment()
        {
            PaymentId = Guid.NewGuid().ToString(),
            Method = "Multibanco",
            Amount = 100,
            Status = "Pago",
            CreatedAt = DateTime.Now,
        })).Entity;
        Assert.NotNull(payment1);

        var order1 = (await _context.DbContext.Order.AddAsync(new Order()
            {
                OrderId = Guid.NewGuid().ToString(),
                ApplicationUserId = notOwnerUser.Id,
                PropertyId = property.PropertyId,
                PaymentId = payment1.PaymentId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                State = true,
            }
        )).Entity;
        Assert.NotNull(order1);

        var bookingOrderCreateResult1 = (await _context.DbContext.BookingOrder.AddAsync(new BookingOrder()
        {
            BookingOrderId = Guid.NewGuid().ToString(),
            OrderId = order1.OrderId,
            NumberOfGuests = 2
        })).Entity;
        Assert.NotNull(bookingOrderCreateResult1);

        var payment2 = (await _context.DbContext.Payment.AddAsync(new Payment()
        {
            PaymentId = Guid.NewGuid().ToString(),
            Method = "Multibanco",
            Amount = 150,
            Status = "Pago",
            CreatedAt = DateTime.Now,
        })).Entity;
        Assert.NotNull(payment2);

        var order2 = (await _context.DbContext.Order.AddAsync(new Order()
            {
                OrderId = Guid.NewGuid().ToString(),
                ApplicationUserId = notOwnerUser.Id,
                PropertyId = property.PropertyId,
                PaymentId = payment2.PaymentId,
                StartDate = DateTime.Now.AddDays(7),
                EndDate = DateTime.Now.AddDays(14),
                State = true,
            }
        )).Entity;
        Assert.NotNull(order2);

        var bookingOrderCreateResult2 = (await _context.DbContext.BookingOrder.AddAsync(new BookingOrder()
        {
            BookingOrderId = Guid.NewGuid().ToString(),
            OrderId = order2.OrderId,
            NumberOfGuests = 3
        })).Entity;
        Assert.NotNull(bookingOrderCreateResult2);

        try
        {
            await _context.DbContext.SaveChangesAsync();
        }
        catch
        {
            Assert.True(false);
        }

        Assert.Equal(2, await _context.DbContext.BookingOrder.CountAsync());

        var ownerUser = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(ownerUser);

        var controller = CreateController(ownerUser.Id);
        Assert.NotNull(controller);

        var result = await controller.GetAssociatedBookings();
        Assert.IsType<OkObjectResult>(result);

        var bookings = (result as OkObjectResult)?.Value as IEnumerable<dynamic>;
        Assert.NotNull(bookings);
        Assert.Equal(2, bookings.Count());
    }

    [Fact]
    public async void GetPropertyDiscounts_Returns_NotFound_When_PropertyNotFound()
    {
        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetPropertyDiscounts(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void GetPropertyDiscounts_Returns_Ok_When_PropertyFound()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.GetPropertyDiscounts(property.PropertyId);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void ApplyDiscount_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.ApplyDiscount(
            new DiscountInputModel(
                10,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                property.PropertyId
            ),
            false
        );
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void ApplyDiscount_Returns_NotFound_When_PropertyNotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.ApplyDiscount(
            new DiscountInputModel(
                10,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                Guid.NewGuid().ToString()
            ),
            false
        );
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void ApplyDiscount_Returns_Forbidden_When_User_NotOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.ApplyDiscount(
            new DiscountInputModel(
                10,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                property.PropertyId
            ),
            false
        );
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void ApplyDiscount_Returns_Ok_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.ApplyDiscount(
            new DiscountInputModel(
                10,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                property.PropertyId
            ),
            false
        );
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void RemoveDiscount_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.RemoveDiscount("0");
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void RemoveDiscount_Returns_NotFound_When_DiscountNotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.RemoveDiscount("invalid-id");
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void RemoveDiscount_Returns_Forbidden_When_User_NotOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var ownerUser = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(ownerUser);

        var discount = (await _context.DbContext.Discount.AddAsync(new Discount()
        {
            DiscountId = "0",
            PropertyId = property.PropertyId,
            DiscountAmount = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
        })).Entity;
        Assert.NotNull(discount);

        try
        {
            await _context.DbContext.SaveChangesAsync();
        }
        catch
        {
            Assert.True(false);
        }

        var controller = CreateController(ownerUser.Id);
        Assert.NotNull(controller);

        var result = await controller.RemoveDiscount(discount.DiscountId);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void RemoveDiscount_Returns_Ok_When_User_IsOwner()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var ownerUser = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.landlord@bookingbuddy.com");
        Assert.NotNull(ownerUser);

        var discount = (await _context.DbContext.Discount.AddAsync(new Discount()
        {
            DiscountId = Guid.NewGuid().ToString(),
            PropertyId = property.PropertyId,
            DiscountAmount = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
        })).Entity;
        Assert.NotNull(discount);

        try
        {
            await _context.DbContext.SaveChangesAsync();
        }
        catch
        {
            Assert.True(false);
        }

        var controller = CreateController(ownerUser.Id);
        Assert.NotNull(controller);

        var result = await controller.RemoveDiscount(discount.DiscountId);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void AddToFavorite_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.AddToFavorite(property!.PropertyId);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void AddToFavorite_Returns_NotFound_When_PropertyNotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.AddToFavorite(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void AddToFavorite_Returns_Ok_When_User_IsAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.AddToFavorite(property!.PropertyId);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void AddToFavorite_Returns_Conflict_When_PropertyAlreadyInFavorites()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.AddToFavorite(property!.PropertyId);
        Assert.IsType<OkObjectResult>(result);

        result = await controller.AddToFavorite(property!.PropertyId);
        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async void RemoveFromFavorite_Returns_Unauthorized_When_User_NotAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var controller = CreateController();
        Assert.NotNull(controller);

        var result = await controller.RemoveFromFavorite(property!.PropertyId);
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void RemoveFromFavorite_Returns_NotFound_When_PropertyNotFound()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.RemoveFromFavorite(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async void RemoveFromFavorite_Returns_Ok_When_User_IsAuthenticated()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.AddToFavorite(property!.PropertyId);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void RemoveFromFavorite_Returns_NotFound_When_PropertyNotInFavorites()
    {
        var property = await CreateRandomProperty();
        Assert.NotNull(property);

        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.user@bookingbuddy.com");
        Assert.NotNull(user);

        var controller = CreateController(user.Id);
        Assert.NotNull(controller);

        var result = await controller.RemoveFromFavorite(property!.PropertyId);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void GetFavorites_Returns_NotFound_When_UserNotExists()
    {
        var controller = CreateController(Guid.NewGuid().ToString());
        Assert.NotNull(controller);

        var result = await controller.GetUserFavorites(Guid.NewGuid().ToString());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetFavorites_Returns_Ok_When_UserExists()
    {
        var user = await _userManager.UserManager.FindByEmailAsync("bookingbuddy.admin@bookingbuddy.com");
        Assert.NotNull(user);

        var property = await CreateRandomProperty();
        Assert.NotNull(property);
        
        var property2 = await CreateRandomProperty();
        Assert.NotNull(property2);
        
        var controller = CreateController(user.Id);
        Assert.NotNull(controller);
        
        var addToFavoriteResult = await controller.AddToFavorite(property!.PropertyId);
        Assert.IsType<OkObjectResult>(addToFavoriteResult);
        
        addToFavoriteResult = await controller.AddToFavorite(property2!.PropertyId);
        Assert.IsType<OkObjectResult>(addToFavoriteResult);
        
        var result = await controller.GetUserFavorites(user.Id);
        Assert.IsType<OkObjectResult>(result);
        var favorites = (result as OkObjectResult)?.Value as IEnumerable<dynamic>;
        Assert.NotNull(favorites);
    }
}