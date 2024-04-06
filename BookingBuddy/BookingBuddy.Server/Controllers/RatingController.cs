using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookingBuddy.Server.Services;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Classe que representa o controlador de classificações.
    /// </summary>
    [Route("api/ratings")]
    [ApiController]
    public class RatingController : Controller
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Construtor da classe RatingController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        public RatingController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // post: api/ratings
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Rating>> CreateRating([FromBody] RatingInputModel inputModel)
        {
            try {
                var user = await _userManager.GetUserAsync(User);
                if(user == null)
                {
                    return Unauthorized("Utilizador não autenticado.");
                }

                var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderId == inputModel.OrderId);
                if(order == null)
                {
                    return NotFound("Reserva não encontrada.");
                }

                // TODO: group booking classificaçao
                if(order.Type == "Group-Booking"){
                    Console.WriteLine("todo");
                } else if(order.Type == "Booking"){
                    var bookingOrder = await _context.BookingOrder.FirstOrDefaultAsync(bo => bo.OrderId == inputModel.OrderId);
                    if(bookingOrder == null)
                    {
                        return NotFound("Reserva não encontrada.");
                    }

                    if(bookingOrder.ApplicationUserId != user.Id)
                    {
                        return Unauthorized("Não tem permissão para classificar esta propriedade.");
                    }

                    var ratingExists = await _context.Rating.AnyAsync(r => r.PropertyId == bookingOrder.PropertyId && r.ApplicationUserId == user.Id);
                    if(ratingExists)
                    {
                        return BadRequest("Já classificou esta propriedade.");
                    }

                    if (inputModel.Rating < 1 || inputModel.Rating > 5)
                    {
                        return BadRequest("O valor da classificação deve ser um número entre 1 e 5.");
                    }

                    var rating = new Rating
                    {
                        RatingId = Guid.NewGuid().ToString(),
                        PropertyId = bookingOrder.PropertyId,
                        ApplicationUserId = user.Id,
                        Value = inputModel.Rating
                    };

                    _context.Rating.Add(rating);
                    await _context.SaveChangesAsync();
                } else {
                    return BadRequest("Tipo de reserva inválido.");
                }

                return Ok("Classificação guardada com sucesso!");
            } catch (Exception e) {
                return BadRequest("Ocorreu um erro ao guardar a classificação.");
            }
        }
    }

    /// <summary>
    /// Modelo utilizado para criar uma classificação.
    /// </summary>
    /// <param name="OrderId">Identificador da reserva</param>
    /// <param name="Rating">Valor da classificação</param>
    public record RatingInputModel(string OrderId, int Rating);
}
