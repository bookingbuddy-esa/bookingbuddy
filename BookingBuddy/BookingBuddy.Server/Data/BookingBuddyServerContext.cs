using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Models;
using System;

namespace BookingBuddy.Server.Data
{
    public class BookingBuddyServerContext(DbContextOptions<BookingBuddyServerContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Amenity> PropertyAmenity { get; set; } = default!;

        public DbSet<Landlord> Landlord { get; set; } = default!;

        public DbSet<Property> Property { get; set; } = default!;
    }
}
