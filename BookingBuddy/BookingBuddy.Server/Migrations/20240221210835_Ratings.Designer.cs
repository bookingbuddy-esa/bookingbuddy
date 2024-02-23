﻿// <auto-generated />
using System;
using BookingBuddy.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookingBuddy.Server.Migrations
{
    [DbContext(typeof(BookingBuddyServerContext))]
    [Migration("20240221210835_Ratings")]
    partial class Ratings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookingBuddy.Server.Models.Amenity", b =>
                {
                    b.Property<string>("AmenityId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AmenityId");

                    b.HasIndex("PropertyId");

                    b.ToTable("Amenity");

                    b.HasData(
                        new
                        {
                            AmenityId = "a04dd010-bb74-49de-bc5e-66269a31ec4c",
                            DisplayName = "Estacionamento",
                            Name = "Estacionamento"
                        },
                        new
                        {
                            AmenityId = "885607aa-28a3-4c40-b0af-6548c32e3ad3",
                            DisplayName = "Wifi",
                            Name = "Wifi"
                        },
                        new
                        {
                            AmenityId = "145e667c-a7fd-488b-9243-4379f528b903",
                            DisplayName = "Cozinha",
                            Name = "Cozinha"
                        },
                        new
                        {
                            AmenityId = "ed6fd758-5c19-4b53-94bc-61851ef0c57f",
                            DisplayName = "Varanda",
                            Name = "Varanda"
                        },
                        new
                        {
                            AmenityId = "dd8fb3cc-583c-462b-bed5-4f89e04d5b3b",
                            DisplayName = "Frigorífico",
                            Name = "Frigorifico"
                        },
                        new
                        {
                            AmenityId = "2913e613-e087-463f-8bea-abd99000ed32",
                            DisplayName = "Microondas",
                            Name = "Microondas"
                        },
                        new
                        {
                            AmenityId = "d1264673-520b-44ea-b059-d1167884fe81",
                            DisplayName = "Quintal",
                            Name = "Quintal"
                        },
                        new
                        {
                            AmenityId = "08da675b-4c42-4c92-bc80-8cf530451531",
                            DisplayName = "Máquina de Lavar",
                            Name = "MaquinaLavar"
                        },
                        new
                        {
                            AmenityId = "8a9ce2c6-2a48-4291-b4e4-8d54b14a370d",
                            DisplayName = "Piscina Partilhada",
                            Name = "PiscinaPartilhada"
                        },
                        new
                        {
                            AmenityId = "ad5c3780-d476-4ee8-b69e-04cba14613e1",
                            DisplayName = "Piscina Individual",
                            Name = "PiscinaIndividual"
                        },
                        new
                        {
                            AmenityId = "3a57446f-067d-4308-9bed-589137abcbcd",
                            DisplayName = "Animais",
                            Name = "Animais"
                        },
                        new
                        {
                            AmenityId = "a8cc57a0-430c-466e-bee3-bb0d62385f4a",
                            DisplayName = "Câmaras",
                            Name = "Camaras"
                        },
                        new
                        {
                            AmenityId = "330c8afc-5843-4e2e-a482-e9476d3b5646",
                            DisplayName = "TV",
                            Name = "Tv"
                        });
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ProviderId");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "8f6ba4eb-12a6-4958-a554-da4df2de5bb5",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "99aabb2f-d474-41e6-a3c4-21f143efbd9f",
                            Email = "bookingbuddy.admin@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "admin",
                            NormalizedEmail = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEFxssz0++kaf4JaAAJgSwkvYJj4OnfGRIPhgjRi2ZFoUU//ApvNNKRU7IY2AxZ2KXg==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2",
                            SecurityStamp = "21c853d4-4571-422b-850b-42657b885c1c",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.admin@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "d2a440c0-732b-48da-b815-1105710ef153",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e39d7a5b-4e38-441e-bf98-4e6325cd3716",
                            Email = "bookingbuddy.user@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "user",
                            NormalizedEmail = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAED+1wfPNRc5l/UxYsQpWNND1SjDyddoekC/ztbZamYM21iaG09Wx9pLLR5OhzdEVlg==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2",
                            SecurityStamp = "b21e6875-8c29-42cf-bd53-28a2fb282395",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.user@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "129808f1-c20d-4fa1-a34a-689bc0b89f9a",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a2890ca3-40ba-431f-9d51-866245553700",
                            Email = "bookingbuddy.landlord@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "landlord",
                            NormalizedEmail = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEP60aljRUtT9KdULM3RcINYm9ibZJHWqUwpmlB1uiKTeGeORy68O4Ivy4pxb4M0YkQ==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2",
                            SecurityStamp = "27e72b63-7b68-4d93-82c0-28592ca74255",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.landlord@bookingbuddy.com"
                        });
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.AspNetProvider", b =>
                {
                    b.Property<string>("AspNetProviderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AspNetProviderId");

                    b.ToTable("AspNetProviders");

                    b.HasData(
                        new
                        {
                            AspNetProviderId = "4c393618-868d-404d-bd1d-7f0a4cf9f5c5",
                            Name = "google",
                            NormalizedName = "GOOGLE"
                        },
                        new
                        {
                            AspNetProviderId = "eddd2412-121a-45f4-bde4-488bb49097c3",
                            Name = "microsoft",
                            NormalizedName = "MICROSOFT"
                        },
                        new
                        {
                            AspNetProviderId = "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2",
                            Name = "local",
                            NormalizedName = "LOCAL"
                        });
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.BlockedDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("End")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Start")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("BlockedDate");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.BookingOrder", b =>
                {
                    b.Property<string>("BookingOrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("int");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BookingOrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("BookingOrder");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("State")
                        .HasColumnType("bit");

                    b.HasKey("OrderId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("PropertyId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Payment", b =>
                {
                    b.Property<string>("PaymentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Entity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.PromoteOrder", b =>
                {
                    b.Property<string>("PromoteOrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PromoteOrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("PromoteOrder");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.PromotionOrder", b =>
                {
                    b.Property<string>("PromotionOrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PromotionOrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("PromotionOrder");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Property", b =>
                {
                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AmenityIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Clicks")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImagesUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PricePerNight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PropertyId");

                    b.ToTable("Property");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Rating", b =>
                {
                    b.Property<string>("RatingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("RatingId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "7ab35194-5997-4185-90ed-c2edabb63b96",
                            ConcurrencyStamp = "97f698ab-5ba5-424d-9c31-b43b65793e40",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "c4111660-4886-42a0-9722-2830bfac1cc7",
                            ConcurrencyStamp = "b0a9d8f1-87af-4747-9264-215a2a1701bf",
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "5e34358a-343b-4db0-9e91-e5f399cacbd6",
                            ConcurrencyStamp = "ed1b0558-59df-4b99-8422-ede7f06ef68d",
                            Name = "landlord",
                            NormalizedName = "LANDLORD"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "8f6ba4eb-12a6-4958-a554-da4df2de5bb5",
                            RoleId = "7ab35194-5997-4185-90ed-c2edabb63b96"
                        },
                        new
                        {
                            UserId = "d2a440c0-732b-48da-b815-1105710ef153",
                            RoleId = "c4111660-4886-42a0-9722-2830bfac1cc7"
                        },
                        new
                        {
                            UserId = "129808f1-c20d-4fa1-a34a-689bc0b89f9a",
                            RoleId = "5e34358a-343b-4db0-9e91-e5f399cacbd6"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Amenity", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.Property", null)
                        .WithMany("Amenities")
                        .HasForeignKey("PropertyId");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.ApplicationUser", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.AspNetProvider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.BlockedDate", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.Property", null)
                        .WithMany("BlockedDates")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.BookingOrder", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Order", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBuddy.Server.Models.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBuddy.Server.Models.Property", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Payment");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.PromoteOrder", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.PromotionOrder", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Rating", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingBuddy.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BookingBuddy.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookingBuddy.Server.Models.Property", b =>
                {
                    b.Navigation("Amenities");

                    b.Navigation("BlockedDates");
                });
#pragma warning restore 612, 618
        }
    }
}
