using System.Text;
using System.Web;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Classe que representa o controlador de gestão de orders.
    /// </summary>
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrderPromotion([FromBody] PropertyPromoteModel model)
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

            // TODO: criar pagamento

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                PropertyId = model.PropertyId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                State = false
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

    }

    public record PropertyPromoteModel(string PropertyId, DateTime StartDate, DateTime EndDate);
}