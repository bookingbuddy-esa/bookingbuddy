using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Controlador para as reservas.
    /// </summary>
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Construtor da classe BookingController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        public BookingController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Obtém as reservas do utilizador.
        /// </summary>
        /// <returns>Retorna as reservas do utilizador.</returns>

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

        /// <summary>
        /// Obtém as mensagens relacionadas a uma reserva específica.
        /// </summary>
        /// <param name="bookingId">O ID da reserva para a qual as mensagens serão obtidas.</param>
        /// <returns>Retorna as mensagens relacionadas à reserva.</returns>
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
                    m.ApplicationUser!.Name,
                    m.Message,
                    m.SentAt
                }).ToListAsync();

            return Ok(messages);
        }

        /// <summary>
        /// Cria uma nova mensagem relacionada a uma reserva específica.
        /// </summary>
        /// <param name="bookingId">O ID da reserva à qual a mensagem será adicionada.</param>
        /// <param name="message">Os dados da nova mensagem a ser criada.</param>
        /// <returns>Retorna o resultado da criação da mensagem.</returns>

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

    /// <summary>
    /// Representa os dados de uma nova mensagem relacionada a uma reserva.
    /// </summary>
    /// <param name="Message">O conteúdo da mensagem.</param>
    public record NewBookingMessage(string Message);
}