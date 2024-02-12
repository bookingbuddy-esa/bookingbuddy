using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        
        [HttpGet("property/{propertyId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BlockedDate>>> GetPropertyBlockedDates(string propertyId)
        {
           var blockedDates = await _context.BlockedDate
            .Where(b => b.PropertyId == propertyId)
            .ToListAsync();

            if (blockedDates == null || blockedDates.Count == 0)
            {
                return NotFound("Nenhuma propriedade encontrada para o usuário fornecido.");
            }

            return blockedDates;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BlockedDate>>> GetBlockedDates(string propertyId)
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
                 End = inputModel.EndDate,
                 PropertyId= inputModel.PropertyId
            };

             _context.BlockedDate.Add(blockedDate);
             await _context.SaveChangesAsync();

            return Ok("Dates blocked successfully");
        }

        [HttpDelete("unblock/{id}")]
        public async Task<IActionResult> UnblockDates(int id)
        {
            var blockedDate = await _context.BlockedDate.FindAsync(id);

            if (blockedDate == null)
            {
                return NotFound();
            }

            _context.BlockedDate.Remove(blockedDate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class BlockDateInputModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PropertyId {  get; set; }
    }
}
