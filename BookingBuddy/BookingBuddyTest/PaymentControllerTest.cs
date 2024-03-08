using System.Security.Claims;
using BookingBuddy.Server.Controllers;
using BookingBuddy.Server.Models;
using BookingBuddyTest.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookingBuddyTest;

public class PaymentControllerTest : IClassFixture<ApplicationDbContextFixture>
{
    private readonly ApplicationDbContextFixture _context;
    private readonly UserManagerFixture _userManager;

    public PaymentControllerTest(ApplicationDbContextFixture context)
    {
        _context = context;
        _userManager = new UserManagerFixture(_context);
    }

    private PaymentController CreateController(string? userId = null)
    {
        return new PaymentController(
            _context.DbContext,
            _userManager.UserManager,
            Mock.Of<IConfiguration>()
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

    [Theory]
    [InlineData("non-existing-id", typeof(NotFoundResult))]
    [InlineData(null, typeof(BadRequestObjectResult))]
    [InlineData("valid-id", typeof(OkObjectResult))]
    public async Task GetPayment_Returns_Expected_Result(string paymentId, Type resultType)
    {
        var controller = CreateController();
        Assert.NotNull(controller);

        if (paymentId == "valid-id")
        {
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                Amount = 100,
                Method = "Multibanco",
                Status = "Pendente",
                CreatedAt = DateTime.Now,
            };
            _context.DbContext.Payment.Add(payment);
            try
            {
                await _context.DbContext.SaveChangesAsync();
                paymentId = payment.PaymentId;
            }
            catch
            {
                Assert.True(false);
            }
        }

        var result = await controller.GetPayment(paymentId);
        Assert.IsType(resultType, result);
    }
}