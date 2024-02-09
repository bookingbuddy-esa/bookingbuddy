using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookingBuddy.Server.Controllers
{
    [Route("api/blockedDates")]
    [ApiController]
    public class BlockedDatesController : Controller
    {
        private readonly BookingBuddyServerContext _context;

        public BlockedDatesController(BookingBuddyServerContext context)
        {
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BlockedDate>>> GetBlockedDates()
        {
            return await _context.BlockedDate.ToListAsync();
        }
    }
}
