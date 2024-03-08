using System.Text;
using System.Web;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Classe que representa o controlador de gestão de orders.
    /// </summary>
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaymentController _paymentController;

        public OrderController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager,
            PaymentController paymentController)
        {
            _context = context;
            _userManager = userManager;
            _paymentController = paymentController;
        }

        /// <summary>
        /// Método que representa o endpoint de obtenção de uma order através do seu ID.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            var order = await _context.Order.FindAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        /// <summary>
        /// Método que representa o endpoint de criação de um pedido de promover uma propriedade.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createPayment">Especifica se deve ser criado um pagamento</param>
        /// <returns></returns>
        [HttpPost("promote")]
        [Authorize]
        public async Task<IActionResult> CreateOrderPromote([FromBody] PropertyPromoteModel model,
            bool createPayment = true)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var property = await _context.Property.FindAsync(model.PropertyId);
                if (property == null)
                {
                    return NotFound();
                }

                var newPaymentResult = await _paymentController.CreatePayment(user, model.PaymentMethod,
                    Math.Round(GetPromoteAmount(model.StartDate, model.EndDate)), model.PhoneNumber);
                if (!createPayment)
                {
                    var payment = new Payment
                    {
                        PaymentId = Guid.NewGuid().ToString(),
                        Method = model.PaymentMethod,
                        Entity = model.PaymentMethod == "Multibanco" ? $"{new Random().Next(10000, 99999)}" : null,
                        Reference = model.PaymentMethod == "Multibanco"
                            ? $"{new Random().Next(10000000, 99999999)}"
                            : null,
                        Amount = new Random().Next(),
                        Status = "Pending",
                        CreatedAt = DateTime.Now,
                        ExpiryDate = DateTime.Now.AddDays(1).ToLongDateString(),
                    };
                    _context.Payment.Add(payment);
                    newPaymentResult = new ActionResult<Payment>(payment);
                }


                if (newPaymentResult is { Value: Payment newPayment })
                {
                    var order = new Order
                    {
                        OrderId = "PROMOTE-" + Guid.NewGuid(),
                        PaymentId = newPayment!.PaymentId,
                        ApplicationUserId = user.Id,
                        PropertyId = model.PropertyId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        State = false
                    };
                    _context.Order.Add(order);

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while saving order to database: {ex.Message}");
                    }

                    order.ApplicationUser = null;
                    order.Property = null;
                    return Ok(order);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating order: {ex.Message}");
            }

            return BadRequest();
        }

        private decimal GetPromoteAmount(DateTime startingDate, DateTime endingDate)
        {
            if (startingDate > endingDate)
            {
                throw new Exception("Invalid dates");
            }

            var duration = (endingDate - startingDate).TotalDays;

            if (duration >= 7)
            {
                return Convert.ToDecimal(5 * duration);
            }
            else if (duration >= 30)
            {
                return Convert.ToDecimal(4.5 * duration);
            }
            else if (duration >= 365)
            {
                return Convert.ToDecimal(3.5 * duration);
            }
            else
            {
                return Convert.ToDecimal(5 * duration);
            }
        }
    }

    /// <summary>
    /// Classe que representa o modelo de um pedido de promoção de uma propriedade.
    /// </summary>
    /// <param name="PropertyId"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <param name="PaymentMethod"></param>
    public record PropertyPromoteModel(
        string PropertyId,
        DateTime StartDate,
        DateTime EndDate,
        string PaymentMethod,
        string? PhoneNumber = null);
}