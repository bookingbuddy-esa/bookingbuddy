using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("booking")]
        [Authorize]
        public async Task<IActionResult> CreateOrderBooking([FromBody] PropertyBookingModel model,
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

                if (model.StartDate > model.EndDate)
                {
                    return BadRequest("Invalid dates");
                }

                List<DateTime> selectedDates = new List<DateTime>();
                Dictionary<decimal, int> pricesMap = new Dictionary<decimal, int>();

                for (DateTime date = model.StartDate; date < model.EndDate; date = date.AddDays(1))
                {
                    selectedDates.Add(date);
                }

                foreach (DateTime selectedDate in selectedDates)
                {
                    var matchingDiscount = await _context.Discount.Where(d => d.PropertyId == model.PropertyId)
                        .Where(d => d.StartDate <= selectedDate && d.EndDate >= selectedDate)
                        .FirstOrDefaultAsync();

                    if (matchingDiscount != null)
                    {
                        decimal discountMultiplier = 1 - (decimal)matchingDiscount.DiscountAmount / 100;
                        decimal newPrice = Math.Round(property.PricePerNight * discountMultiplier, 2);
                        int currentCount = pricesMap.ContainsKey(newPrice) ? pricesMap[newPrice] : 0;
                        pricesMap[newPrice] = currentCount + 1;
                    }
                    else
                    {
                        decimal currentPrice = property.PricePerNight;
                        int currentCount = pricesMap.ContainsKey(currentPrice) ? pricesMap[currentPrice] : 0;
                        pricesMap[currentPrice] = currentCount + 1;
                    }
                }

                decimal reservationAmount = pricesMap.Sum(entry => entry.Key * entry.Value);
                var newPaymentResult = await _paymentController.CreatePayment(user, model.PaymentMethod,
                    reservationAmount, model.PhoneNumber);

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
                        OrderId = "BOOKING-" + Guid.NewGuid(),
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
                        var bookingOrder = new BookingOrder
                        {
                            BookingOrderId = Guid.NewGuid().ToString(),
                            OrderId = order.OrderId,
                            NumberOfGuests = model.NumberOfGuests
                        };

                        _context.BookingOrder.Add(bookingOrder);
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

        [HttpPost]
        [Authorize]
        [Route("group-booking")]
        public async Task<IActionResult> CreateGroupBooking([FromBody] GroupBookingModel model)
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

                if (model.StartDate > model.EndDate)
                {
                    return BadRequest("Invalid dates");
                }

                List<DateTime> selectedDates = [];
                Dictionary<decimal, int> pricesMap = new();

                for (var date = model.StartDate; date < model.EndDate; date = date.AddDays(1))
                {
                    selectedDates.Add(date);
                }

                foreach (var selectedDate in selectedDates)
                {
                    var matchingDiscount = await _context.Discount.Where(d => d.PropertyId == model.PropertyId)
                        .Where(d => d.StartDate <= selectedDate && d.EndDate >= selectedDate)
                        .FirstOrDefaultAsync();

                    if (matchingDiscount != null)
                    {
                        var discountMultiplier = 1 - (decimal)matchingDiscount.DiscountAmount / 100;
                        var newPrice = Math.Round(property.PricePerNight * discountMultiplier, 2);
                        var currentCount = pricesMap.TryGetValue(newPrice, out var value) ? value : 0;
                        pricesMap[newPrice] = currentCount + 1;
                    }
                    else
                    {
                        var currentPrice = property.PricePerNight;
                        var currentCount = pricesMap.TryGetValue(currentPrice, out var value) ? value : 0;
                        pricesMap[currentPrice] = currentCount + 1;
                    }
                }

                var reservationAmount = pricesMap.Sum(entry => entry.Key * entry.Value);

                try
                {
                    var groupBookingOrder = new GroupBookingOrder
                    {
                        GroupBookingOrderId = Guid.NewGuid().ToString(),
                        ApplicationUserId = user.Id,
                        MembersId = model.MembersId,
                        PropertyId = model.PropertyId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        TotalPrice = reservationAmount,
                        PaymentsId = [],
                        PaidById = [],
                        State = false
                    };

                    _context.GroupBookingOrder.Add(groupBookingOrder);
                    await _context.SaveChangesAsync();
                    var members = _context.Users.Where(u => model.MembersId.Contains(u.Id)).ToList();
                    var result = await _context.GroupBookingOrder.Where(gbo =>
                            gbo.GroupBookingOrderId == groupBookingOrder.GroupBookingOrderId)
                        .Include(gbo => gbo.Payments)
                        .Include(gbo => gbo.ApplicationUser)
                        .Select(gbo => new
                        {
                            gbo.GroupBookingOrderId,
                            ApplicationUser = new
                                { gbo.ApplicationUser!.Id, gbo.ApplicationUser.Name, gbo.ApplicationUser.Email },
                            Members = members.Select(m => new { m.Id, m.Name, m.Email }).ToList(),
                            gbo.PropertyId,
                            gbo.StartDate,
                            gbo.EndDate,
                            gbo.TotalPrice,
                            State = gbo.State ? "Paid" : "Pending"
                        })
                        .FirstOrDefaultAsync();
                    return Ok(result);
                }
                catch
                {
                    return StatusCode(500, $"Ocorreu um erro ao criar a reserva.");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        [Route("group-booking/pay")]
        public async Task<IActionResult> PayGroupBooking([FromBody] PayGroupBookingModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var groupBooking = await _context.GroupBookingOrder.FindAsync(model.GroupBookingId);
            if (groupBooking == null)
            {
                return NotFound();
            }
            
            if(!groupBooking.MembersId.Contains(user.Id))
            {
                return Forbid();
            }
            
            if(groupBooking.PaidById.Contains(user.Id))
            {
                return BadRequest("Já pagou a sua parte.");
            }
            
            if(groupBooking.State)
            {
                return BadRequest("Reserva já se encontra paga.");
            }
            
            var reservationAmount = Math.Round(groupBooking.TotalPrice / groupBooking.MembersId.Count, 2);
            var newPaymentResult = await _paymentController.CreatePayment(user, model.PaymentMethod, reservationAmount, model.PhoneNumber);

            if (newPaymentResult is { Value: Payment newPayment })
            {
                groupBooking.PaymentsId.Add(newPayment.PaymentId);
                await _context.SaveChangesAsync();
                return Ok(newPayment);
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
    /// <param name="PhoneNumber"></param>
    public record PropertyPromoteModel(
        string PropertyId,
        DateTime StartDate,
        DateTime EndDate,
        string PaymentMethod,
        string? PhoneNumber = null);


    /// <summary>
    /// Classe que representa o modelo de um pedido de reserva de uma propriedade.
    /// </summary>
    /// <param name="PropertyId"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <param name="PaymentMethod"></param>
    /// <param name="NumberOfGuests"></param>
    /// <param name="PhoneNumber"></param>
    public record PropertyBookingModel(
        string PropertyId,
        DateTime StartDate,
        DateTime EndDate,
        string PaymentMethod,
        int NumberOfGuests,
        string? PhoneNumber = null);

    public record GroupBookingModel(
        string PropertyId,
        DateTime StartDate,
        DateTime EndDate,
        List<string> MembersId
    );

    public record PayGroupBookingModel(
        string GroupBookingId,
        string PaymentMethod,
        string? PhoneNumber = null
    );
}