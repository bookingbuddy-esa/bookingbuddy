using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace BookingBuddy.Server.Data
{
    /// <summary>
    /// Classe que representa o contexto da base de dados da aplicação.
    /// </summary>
    /// <param name="options">As opções para a criação do contexto</param>
    public class BookingBuddyServerContext(DbContextOptions<BookingBuddyServerContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        /// <summary>
        /// Propriedade que diz respeito ao fornecedor de login.
        /// </summary>
        public DbSet<AspNetProvider> AspNetProviders { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito à comodidade da propriedade física.
        /// </summary>
        public DbSet<Amenity> PropertyAmenity { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito à propriedade física.
        /// </summary>
        public DbSet<Property> Property { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às datas bloqueadas de uma propriedade.
        /// </summary>
        public DbSet<BlockedDate> BlockedDate { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito aos descontos de uma propriedade.
        /// </summary>
        public DbSet<Discount> Discount { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito a lista de favoritos de um utilizador.
        /// </summary>
        public DbSet<Favorite> Favorites { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito aos grupos de viagem.
        /// </summary>
        public DbSet<Group> Groups { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às orders.
        /// </summary>
        public DbSet<Order> Order { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às orders de promoção.
        /// </summary>
        public DbSet<PromotionOrder> PromotionOrder { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às orders de promover uma propriedade.
        /// </summary>
        public DbSet<PromoteOrder> PromoteOrder { get; init; } = default!;

        /// <summary>
        ///  Propriedade que diz respeito às reservas.
        /// </summary>
        public DbSet<BookingOrder> BookingOrder { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às reservas de grupo.
        /// </summary>
        public DbSet<GroupBookingOrder> GroupBookingOrder { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às mensagens de reserva.
        /// </summary>
        public DbSet<BookingMessage> BookingMessage { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito aos pagamentos.
        /// </summary>
        public DbSet<Payment> Payment { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às comodidades.
        /// </summary>
        public DbSet<Amenity> Amenity { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às avaliações.
        /// </summary>
        public DbSet<Rating> Rating { get; init; } = default!;

        /// <summary>
        /// Propriedade que diz respeito ao chat.
        /// </summary>
        public DbSet<Chat> Chat { get; set; } = default!;

        /// <summary>
        ///  Propriedade que diz respeito às mensagens do chat.
        /// </summary>
        public DbSet<ChatMessage> ChatMessage { get; set; } = default!;

        /// <summary>
        ///  Propriedade que diz respeito às mensagens de grupo.
        /// </summary>
        public DbSet<GroupMessage> GroupMessage { get; init; } = default!;

        /// <summary>
        /// Dados de inicialização da base de dados.
        /// </summary>
        /// <param name="builder">Construtor do modelo</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRole = new IdentityRole
            {
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var userRole = new IdentityRole
            {
                Name = "user",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var landlordRole = new IdentityRole
            {
                Name = "landlord",
                NormalizedName = "LANDLORD",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            builder.Entity<IdentityRole>().HasData(
                adminRole,
                userRole,
                landlordRole
            );

            var googleProvider = new AspNetProvider
            {
                AspNetProviderId = Guid.NewGuid().ToString(),
                Name = "google",
                NormalizedName = "GOOGLE"
            };

            var microsoftProvider = new AspNetProvider
            {
                AspNetProviderId = Guid.NewGuid().ToString(),
                Name = "microsoft",
                NormalizedName = "MICROSOFT"
            };

            var localProvider = new AspNetProvider
            {
                AspNetProviderId = Guid.NewGuid().ToString(),
                Name = "local",
                NormalizedName = "LOCAL"
            };

            builder.Entity<AspNetProvider>().HasData(
                googleProvider,
                microsoftProvider,
                localProvider
            );

            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                UserName = "bookingbuddy.admin@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.admin@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "adminBB123!");

            var userUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user",
                UserName = "bookingbuddy.user@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            userUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userUser, "userBB123!");

            var landlordUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "landlord",
                UserName = "bookingbuddy.landlord@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.landlord@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            landlordUser.PasswordHash =
                new PasswordHasher<ApplicationUser>().HashPassword(landlordUser, "landlordBB123!");

            builder.Entity<ApplicationUser>().HasData(
                adminUser,
                userUser,
                landlordUser
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRole.Id,
                    UserId = adminUser.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRole.Id,
                    UserId = userUser.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = landlordRole.Id,
                    UserId = landlordUser.Id
                }
            );

            List<Amenity> amenities = [];
            amenities.AddRange(Enum.GetValues<AmenityEnum>().Select(amenity => new Amenity
            {
                AmenityId = Guid.NewGuid().ToString(), Name = amenity.ToString(), DisplayName = amenity.GetAmenityName()
            }));

            builder.Entity<Amenity>().HasData(amenities);
        }
    }
}