using Microsoft.AspNetCore.Mvc;

namespace BookingBuddy.Server.Controllers
{
    public class GroupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
