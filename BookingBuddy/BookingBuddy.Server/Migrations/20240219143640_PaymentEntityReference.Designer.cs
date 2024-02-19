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
    [Migration("20240219143640_PaymentEntityReference")]
    partial class PaymentEntityReference
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
                            Id = "b057c7e3-cdee-413a-8519-679d208487c6",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c5d9313e-797a-4e3e-98a2-215453844ba5",
                            Email = "bookingbuddy.admin@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "admin",
                            NormalizedEmail = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEE6F0+amt1r4upK81a+qylNK2z2mx+cMiL/IWVKI1IrSR83eskUGCT43mUquqdbsdQ==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "b81bc9f6-9cdc-4d60-be3b-8367414d1811",
                            SecurityStamp = "5aee445b-771a-40c9-b802-b1fe6e72fd0b",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.admin@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "fd492321-0413-4b81-ba6a-6cb3082ccfb4",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "7e39e048-d66f-4015-827e-24891cf03c5d",
                            Email = "bookingbuddy.user@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "user",
                            NormalizedEmail = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAENWNhRQET30ZdlqH8SbLaxfLEjGyzN7QhkSLzsCY1cTwE+zyXQ9HY55kwFAoxU+N9g==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "b81bc9f6-9cdc-4d60-be3b-8367414d1811",
                            SecurityStamp = "c1474d89-6528-451c-a499-57c65b4cd0ed",
                            TwoFactorEnabled = false,
                            UserName = "bookingbuddy.user@bookingbuddy.com"
                        },
                        new
                        {
                            Id = "fc70eb1f-a3a0-45eb-8c90-dcbc1f89b617",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "74782839-169b-493c-9bd2-dc9807480f60",
                            Email = "bookingbuddy.landlord@bookingbuddy.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "landlord",
                            NormalizedEmail = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            NormalizedUserName = "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEMrzUxss7v/Jy+Y7mO5Xmkh9JQzsYofknRf+XREYuWGd2ky/s/jdCkB51t38kw2Emg==",
                            PhoneNumberConfirmed = false,
                            ProviderId = "b81bc9f6-9cdc-4d60-be3b-8367414d1811",
                            SecurityStamp = "c7f211f8-6bef-4f83-a8f5-7d0f3a677623",
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
                            AspNetProviderId = "33111571-4c78-4e33-a807-8a210cb573b5",
                            Name = "google",
                            NormalizedName = "GOOGLE"
                        },
                        new
                        {
                            AspNetProviderId = "66bb57d5-e9f2-4403-b227-4ede7fc79bb5",
                            Name = "microsoft",
                            NormalizedName = "MICROSOFT"
                        },
                        new
                        {
                            AspNetProviderId = "b81bc9f6-9cdc-4d60-be3b-8367414d1811",
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
                            Id = "497d10df-3747-46f1-8dcf-b66f20f5da3d",
                            ConcurrencyStamp = "7ebfa63a-4807-4544-a598-96dd57a18307",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "5a1b2ba7-66fd-4d0e-bc71-1f2824f901a3",
                            ConcurrencyStamp = "c173d319-cd61-49a6-bf41-bcefe659f0d2",
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "5ddead2a-6e21-4c18-99d2-d755495ef627",
                            ConcurrencyStamp = "8f36dfb1-835d-443b-b6d8-26362684d25a",
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
                            UserId = "b057c7e3-cdee-413a-8519-679d208487c6",
                            RoleId = "497d10df-3747-46f1-8dcf-b66f20f5da3d"
                        },
                        new
                        {
                            UserId = "fd492321-0413-4b81-ba6a-6cb3082ccfb4",
                            RoleId = "5a1b2ba7-66fd-4d0e-bc71-1f2824f901a3"
                        },
                        new
                        {
                            UserId = "fc70eb1f-a3a0-45eb-8c90-dcbc1f89b617",
                            RoleId = "5ddead2a-6e21-4c18-99d2-d755495ef627"
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
