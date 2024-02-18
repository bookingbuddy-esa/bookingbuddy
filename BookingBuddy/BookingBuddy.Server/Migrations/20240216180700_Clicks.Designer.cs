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
    [Migration("20240216180700_Clicks")]
    partial class Clicks
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
                    b.Property<int>("AmenityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AmenityId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AmenityId");

                    b.HasIndex("PropertyId");

                    b.ToTable("PropertyAmenity");
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
                            Id = "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "5cd1d3aa-e538-44c5-8af6-e59520e2a973",
                            Email = "bookingbuddy.admin@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "admin",
                            NormalizedEmail = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEJtcHc1qCjtJnehNWdJ2Gk5wV41Dd+AKDkkOvzVMUOIJ/haHphiSaGKBKgl0mib7+A==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "4b915b1f-8b74-45cf-979e-b10f8b98f919",
                            SecurityStamp = "676321b8-1748-4e2d-9b6e-602849ecf26c",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.admin@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "48996947-a573-4470-8d32-7797a2bac0e5",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "1865f161-f61a-45cd-a8f5-f7c58e2a040a",
                            Email = "bookingbuddy.user@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "user",
                            NormalizedEmail = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEEzFRjlpejUeQ2FduLqTCg+K1pAhDOKOgbm9hiZ2mwlYaTN718YVLhUXnZ96HXOvmQ==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "4b915b1f-8b74-45cf-979e-b10f8b98f919",
                            SecurityStamp = "286c81f7-66b2-460e-97b3-c0876b8b5b16",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.user@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "2de95519-31eb-459e-8ad1-5021989213f5",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "01c1e30e-888f-4f11-91e9-875ba385cbe5",
                            Email = "bookingbuddy.landlord@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "landlord",
                            NormalizedEmail = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEJil2s4PH5/oiIIzET0MZif5nEwFry6KmL4TzvDxC04ljdbVtJ4oyvW3IKxjeeT73Q==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "4b915b1f-8b74-45cf-979e-b10f8b98f919",
                            SecurityStamp = "f372039e-cc5c-4961-95d5-daba427cf305",
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
                            AspNetProviderId = "4b8bcf90-dfa6-4394-9d53-9686cd0e9ed9",
                            Name = "google",
                            NormalizedName = "GOOGLE"
                        },
                        new
                        {
                            AspNetProviderId = "1ea9abb9-f0ee-456f-9d80-5c75d95ca7a1",
                            Name = "microsoft",
                            NormalizedName = "MICROSOFT"
                        },
                        new
                        {
                            AspNetProviderId = "4b915b1f-8b74-45cf-979e-b10f8b98f919",
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

                    b.Property<string>("Method")
                        .IsRequired()
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
                            Id = "42630d5b-ba89-4955-a36a-e31dd8cf7705",
                            ConcurrencyStamp = "5e69ee78-9632-42a3-9e56-15bb46da26e7",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "61656fea-ed23-460c-bed2-acae223beeef",
                            ConcurrencyStamp = "3f93db4b-e044-4640-96a3-40093425fab0",
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "acb67e3b-29f7-409a-af25-8dac9168fe56",
                            ConcurrencyStamp = "4f564740-89b2-4e57-93f5-579aab7b661f",
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
                            UserId = "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362",
                            RoleId = "42630d5b-ba89-4955-a36a-e31dd8cf7705"
                        },
                        new
                        {
                            UserId = "48996947-a573-4470-8d32-7797a2bac0e5",
                            RoleId = "61656fea-ed23-460c-bed2-acae223beeef"
                        },
                        new
                        {
                            UserId = "2de95519-31eb-459e-8ad1-5021989213f5",
                            RoleId = "acb67e3b-29f7-409a-af25-8dac9168fe56"
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
