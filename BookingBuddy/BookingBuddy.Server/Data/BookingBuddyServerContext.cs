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
        public DbSet<AspNetProvider> AspNetProviders { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito à comodidade da propriedade física.
        /// </summary>
        public DbSet<Amenity> PropertyAmenity { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito à propriedade física.
        /// </summary>
        public DbSet<Property> Property { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às datas bloqueadas de uma propriedade.
        /// </summary>
        public DbSet<BlockedDate> BlockedDate { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito aos descontos de uma propriedade.
        /// </summary>
        public DbSet<Discount> Discount { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito a lista de favoritos de um utilizador.
        /// </summary>
        public DbSet<Favorite> Favorites { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito aos grupos de viagem.
        /// </summary>
        public DbSet<Group> Groups { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às orders.
        /// </summary>
        public DbSet<Order> Order { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às orders de promoção.
        /// </summary>
        public DbSet<PromotionOrder> PromotionOrder { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às orders de promover uma propriedade.
        /// </summary>
        public DbSet<PromoteOrder> PromoteOrder { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às reservas.
        /// </summary>
        public DbSet<BookingOrder> BookingOrder { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às reservas de grupo.
        /// </summary>
        public DbSet<GroupBookingOrder> GroupBookingOrder { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às mensagens de reserva.
        /// </summary>
        public DbSet<GroupOrderPayment> GroupOrderPayment { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às mensagens de reserva.
        /// </summary>
        public DbSet<BookingMessage> BookingMessage { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito aos pagamentos.
        /// </summary>
        public DbSet<Payment> Payment { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito às comodidades.
        /// </summary>
        public DbSet<Amenity> Amenity { get; set; } = default!;

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
        /// Propriedade que diz respeito às propriedades adicionadas pelo utilizador.
        /// </summary>
        public DbSet<UserAddedProperty> UserAddedProperty { get; set; } = default!;

        /// <summary>
        /// Propriedade que diz respeito aos votos dos utilizadores.
        /// </summary>
        public DbSet<UserVote> UserVote { get; set; } = default!;

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
                Description = "Esta é descrição da conta de administrador do BookingBuddy."
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
                Description = "Esta é descrição da conta de utilizador (padrão) do BookingBuddy."
            };
            userUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userUser, "userBB123!");

            var user2User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 2",
                UserName = "bookingbuddy.user2@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER2@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user2@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER2@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 2 (padrão) do BookingBuddy."
            };
            user2User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user2User, "user2BB123!");

            var user3User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 3",
                UserName = "bookingbuddy.user3@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER3@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user3@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER3@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 3 (padrão) do BookingBuddy."
            };
            user3User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user3User, "user3BB123!");

            var user4User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 4",
                UserName = "bookingbuddy.user4@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER4@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user4@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER4@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 4 (padrão) do BookingBuddy."
            };
            user4User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user4User, "user4BB123!");

            var user5User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 5",
                UserName = "bookingbuddy.user5@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER5@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user5@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER5@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 5 (padrão) do BookingBuddy."
            };
            user5User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user5User, "user5BB123!");


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
                Description = "Esta é a descrição da conta de proprietário do BookingBuddy."
            };
            landlordUser.PasswordHash =
                new PasswordHasher<ApplicationUser>().HashPassword(landlordUser, "landlordBB123!");

            var jMeterUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "JMeter",
                UserName = "bookingbuddy.jmeter@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.jmeter@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é a descrição da conta de teste do JMeter."
            };
            jMeterUser.PasswordHash =
                new PasswordHasher<ApplicationUser>().HashPassword(jMeterUser, "jmeterBB123!");

            builder.Entity<ApplicationUser>().HasData(
                adminUser,
                userUser,
                user2User,
                user3User,
                user4User,
                user5User,
                landlordUser,
                jMeterUser
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
                    RoleId = userRole.Id,
                    UserId = user2User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRole.Id,
                    UserId = user3User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRole.Id,
                    UserId = user4User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRole.Id,
                    UserId = user5User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = landlordRole.Id,
                    UserId = landlordUser.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = adminRole.Id,
                    UserId = jMeterUser.Id
                }
            );

            List<Amenity> amenities = [];
            amenities.AddRange(Enum.GetValues<AmenityEnum>().Select(amenity => new Amenity
            {
                AmenityId = Guid.NewGuid().ToString(), Name = amenity.ToString(), DisplayName = amenity.GetDescription()
            }));

            builder.Entity<Amenity>().HasData(amenities);
        }

        /// <summary>
        /// Método que limpa a base de dados.
        /// </summary>
        public async Task ResetDatabase()
        {
            await DeleteData();
            await DefaultData();
        }

        /// <summary>
        /// Método que elimina os dados da base de dados.
        /// </summary>
        private async Task DeleteData()
        {
            AspNetProviders.Clear();
            PropertyAmenity.Clear();
            Property.Clear();
            BlockedDate.Clear();
            Discount.Clear();
            Favorites.Clear();
            Groups.Clear();
            Order.Clear();
            PromotionOrder.Clear();
            PromoteOrder.Clear();
            BookingOrder.Clear();
            GroupBookingOrder.Clear();
            GroupOrderPayment.Clear();
            BookingMessage.Clear();
            Payment.Clear();
            Amenity.Clear();
            Rating.Clear();
            Chat.Clear();
            ChatMessage.Clear();
            UserAddedProperty.Clear();
            UserVote.Clear();
            Users.Clear();
            Roles.Clear();
            UserRoles.Clear();
            await SaveChangesAsync();
        }

        /// <summary>
        /// Método que insere os dados por defeito.
        /// </summary>
        private async Task DefaultData()
        {
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

            Roles.AddRange(adminRole, userRole, landlordRole);

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

            AspNetProviders.AddRange(googleProvider, microsoftProvider, localProvider);

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
                Description = "Esta é descrição da conta de administrador do BookingBuddy."
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
                Description = "Esta é descrição da conta de utilizador (padrão) do BookingBuddy."
            };
            userUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userUser, "userBB123!");

            var user2User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 2",
                UserName = "bookingbuddy.user2@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER2@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user2@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER2@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 2 (padrão) do BookingBuddy."
            };
            user2User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user2User, "user2BB123!");

            var user3User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 3",
                UserName = "bookingbuddy.user3@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER3@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user3@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER3@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 3 (padrão) do BookingBuddy."
            };
            user3User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user3User, "user3BB123!");

            var user4User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 4",
                UserName = "bookingbuddy.user4@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER4@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user4@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER4@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 4 (padrão) do BookingBuddy."
            };
            user4User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user4User, "user4BB123!");

            var user5User = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user 5",
                UserName = "bookingbuddy.user5@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.USER5@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.user5@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.USER5@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é descrição da conta de utilizador 5 (padrão) do BookingBuddy."
            };
            user5User.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user5User, "user5BB123!");


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
                Description = "Esta é a descrição da conta de proprietário do BookingBuddy."
            };
            landlordUser.PasswordHash =
                new PasswordHasher<ApplicationUser>().HashPassword(landlordUser, "landlordBB123!");

            var jMeterUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "JMeter",
                UserName = "bookingbuddy.jmeter@bookingbuddy.com",
                NormalizedUserName = "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM",
                Email = "bookingbuddy.jmeter@bookingbuddy.com",
                NormalizedEmail = "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                ProviderId = localProvider.AspNetProviderId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Description = "Esta é a descrição da conta de teste do JMeter."
            };
            jMeterUser.PasswordHash =
                new PasswordHasher<ApplicationUser>().HashPassword(jMeterUser, "jmeterBB123!");

            Users.AddRange(adminUser, userUser, user2User, user3User, user4User, user5User, landlordUser, jMeterUser);

            UserRoles.AddRange(
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
                    RoleId = userRole.Id,
                    UserId = user2User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRole.Id,
                    UserId = user3User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRole.Id,
                    UserId = user4User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRole.Id,
                    UserId = user5User.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = landlordRole.Id,
                    UserId = landlordUser.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = adminRole.Id,
                    UserId = jMeterUser.Id
                }
            );

            List<Amenity> amenities = [];
            amenities.AddRange(Enum.GetValues<AmenityEnum>().Select(amenity => new Amenity
            {
                AmenityId = Guid.NewGuid().ToString(), Name = amenity.ToString(), DisplayName = amenity.GetDescription()
            }));

            Amenity.AddRange(amenities);

            var property1 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Sea by the Rocks Sesimbra",
                Description =
                    "Vista de tirar o fôlego em Sesimbra! Com vista para o penhasco e para a linda praia da Califórnia, o edifício tem acesso privado à praia e está a uma caminhada de 5 a 10 minutos da área de restaurantes / mercado / lojas no centro da cidade.",
                Location = "R. Paula de Jesus, Sesimbra, Portugal",
                PricePerNight = 245,
                RoomsNumber = 3,
                MaxGuestsNumber = 6,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/miso/Hosting-878862770701762372/original/9a2d5701-45ea-4f50-ba96-1b7b5d2e2c76.jpeg?im_w=720"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Wifi,
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.PiscinaIndividual,
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property2 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Casa dos Cogumelos, Retiro Exclusivo à Beira Mar",
                Description =
                    "A Casa dos Cogumelos é uma propriedade exclusiva situada na Murtinheira, perto da Praia de Quiaios e da Figueira da Foz em Portugal. Dispõe de duas casas independentes inseridas numa paisagem natural única e com acesso direto e privado à praia. Entre a Serra da Boa Viagem e o mar, oferece o retiro perfeito para férias e relaxamento com família e amigos.",
                Location = "Casa dos Cogumelos, R. Farol Novo 68, Quiaios",
                PricePerNight = 590,
                RoomsNumber = 3,
                MaxGuestsNumber = 6,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/miso/Hosting-1050570021780851760/original/59e70c33-041d-4129-8c42-a9ed0a1539d2.jpeg?im_w=1440"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property3 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Praia Cabana da Costa Caparica",
                Description =
                    "Esta cabana está localizada na praia da Praia da Saude, uma das praias mais amadas da famosa Costa da Caparica de Lisboa, um lindo trecho de praia de areia branca repleta de restaurantes de frutos do mar, escolas de surf e casas de campo de cor doce. Na praia, a cabana é feita para desfrutar de um momento único. Sol. Surfe. Serenidade. ",
                Location = "Praia da Mata, 2825-490 Costa da Caparica",
                PricePerNight = 450,
                RoomsNumber = 2,
                MaxGuestsNumber = 4,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/4c0743d8-df89-4fd6-9c4b-4238a365368c.jpg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property4 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Vistas Panorâmicas I - Terraço, Vistas para o Mar e Piscina",
                Description =
                    "Apartamento recém-construído (2021) num condomínio privado com piscina.\nA piscina tem espreguiçadeiras e guarda-sóis para todos os hóspedes.\n2 quartos, 2 casas de banho e uma grande cozinha de espaço aberto/ sala de estar paisagem onde você não pode escapar da vista para o mar e luz mágica que ilumina a sala de estar. Materiais e móveis de alto padrão com uma bela decoração. AC em todos os quartos com aquecimento / refrigeração.",
                Location = "R. dos Mares da Gronelândia, 2450-503 Nazaré",
                PricePerNight = 123,
                RoomsNumber = 2,
                MaxGuestsNumber = 4,
                ImagesUrl = ["https://offloadmedia.feverup.com/lisboasecreta.co/wp-content/uploads/2019/08/19145953/louis-droege-8Nd3GY8z-iU-unsplash-1024x682.jpg"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.PiscinaIndividual
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property5 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "A View to the Raging Ocean -WCDS- Azenhas do Mar",
                Description =
                    "O Azenhas do Mar West Coast Design and Surf Villas (WCDS n1) permite que o visitante faça parte do cenário único do local, localizado na área central com fácil acesso e vista para o mar.",
                Location = "Estr. do Rodízio 51, 2705-340 Colares",
                PricePerNight = 210,
                RoomsNumber = 1,
                MaxGuestsNumber = 2,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/0fd04c5f-f7f2-49ef-b2c3-0823d37304b7.jpg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.PiscinaIndividual
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property6 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Microcasa Garden.Piscina,VistaMar(Villa Epicurea)",
                Description =
                    "Inspirado na filosofia do jardim de Epicuro, nosso refúgio na Serra d’Árrabida foi desenvolvido a pensar em seu bem-estar. Assim como o jardim do filósofo grego Epicuro, nosso eco-hotel celebra o prazer cotidiano como o princípio de um estilo de vida feliz e saudável. ",
                Location = "R. 4 de Maio 24-16",
                PricePerNight = 295,
                RoomsNumber = 4,
                MaxGuestsNumber = 5,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/9126de1d-cbc8-4fd3-8575-8ad2efb9d686.jpg?im_w=720"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.PiscinaIndividual,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property7 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Villa Laranjeiras com piscina aquecida, Comporta",
                Description =
                    "A espaçosa moradia de 220 m \u00b2 (construída em 2019) está localizada em Brejos da Carregueira de Cima, uma pequena aldeia tranquila a cerca de 5 minutos de carro das populares praias de Carvalhal e Pego.",
                Location = "Rua das Oliveiras 20-38, Comporta, Portugal",
                PricePerNight = 640,
                RoomsNumber = 4,
                MaxGuestsNumber = 10,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/miso/Hosting-37227186/original/3d8a4ff1-d586-417a-92ba-e6fd39c4c0be.jpeg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.PiscinaIndividual,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property8 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Story Flat Lisbon - Alfama",
                Description =
                    "Story Flat Lisbon é um convite para descobrir a sua própria história de Lisboa. Este apartamento luminoso e encantador oferece uma verdadeira experiência sensorial da cidade, uma vez que todos os quartos oferecem uma vista panorâmica deslumbrante do rio Tejo.",
                Location = "Largo do Contador Mor 21, 1100-387 Lisboa",
                PricePerNight = 129,
                RoomsNumber = 1,
                MaxGuestsNumber = 3,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/4257d9fa-00ff-4bfc-a759-30d8a653d8c4.jpg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property9 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Libest Av. da Liberdade 1 - Restauradores SUBLIME",
                Description =
                    "Lindo apartamento, finamente decorado, localizado na Praça dos Restauradores, coração de Lisboa. No lugar onde a Avenida da Liberdade se encontra com o Rossio, a poucos passos de lojas, cafés, transporte e tudo mais que um viajante pode querer para melhor desfrutar dessa linda cidade.",
                Location = "Av. da Liberdade 1, 1250-144 Lisboa",
                PricePerNight = 240,
                RoomsNumber = 2,
                MaxGuestsNumber = 4,
                ImagesUrl = ["https://lisbonlanguagecafe.pt/wp-content/uploads/2022/02/marcas-de-luxo-2.jpg"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property10 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Woodhouse Cityard, lodge de madeira perto do Estoril-Cascais",
                Description =
                    "Uma casa de madeira construída especialmente para os nossos hóspedes, humilde e confortável num ambiente amigável e natural. \nÉ um único alojamento, localizado numa propriedade privada, onde a privacidade e é bastante garantida.",
                Location = "R. dos Pescadores 1, 2825-486 Costa da Caparica",
                PricePerNight = 84,
                RoomsNumber = 2,
                MaxGuestsNumber = 4,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/c6bb86ef-be34-4098-9a23-6dbb7ba86e4c.jpg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property11 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Varanda sobre o Rio Tejo com Vistas - Rede Ultra Rápida",
                Description =
                    "Casa da Praia is a beautiful beach house located in the heart of the Costa da Caparica village, just 100 meters from the beach. The house has a large terrace with a barbecue and a dining area, perfect for enjoying the sunny days of the Portuguese coast.",
                Location = "R. Cidade de Goa 24, 2685-038 Sacavém",
                PricePerNight = 299,
                RoomsNumber = 1,
                MaxGuestsNumber = 2,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/d7c0767f-807a-4b02-820c-186b3f86539f.jpg?im_w=720"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property12 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Upper Villa",
                Description =
                    "Upper Villa é uma vila de luxo incrível em Sintra com a mais bela vista, árvores verdes até onde os olhos podem ver e nada mais, exceto o Castelo dos Mouros no topo de uma crista, um dos monumentos mais emblemáticos de Sintra.",
                Location = "R. Mal. Saldanha 13 , 15 e 17, 2710-587 Sintra",
                PricePerNight = 1040,
                RoomsNumber = 5,
                MaxGuestsNumber = 12,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/miso/Hosting-660603348227741841/original/4708df16-28f1-493f-bc0d-e54787cdbc09.jpeg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.PiscinaIndividual,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property13 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Lux Mare I",
                Description =
                    "Esta deslumbrante moradia de 6 quartos ocupa uma posição invejável à beira-mar, a uma curta distância a pé de belas praias de areia.",
                Location = "R. do Canavial, 8600-513 Lagos",
                PricePerNight = 1875,
                RoomsNumber = 6,
                MaxGuestsNumber = 12,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/7ca6118f-68c7-4a32-8bbc-09ce1840a373.jpg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.PiscinaIndividual,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property14 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Central address meets style",
                Description =
                    "Recentemente renovado e com localização central, este apartamento irá colocá-lo a uma curta distância a pé de restaurantes, bares, ferry para a ilha, supermercado e cidade velha, mantendo-o longe o suficiente da agitação normal do verão.",
                Location = "Largo das Termas de Santo António, Tavira",
                PricePerNight = 139,
                RoomsNumber = 2,
                MaxGuestsNumber = 4,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/miso/Hosting-777452185596929196/original/22fa3830-610e-421f-ba2b-beefa56376e9.jpeg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                            AmenityEnum.Wifi
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            var property15 = new Property
            {
                PropertyId = Guid.NewGuid().ToString(),
                ApplicationUserId = landlordUser.Id,
                Name = "Monte da Várzea da Rainha II",
                Description =
                    "O Monte da Várzea da Rainha está inserido numa quinta em Óbidos, mais propriamente nas terras da Várzea da Rainha, propriedade da familia com séculos de tradição agrícola. \nRecuperada a partir de uma antiga ruína, compreende 2 bungalows, com capacidade para quatro pessoas cada e uma casa comum, com sala de estar e de refeições.",
                Location = "Estr. da Galiota, Óbidos, Portugal",
                PricePerNight = 80,
                RoomsNumber = 1,
                MaxGuestsNumber = 2,
                ImagesUrl = ["https://a0.muscache.com/im/pictures/miso/Hosting-908573299832149690/original/da6bafd7-7a7d-4a40-a7c9-a829dde98279.jpeg?im_w=1200"],
                AmenityIds = amenities
                    .Where(a =>
                    {
                        var selectedAmens = new[]
                        {
                            AmenityEnum.Microondas,
                            AmenityEnum.Cozinha,
                            AmenityEnum.Quintal,
                            AmenityEnum.Varanda,
                            AmenityEnum.Tv,
                        };
                        return selectedAmens.Select(sa => sa.ToString()).Contains(a.Name);
                    })
                    .Select(ai => ai.AmenityId)
                    .ToList()
            };
            
            Property.AddRange(property1, property2, property3, property4, property5, property6, property7, property8, property9, property10, property11, property12, property13, property14, property15);
            
            await SaveChangesAsync();
        }
    }
}

/// <summary>
/// Extensões para entidades.
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// Método que limpa uma entidade.
    /// </summary>
    /// <param name="dbSet">Entidade a ser limpa.</param>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    public static void Clear<T>(this DbSet<T> dbSet) where T : class
    {
        dbSet.RemoveRange(dbSet);
    }
}