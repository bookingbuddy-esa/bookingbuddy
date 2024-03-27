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
        /// Obtém todas as reservas associadas ao utilizador autenticado.
        /// Inclui tanto as reservas individuais como as reservas em grupo em que o utilizador é membro.
        /// </summary>
        /// <returns>
        /// Um código de estado 200 (OK) com a lista de todas as reservas do utilizador.
        /// Um código de estado 401 (Não Autorizado) se o utilizador não estiver autenticado.
        /// </returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var individualBookings = _context.BookingOrder
                .Include(booking => booking.Property)
                .Include(booking => booking.Payment)
                .Where(booking => booking.ApplicationUserId == user.Id)
                .Select(booking => new
                {
                    orderId = booking.OrderId,
                    propertyName = booking.Property!.Name,
                    host =  new ReturnUser{
                        Id = booking.Property.ApplicationUserId,
                        Name = _userManager.FindByIdAsync(booking.Property.ApplicationUserId).Result!.Name
                    },
                    checkIn = booking.StartDate,
                    checkOut = booking.EndDate,
                    state = booking.State,
                    totalAmount = booking.Payment!.Amount,
                    numberOfGuests = booking.NumberOfGuests
                })
                .ToList();

            var groupBookings = _context.GroupBookingOrder
                .Include(groupBooking => groupBooking.Property)
                .Include(groupBooking => groupBooking.Group)
                .Where(groupBooking => groupBooking.Group.MembersId.Contains(user.Id))
                .Select(groupBooking => new
                {
                    orderId = groupBooking.OrderId,
                    propertyName = groupBooking.Property.Name,
                    host = new ReturnUser{
                        Id = groupBooking.Property.ApplicationUserId,
                        Name = _userManager.FindByIdAsync(groupBooking.Property.ApplicationUserId).Result!.Name
                    },
                    checkIn = groupBooking.StartDate,
                    checkOut = groupBooking.EndDate,
                    state = groupBooking.State,
                    totalAmount = groupBooking.TotalAmount,
                    numberOfGuests = groupBooking.Group.MembersId.Count
                })
                .ToList();

            var allBookings = individualBookings.Cast<object>().Concat(groupBookings.Cast<object>());
            return Ok(allBookings);
        }


        /// <summary>
        /// Obtém todas as mensagens associadas a uma reserva específica.
        /// </summary>
        /// <param name="bookingId">O identificador único da reserva.</param>
        /// <returns>
        /// Um código de estado 200 (OK) com a lista de mensagens associadas à reserva.
        /// Um código de estado 401 (Não Autorizado) se o utilizador não estiver autenticado.
        /// Um código de estado 404 (Não Encontrado) se a reserva não existir.
        /// </returns>

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
        /// Cria uma nova mensagem para uma reserva especificada.
        /// </summary>
        /// <param name="bookingId">O identificador único da reserva.</param>
        /// <param name="message">Os detalhes da nova mensagem.</param>
        /// <returns>
        /// Um código de estado 200 (OK) se a mensagem for criada com sucesso.
        /// Um código de estado 401 (Não Autorizado) se o utilizador não estiver autenticado.
        /// Um código de estado 404 (Não Encontrado) se a reserva não for encontrada.
        /// Um código de estado 400 (Pedido Inválido) se ocorrerem erros durante o processo.
        /// </returns>
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
                BookingOrderId = bookingOrder.OrderId,
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