using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                        State = po.State.AsString()
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
                        State = bo.State.AsString()
                    })
                    .FirstOrDefaultAsync()),
                "GROUP-BOOKING" => await GetBookingOrder(),
                _ => NotFound()
            };

            async Task<IActionResult> GetBookingOrder()
            {
                var groupBookingOrder = await context.GroupBookingOrder
                    .Where(gbo => gbo.OrderId == orderId)
                    .Include(gbo => gbo.ApplicationUser)
                    .Include(gbo => gbo.Group)
                    .FirstOrDefaultAsync();
                var group = groupBookingOrder!.Group;
                var members = new List<ApplicationUser>();
                foreach (var memberId in group!.MembersId)
                {
                    var member = await context.Users.FindAsync(memberId);
                    members.Add(member!);
                }

                var paidBy = new List<ApplicationUser>();
                foreach (var paymentId in groupBookingOrder.PaidByIds)
                {
                    var user = await context.Users.FindAsync(paymentId);
                    paidBy.Add(user!);
                }

                return Ok(new
                {
                    groupBookingOrder.OrderId,
                    ApplicationUser = new
                    {
                        groupBookingOrder.ApplicationUser!.Id,
                        groupBookingOrder.ApplicationUser.Name,
                        groupBookingOrder.ApplicationUser.Email
                    },
                    Group = new
                    {
                        group.GroupId,
                        group.Name,
                        Owner = new
                        {
                            group.GroupOwnerId,
                            members.First(m => m.Id == group.GroupOwnerId).Name,
                            members.First(m => m.Id == group.GroupOwnerId).Email
                        },
                        Members = members.Select(m => new
                        {
                            m.Id,
                            m.Name,
                            m.Email
                        })
                    },
                    groupBookingOrder.PropertyId,
                    groupBookingOrder.StartDate,
                    groupBookingOrder.EndDate,
                    groupBookingOrder.TotalAmount,
                    PaidBy = paidBy.Select(u => new
                    {
                        u.Id,
                        u.Name,
                        u.Email
                    }).ToList(),
                    State = groupBookingOrder.State.AsString()
                });
            }
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
                    Math.Round(GetPromoteAmount(model.StartDate, model.EndDate)), model.PhoneNumber!);
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


                if (newPaymentResult is { Value: { } newPayment })
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
                        State = order.State.AsString()
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating order: {ex.Message}");
            }

            return BadRequest();
        }

        /// <summary>
        /// Cria uma reserva individual.
        /// </summary>
        /// <param name="model">Modelo de reserva individual</param>
        /// <param name="createPayment">Especifica se deve ser criado um pagamento</param>
        /// <returns>
        /// Retorna um IActionResult indicando o resultado da operação:
        /// - 201 Created: Se a reserva foi criada com sucesso
        /// - 400 Bad Request: Se as datas são inválidas
        /// - 401 Unauthorized: Se o utilizador não está autenticado
        /// - 404 Not Found: Se a propriedade não foi encontrada
        /// - 500 Internal Server Error: Se ocorreu um erro ao criar a reserva
        /// </returns>
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


                if (newPaymentResult is { Value: { } newPayment })
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
                            State = order.State.AsString()
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

        /// <summary>
        /// Cria uma reserva de grupo.
        /// </summary>
        /// <param name="model">Modelo de reserva de grupo</param>
        /// <returns>
        /// Retorna um IActionResult indicando o resultado da operação:
        /// - 201 Created: Se a reserva foi criada com sucesso
        /// - 400 Bad Request: Se as datas são inválidas
        /// - 401 Unauthorized: Se o utilizador não está autenticado
        /// - 404 Not Found: Se a propriedade não foi encontrada
        /// - 500 Internal Server Error: Se ocorreu um erro ao criar a reserva
        /// </returns>
        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                var group = await context.Groups.FindAsync(model.GroupId);
                if (group == null)
                {
                    return NotFound("Grupo não encontrado");
                }

                if (group.ChoosenProperty.IsNullOrEmpty())
                {
                    return BadRequest("Propriedade não escolhida");
                }

                var property = await context.Property.FindAsync(group.ChoosenProperty);
                if (property == null)
                {
                    return NotFound("Propriedade não encontrada");
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
                    var matchingDiscount = await context.Discount.Where(d => d.PropertyId == group.ChoosenProperty)
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
                        PropertyId = group.ChoosenProperty!,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        TotalAmount = reservationAmount,
                    };
                    context.GroupBookingOrder.Add(groupBookingOrder);
                    context.Order.Add(new Order { OrderId = groupBookingOrder.OrderId, Type = "Group-Booking" });
                    await context.SaveChangesAsync();

                    List<ApplicationUser> members = [];
                    foreach (var memberId in group.MembersId)
                    {
                        var member = await context.Users.FindAsync(memberId);
                        members.Add(member!);
                    }

                    return CreatedAtAction("GetOrder", new { orderId = groupBookingOrder.OrderId }, new
                    {
                        groupBookingOrder.OrderId,
                        ApplicationUser = new
                        {
                            groupBookingOrder.ApplicationUser!.Id,
                            groupBookingOrder.ApplicationUser.Name,
                            groupBookingOrder.ApplicationUser.Email
                        },
                        Group = new
                        {
                            group.GroupId,
                            group.Name,
                            Owner = new
                            {
                                group.GroupOwnerId,
                                members.First(m => m.Id == group.GroupOwnerId).Name,
                                members.First(m => m.Id == group.GroupOwnerId).Email
                            },
                            Members = members.Select(m => new
                            {
                                m.Id,
                                m.Name,
                                m.Email
                            })
                        },
                        groupBookingOrder.PropertyId,
                        groupBookingOrder.StartDate,
                        groupBookingOrder.EndDate,
                        groupBookingOrder.TotalAmount,
                        State = groupBookingOrder.State.AsString()
                    });
                }
                catch
                {
                    return StatusCode(500, "Ocorreu um erro ao criar a reserva.");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Método que permite pagar uma parte de uma reserva de grupo.
        /// </summary>
        /// <param name="model">Modelo de pagamento de reserva de grupo</param>
        /// <returns>
        /// Retorna um IActionResult indicando o resultado da operação:
        /// - 200 OK: Se o pagamento foi efetuado com sucesso
        /// - 400 Bad Request: Se o utilizador já pagou a sua parte ou a reserva já se encontra paga
        /// - 401 Unauthorized: Se o utilizador não está autenticado
        /// - 403 Forbidden: Se o utilizador não faz parte do grupo
        /// - 404 Not Found: Se a reserva de grupo não foi encontrada
        /// - 500 Internal Server Error: Se ocorreu um erro ao efetuar o pagamento
        /// </returns>
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
                await paymentController.CreatePayment(user, model.PaymentMethod, reservationAmount, model.PhoneNumber!);

            if (newPaymentResult is not { Value: { } newPayment }) return BadRequest();
            groupBooking.PaymentIds.Add(newPayment.PaymentId);
            await context.SaveChangesAsync();
            return Ok(newPayment);
        }

        private static decimal GetPromoteAmount(DateTime startingDate, DateTime endingDate)
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

            if (duration >= 30)
            {
                return Convert.ToDecimal(4.5 * duration);
            }

            return duration >= 365 ? Convert.ToDecimal(3.5 * duration) : Convert.ToDecimal(5 * duration);
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

    /// <summary>
    /// Modelo de reserva de grupo.
    /// </summary>
    /// <param name="GroupId">Identificador do grupo</param>
    /// <param name="StartDate">Data de início da reserva</param>
    /// <param name="EndDate">Data de fim da reserva</param>
    public record GroupBookingModel(
        string GroupId,
        DateTime StartDate,
        DateTime EndDate
    );

    /// <summary>
    /// Modelo de pagamento de reserva de grupo.
    /// </summary>
    /// <param name="GroupBookingId">Identificador da reserva de grupo</param>
    /// <param name="PaymentMethod">Método de pagamento</param>
    /// <param name="PhoneNumber">Número de telefone</param>
    public record PayGroupBookingModel(
        string GroupBookingId,
        string PaymentMethod,
        string? PhoneNumber = null
    );
}