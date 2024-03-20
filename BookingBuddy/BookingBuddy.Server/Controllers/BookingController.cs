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
        public BookingController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
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
                var randomPropertyId = _context.Property.Select(p => p.PropertyId).OrderBy(p => Guid.NewGuid())
                    .FirstOrDefault();

                // create dev payment
                var payment = new Payment
                {
                    PaymentId = "dev-" + Guid.NewGuid().ToString(),
                    Method = "mbway",
                    Amount = 100 + i * 10,
                    Status = "Paid",
                    CreatedAt = DateTime.Now
                };
                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                // create dev order
                var order = new BookingOrder
                {
                    OrderId = Guid.NewGuid().ToString(),
                    PaymentId = payment.PaymentId,
                    ApplicationUserId = user.Id,
                    NumberOfGuests = i + 1,
                    PropertyId = randomPropertyId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(i + 3),
                    State = OrderState.Paid
                };
                _context.BookingOrder.Add(order);
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
                .Include(o => o.Payment)
                .Include(o => o.ApplicationUser)
                .Include(o => o.Property)
                .Where(bo => bo.ApplicationUserId == user.Id)
                .Select(bo => new
                {
                    bo.OrderId,
                    applicationUser = new ReturnUser()
                    {
                        Id = bo.ApplicationUser!.Id,
                        Name = bo.ApplicationUser.Name,
                    },
                    bo.Property!.Name,
                    host = bo.Property!.ApplicationUserId,
                    checkIn = bo.StartDate,
                    checkOut = bo.EndDate,
                    bo.State,
                    bo.Payment!.Amount,
                    bo.NumberOfGuests,
                }).ToListAsync();

            return Ok(bookingOrders);
        }

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
                .Include(o => o.ApplicationUser)
                //.Include(o => o.Order!.Property)
                .Where(bo => bo.OrderId == bookingId)
                .FirstOrDefaultAsync();

            if (bookingOrder == null)
            {
                return NotFound();
            }

            /* TODO: esta verificação será necessária
            if (bookingOrder.Order!.ApplicationUserId != user.Id || bookingOrder.Order!.Property!.ApplicationUserId != user.Id)
            {
                return Unauthorized();
            }*/

            var messages = await _context.BookingMessage
                .Where(m => m.BookingOrderId == bookingOrder.OrderId)
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    m.ApplicationUser.Name,
                    m.Message,
                    m.SentAt
                }).ToListAsync();

            return Ok(messages);
        }

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
                .Include(o => o.ApplicationUser)
                //.Include(o => o.Order!.Property)
                .Where(bo => bo.OrderId == bookingId)
                .FirstOrDefaultAsync();

            if (bookingOrder == null)
            {
                return NotFound();
            }

            /*if (bookingOrder.Order!.ApplicationUserId != user.Id || bookingOrder.Order!.Property!.ApplicationUserId != user.Id)
            {
                return Unauthorized();
            }*/

            var newMessage = new BookingMessage
            {
                BookingMessageId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                Message = message.Message,
                SentAt = DateTime.Now
            };

            _context.BookingMessage.Add(newMessage);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }

    public record NewBookingMessage(string Message);
}