using Microsoft.AspNetCore.Identity;

namespace BookingBuddy.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
    }
}
