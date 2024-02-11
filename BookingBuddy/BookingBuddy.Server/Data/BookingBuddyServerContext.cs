using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Models;

namespace BookingBuddy.Server.Data
{
    /// <summary>
    /// Classe que representa o contexto da base de dados da aplicação.
    /// </summary>
    /// <param name="options">As opções para a criação do contexto</param>
    public class BookingBuddyServerContext(DbContextOptions<BookingBuddyServerContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        /// <summary>
        /// Propriedade que diz respeito à comodidade da propriedade física.
        /// </summary>
        public DbSet<Amenity> PropertyAmenity { get; set; } = default!;

        /*
        /// <summary>
        /// Propriedade que diz respeito ao proprietário da propriedade física.
        /// </summary>
        public DbSet<Landlord> Landlord { get; set; } = default!;
        */

        /// <summary>
        /// Propriedade que diz respeito à propriedade física.
        /// </summary>
        public DbSet<Property> Property { get; set; } = default!;
    }
}
