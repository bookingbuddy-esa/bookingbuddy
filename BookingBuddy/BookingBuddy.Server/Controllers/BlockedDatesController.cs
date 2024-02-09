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

        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> BlockDates([FromBody] BlockDateInputModel inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input");
            }

            var blockedDate = new BlockedDate
            {
                 Start = inputModel.StartDate,
                 End = inputModel.EndDate
            };

             _context.BlockedDate.Add(blockedDate);
             await _context.SaveChangesAsync();

            return Ok("Dates blocked successfully");
        }
    }

    public class BlockDateInputModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
