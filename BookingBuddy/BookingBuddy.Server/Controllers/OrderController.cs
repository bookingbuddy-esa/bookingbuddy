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
    public class OrderController(
        BookingBuddyServerContext context,
        UserManager<ApplicationUser> userManager,
        PaymentController paymentController)
        : ControllerBase
    {
        /// <summary>
        /// Método que representa o endpoint de obtenção de uma order através do seu ID.
        /// </summary>
        /// <param name="orderId">Identificador da order</param>
        /// <param name="type">Tipo de order</param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            var order = await context.Order.FindAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return order.Type.ToUpper() switch
            {
                "PROMOTION" => Ok(await context.PromotionOrder.FindAsync(orderId)),
                "PROMOTE" => Ok(await context.PromoteOrder.Include(po => po.Payment)
                    .Where(po => po.OrderId == orderId)
                    .Select(po => new
                    {
                        po.OrderId,
                        ApplicationUser = new
                        {
                            po.ApplicationUser!.Id,
                            po.ApplicationUser.Name,
                            po.ApplicationUser.Email
                        },
                        po.PropertyId,
                        po.PaymentId,
                        po.StartDate,
                        po.EndDate,
                        State = po.State.GetDescription()
                    })
                    .FirstOrDefaultAsync()),
                "BOOKING" => Ok(await context.BookingOrder
                    .Where(bo => bo.OrderId == orderId)
                    .Include(bo => bo.Payment)
                    .Include(bo => bo.ApplicationUser)
                    .Select(bo => new
                    {
                        BookingOrderId = bo.OrderId,
                        ApplicationUser = new
                        {
                            bo.ApplicationUser!.Id,
                            bo.ApplicationUser.Name,
                            bo.ApplicationUser.Email
                        },
                        bo.PaymentId,
                        bo.StartDate,
                        bo.EndDate,
                        bo.NumberOfGuests,
                        State = bo.State.GetDescription()
                    })
                    .FirstOrDefaultAsync()),
                "GROUP-BOOKING" => Ok(await context.GroupBookingOrder.FindAsync(orderId)),
                _ => BadRequest()
            };
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
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var property = await context.Property.FindAsync(model.PropertyId);
                if (property == null)
                {
                    return NotFound();
                }

                if (model.StartDate > model.EndDate)
                {
                    return BadRequest("Invalid dates");
                }

                var newPaymentResult = await paymentController.CreatePayment(user, model.PaymentMethod,
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
                    context.Payment.Add(payment);
                    newPaymentResult = new ActionResult<Payment>(payment);
                }


                if (newPaymentResult is { Value: Payment newPayment })
                {
                    var order = new PromoteOrder
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        PaymentId = newPayment!.PaymentId,
                        ApplicationUserId = user.Id,
                        PropertyId = model.PropertyId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        State = OrderState.Pending
                    };
                    context.PromoteOrder.Add(order);
                    context.Order.Add(new Order { OrderId = order.OrderId, Type = "Promote" });
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while saving order to database: {ex.Message}");
                    }

                    return CreatedAtAction("GetOrder", new { orderId = order.OrderId }, new
                    {
                        order.OrderId,
                        ApplicationUser = new
                        {
                            order.ApplicationUser!.Id,
                            order.ApplicationUser.Name,
                            order.ApplicationUser.Email
                        },
                        Payment = new
                        {
                            order.Payment!.PaymentId,
                            order.Payment.Method,
                            order.Payment.Entity,
                            order.Payment.Reference,
                            order.Payment.Amount,
                            order.Payment.Status,
                            order.Payment.CreatedAt,
                            order.Payment.ExpiryDate
                        },
                        order.PropertyId,
                        order.StartDate,
                        order.EndDate,
                        State = order.State.GetDescription()
                    });
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
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var property = await context.Property.FindAsync(model.PropertyId);
                if (property == null)
                {
                    return NotFound();
                }

                if (model.StartDate > model.EndDate)
                {
                    return BadRequest("Invalid dates");
                }

                var selectedDates = new List<DateTime>();
                var pricesMap = new Dictionary<decimal, int>();

                for (var date = model.StartDate; date < model.EndDate; date = date.AddDays(1))
                {
                    selectedDates.Add(date);
                }

                foreach (var selectedDate in selectedDates)
                {
                    var matchingDiscount = await context.Discount.Where(d => d.PropertyId == model.PropertyId)
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
                var newPaymentResult = await paymentController.CreatePayment(user, model.PaymentMethod,
                    reservationAmount, model.PhoneNumber!);

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
                    context.Payment.Add(payment);
                    newPaymentResult = new ActionResult<Payment>(payment);
                }


                if (newPaymentResult is { Value: Payment newPayment })
                {
                    try
                    {
                        var order = new BookingOrder
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            NumberOfGuests = model.NumberOfGuests,
                            PaymentId = newPayment!.PaymentId,
                            ApplicationUserId = user.Id,
                            PropertyId = model.PropertyId,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                        };
                        context.BookingOrder.Add(order);
                        context.Order.Add(new Order { OrderId = order.OrderId, Type = "Booking" });
                        await context.SaveChangesAsync();
                        return CreatedAtAction("GetOrder", new { orderId = order.OrderId }, new
                        {
                            order.OrderId,
                            ApplicationUser = new
                            {
                                order.ApplicationUser!.Id,
                                order.ApplicationUser.Name,
                                order.ApplicationUser.Email
                            },
                            order.StartDate,
                            order.EndDate,
                            order.NumberOfGuests,
                            Payment = new
                            {
                                order.Payment!.PaymentId,
                                order.Payment.Method,
                                order.Payment.Entity,
                                order.Payment.Reference,
                                order.Payment.Amount,
                                order.Payment.Status,
                                order.Payment.CreatedAt,
                                order.Payment.ExpiryDate
                            },
                            State = order.State.GetDescription()
                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred while saving order to database: {ex.Message}");
                    }
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
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var property = await context.Property.FindAsync(model.PropertyId);
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
                    var matchingDiscount = await context.Discount.Where(d => d.PropertyId == model.PropertyId)
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
                        OrderId = Guid.NewGuid().ToString(),
                        ApplicationUserId = user.Id,
                        GroupId = model.GroupId,
                        PropertyId = model.PropertyId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        TotalAmount = reservationAmount,
                    };
                    context.GroupBookingOrder.Add(groupBookingOrder);
                    context.Order.Add(new Order { OrderId = groupBookingOrder.OrderId, Type = "GroupBooking" });
                    await context.SaveChangesAsync();
                    return CreatedAtAction("GetOrder", new { orderId = groupBookingOrder.OrderId }, groupBookingOrder);
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
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var groupBooking = await context.GroupBookingOrder.Include(gbo => gbo.Group)
                .FirstOrDefaultAsync(gbo => gbo.OrderId == model.GroupBookingId);
            if (groupBooking == null)
            {
                return NotFound();
            }

            if (groupBooking.Group == null)
            {
                BadRequest();
            }

            if (!groupBooking.Group!.MembersId.Contains(user.Id))
            {
                return Forbid();
            }

            if (groupBooking.PaidByIds.Contains(user.Id))
            {
                return BadRequest("Já pagou a sua parte.");
            }

            if (groupBooking.State == OrderState.Paid)
            {
                return BadRequest("Reserva já se encontra paga.");
            }

            var reservationAmount = Math.Round(groupBooking.TotalAmount / groupBooking.Group.MembersId.Count, 2);
            var newPaymentResult =
                await paymentController.CreatePayment(user, model.PaymentMethod, reservationAmount, model.PhoneNumber);

            if (newPaymentResult is not { Value: Payment newPayment }) return BadRequest();
            groupBooking.PaymentIds.Add(newPayment.PaymentId);
            await context.SaveChangesAsync();
            return Ok(newPayment);

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
        string GroupId,
        DateTime StartDate,
        DateTime EndDate
    );

    public record PayGroupBookingModel(
        string GroupBookingId,
        string PaymentMethod,
        string? PhoneNumber = null
    );
}