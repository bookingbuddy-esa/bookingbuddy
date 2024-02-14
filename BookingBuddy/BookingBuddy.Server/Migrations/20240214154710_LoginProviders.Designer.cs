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
    [Migration("20240214154710_LoginProviders")]
    partial class LoginProviders
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
                            Id = "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "5767ba9d-167e-42a8-b3ec-8501589ed7f0",
                            Email = "bookingbuddy.admin@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "admin",
                            NormalizedEmail = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEFAlB/VVqc5VZXAvHDdyb/lp/GJvdZhfVJWhj2P43JTCsqrpMM1Y5CHyxof1NHFKxA==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "599b5787-f070-4d1d-8c91-17631c17bc7d",
                            SecurityStamp = "693c4b38-bfb8-4e70-a718-d0962282add6",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.admin@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "b7111238-968e-4b2f-8a49-9cc4969a5624",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "b7b2f876-b387-42b2-bf25-32b0d39634b2",
                            Email = "bookingbuddy.user@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "user",
                            NormalizedEmail = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEN0bgrJ6qQsPY2pvqenJSsnqw1NAB8CpMTldTIciL3N3oyrIo8YZKIlm2vUm47vXiw==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "599b5787-f070-4d1d-8c91-17631c17bc7d",
                            SecurityStamp = "0b65d11a-d2bf-4358-b950-095ad96f2fd4",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.user@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "5266930d-952d-46bd-9fe3-495d7d66660e",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a619f5ae-aaef-4211-be88-6ee1ee17ce4f",
                            Email = "bookingbuddy.landlord@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "landlord",
                            NormalizedEmail = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAECx/ejlklKQ3t9LDtEjyOjoJw+Qzk2q4n0KgKRFcK26Ime4e1O050ahLlbP0I/rmjw==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "599b5787-f070-4d1d-8c91-17631c17bc7d",
                            SecurityStamp = "e098a394-329d-476f-9f46-323f1b63d691",
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
                            AspNetProviderId = "e2f08010-89a9-4225-8ddf-ab5f203e3494",
                            Name = "google",
                            NormalizedName = "GOOGLE"
                        },
                        new
                        {
                            AspNetProviderId = "9d19f1a6-0a1a-478a-bcf9-02234f688367",
                            Name = "microsoft",
                            NormalizedName = "MICROSOFT"
                        },
                        new
                        {
                            AspNetProviderId = "599b5787-f070-4d1d-8c91-17631c17bc7d",
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

            modelBuilder.Entity("BookingBuddy.Server.Models.Property", b =>
                {
                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AmenityIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                            Id = "e2eb363a-7bb4-4847-aff1-abbdd152fdfa",
                            ConcurrencyStamp = "5e8a3126-472b-4d27-9ee9-7aba0ff59e2a",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "af6f0000-a53d-4287-8d77-2b81c1a65d0d",
                            ConcurrencyStamp = "482674ba-d832-414d-b2ad-68571152157d",
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "ac30bd48-cc51-4947-ad2e-9785aeabb38c",
                            ConcurrencyStamp = "d78d64f6-ff5e-4bf9-bca7-bd8c402d16e3",
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
                            UserId = "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9",
                            RoleId = "e2eb363a-7bb4-4847-aff1-abbdd152fdfa"
                        },
                        new
                        {
                            UserId = "b7111238-968e-4b2f-8a49-9cc4969a5624",
                            RoleId = "af6f0000-a53d-4287-8d77-2b81c1a65d0d"
                        },
                        new
                        {
                            UserId = "5266930d-952d-46bd-9fe3-495d7d66660e",
                            RoleId = "ac30bd48-cc51-4947-ad2e-9785aeabb38c"
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
