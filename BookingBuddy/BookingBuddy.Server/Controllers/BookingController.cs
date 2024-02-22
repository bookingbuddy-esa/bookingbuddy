using System.Text;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddy.Server.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor da classe BookingController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        /// <param name="configuration">Configuração da aplicação</param>
        public BookingController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }


        // create "dev" endpoint to create 5 bookings
        [HttpGet("dev")]
        [Authorize]
        public async Task<ActionResult> CreateBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            for (int i = 0; i < 5; i++)
            {
                // get random property id
                var randomPropertyId = _context.Property.Select(p => p.PropertyId).OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                // create dev payment
                var payment = new Payment
                {
                    PaymentId = "dev-"+Guid.NewGuid().ToString(),
                    Method = "mbway",
                    Amount = 100 + i*10,
                    Status = "Paid",
                    CreatedAt = DateTime.Now
                };
                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                // create dev order
                var order = new Order
                {
                    OrderId = "BOOKING-" + Guid.NewGuid().ToString(),
                    PaymentId = payment.PaymentId,
                    ApplicationUserId = user.Id,
                    PropertyId = randomPropertyId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(i+3),
                    State = true
                };
                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                // create dev booking order
                var bookingOrder = new BookingOrder
                {
                    BookingOrderId = Guid.NewGuid().ToString(),
                    OrderId = order.OrderId,
                    NumberOfGuests = i+1
                };
                _context.BookingOrder.Add(bookingOrder);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBookings()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var bookingOrders = await _context.BookingOrder
                .Include(bo => bo.Order)
                .Include(o => o.Order!.Payment)
                .Include(o => o.Order!.ApplicationUser)
                .Include(o => o.Order!.Property)
                .Where(bo => bo.Order!.ApplicationUserId == user.Id)
                .Select(bo => new {
                    bo.BookingOrderId,
                    bo.OrderId,
                    applicationUser = new ReturnUser(){
                        Id = bo.Order!.ApplicationUser.Id,
                        Name = bo.Order!.ApplicationUser.Name,
                    },
                    bo.Order!.Property!.Name,
                    host = bo.Order!.Property!.ApplicationUserId,
                    checkIn = bo.Order!.StartDate,
                    checkOut = bo.Order!.EndDate,
                    bo.Order!.State,
                    bo.Order.Payment!.Amount,
                    bo.NumberOfGuests,
                }).ToListAsync();

            return Ok(bookingOrders);
        }

        // Get all messages from a booking by bookingId
        [HttpGet("{bookingId}/messages")]
        [Authorize]
        public async Task<IActionResult> GetMessages(string bookingId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var bookingOrder = await _context.BookingOrder
                .Include(bo => bo.Order)
                .Include(o => o.Order!.ApplicationUser)
                .Where(bo => bo.BookingOrderId == bookingId)
                .FirstOrDefaultAsync();

            if (bookingOrder == null)
            {
                return NotFound();
            }

            if (bookingOrder.Order!.ApplicationUserId != user.Id)
            {
                return Unauthorized();
            }

            var messages = await _context.BookingMessage
                .Where(m => m.BookingOrderId == bookingOrder.BookingOrderId)
                .Select(m => new {
                    m.ApplicationUser.Name,
                    m.Message,
                    m.SentAt
                }).ToListAsync();

            return Ok(messages);
        }

        // Create a message for a booking
        [HttpPost("{bookingId}/messages")]
        [Authorize]
        public async Task<IActionResult> CreateMessage(string bookingId, [FromBody] NewBookingMessage message)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var bookingOrder = await _context.BookingOrder
                .Include(bo => bo.Order)
                .Include(o => o.Order!.ApplicationUser)
                .Where(bo => bo.BookingOrderId == bookingId)
                .FirstOrDefaultAsync();

            if (bookingOrder == null)
            {
                return NotFound();
            }

            if (bookingOrder.Order!.ApplicationUserId != user.Id)
            {
                return Unauthorized();
            }

            var newMessage = new BookingMessage
            {
                BookingMessageId = Guid.NewGuid().ToString(),
                BookingOrderId = bookingOrder.BookingOrderId,
                ApplicationUserId = user.Id,
                Message = message.Message,
                SentAt = DateTime.Now
            };

            _context.BookingMessage.Add(newMessage);
            await _context.SaveChangesAsync();

            return Ok();
        } 
    }

    public record NewBookingMessage(string Message);
}
