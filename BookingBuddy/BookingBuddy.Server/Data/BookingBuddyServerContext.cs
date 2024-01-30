using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Models;

namespace BookingBuddy.Server.Data
{
    public class BookingBuddyServerContext : IdentityDbContext<ApplicationUser>
    {
        public BookingBuddyServerContext(DbContextOptions<BookingBuddyServerContext> options)
            : base(options)
        {

        }

        //public DbSet<Person> Person { get; set; } = default!;
    }
}
