using BookingBuddy.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingBuddy.Server.Controllers;

/// <summary>
/// Classe que representa o controlador de administração.
/// </summary>
[Route("api/admin")]
[Authorize(Roles = "admin")]
[ApiController]
public class AdminController(BookingBuddyServerContext context) : ControllerBase
{
    /// <summary>
    /// Elimina todos os dados da base de dados e adiciona dados predefinidos.
    /// </summary>
    /// <returns>Um código de estado 200 (OK) se a base de dados for reiniciada com sucesso.</returns>
    [HttpPost("reset-database")]
    public async Task<IActionResult> ResetDatabase()
    {
        try
        {
            await context.ResetDatabase();
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }
}