using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetProviders",
                columns: table => new
                {
                    AspNetProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetProviders", x => x.AspNetProviderId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    ChatId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmenityIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImagesUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clicks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.PropertyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MembersId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedPropertyIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserVoteIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChosenProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChatId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GroupAction = table.Column<int>(type: "int", nullable: false),
                    GroupBookingId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "ChatId");
                });

            migrationBuilder.CreateTable(
                name: "Amenity",
                columns: table => new
                {
                    AmenityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenity", x => x.AmenityId);
                    table.ForeignKey(
                        name: "FK_Amenity_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId");
                });

            migrationBuilder.CreateTable(
                name: "BlockedDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    End = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockedDate_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    DiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountAmount = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_Discount_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupBookingOrderOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "AspNetProviders",
                        principalColumn: "AspNetProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingMessage",
                columns: table => new
                {
                    BookingMessageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingOrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingMessage", x => x.BookingMessageId);
                    table.ForeignKey(
                        name: "FK_BookingMessage_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingOrder",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingOrder", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_BookingOrder_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingOrder_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingOrder_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    MessageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_ChatMessage_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "ChatId");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupBookingOrder",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupPaymentIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidByIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupBookingOrder", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_GroupBookingOrder_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupBookingOrder_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupBookingOrder_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromoteOrder",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoteOrder", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_PromoteOrder_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromoteOrder_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromoteOrder_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionOrder",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionOrder", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_PromotionOrder_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionOrder_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionOrder_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    RatingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Rating_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddedProperty",
                columns: table => new
                {
                    UserAddedPropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddedProperty", x => x.UserAddedPropertyId);
                    table.ForeignKey(
                        name: "FK_UserAddedProperty_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAddedProperty_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_UserAddedProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVote",
                columns: table => new
                {
                    UserVoteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVote", x => x.UserVoteId);
                    table.ForeignKey(
                        name: "FK_UserVote_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVote_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_UserVote_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupOrderPayment",
                columns: table => new
                {
                    GroupPaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupBookingOrderOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOrderPayment", x => x.GroupPaymentId);
                    table.ForeignKey(
                        name: "FK_GroupOrderPayment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOrderPayment_GroupBookingOrder_GroupBookingOrderOrderId",
                        column: x => x.GroupBookingOrderOrderId,
                        principalTable: "GroupBookingOrder",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_GroupOrderPayment_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "0933cbcc-dfed-4965-aed7-9b333a28b420", "Frigorífico", "Frigorifico", null },
                    { "2c9ca8f2-fc7d-47d4-a60b-5009eb984333", "Microondas", "Microondas", null },
                    { "4a7c2f9d-0025-4b42-bb47-b15b1f60211f", "Estacionamento", "Estacionamento", null },
                    { "5d5aafad-20b4-4a1d-9b18-ead91ec94e22", "Animais", "Animais", null },
                    { "8b336182-7167-4128-9aed-7376e5a039ea", "Câmaras", "Camaras", null },
                    { "991cb213-eee7-4767-bd07-719460b3429a", "Máquina de Lavar", "MaquinaLavar", null },
                    { "b0b09e14-d1b7-4d00-8bf7-6a0dbbb0d7d7", "Cozinha", "Cozinha", null },
                    { "b43b6543-24dc-402d-8d2c-72e049e75deb", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "b665de37-1cbc-452f-aa76-f2ce7c6b5617", "Quintal", "Quintal", null },
                    { "bb467b90-9d22-49a0-b186-dd8b3cae99dc", "Varanda", "Varanda", null },
                    { "c655aea4-72e9-423b-8452-51fc6d654666", "Piscina Individual", "PiscinaIndividual", null },
                    { "ea52bdb9-af1f-4b67-a442-6cec9f51dcb3", "Wifi", "Wifi", null },
                    { "f30a7c7a-b036-4560-b693-46e97b44b76f", "TV", "Tv", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22f07bfd-7470-4a16-a408-941b04818fe3", "google", "GOOGLE" },
                    { "8c79eff0-2029-48b5-9f5b-6877f490f891", "microsoft", "MICROSOFT" },
                    { "d16883f5-62f0-4c75-9282-e0dd18ab2963", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3225bc27-4b0c-48b8-a33c-18fddd1e3649", "24ae1ced-a7a5-43a2-8739-0905dda82658", "user", "USER" },
                    { "4f0df77a-59f4-4716-acb8-cc282cc7526d", "2bb24d20-31cc-4b22-9c00-91a8c537098c", "landlord", "LANDLORD" },
                    { "e3c6c525-2475-4fe4-ac7d-1df5bb6f0ed1", "88aa73d0-b3a0-457a-a664-b6be9912cbc6", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6095890e-d648-4068-a573-bec722a25dc9", 0, "9b17e702-941b-4745-b837-8d0890339445", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEPhQUbEM6SdPKy5kDSXPS7j5mjFXLtTG6GSm487Dt1+uZcMFZZdjF2io3LioezDxCQ==", null, false, null, "d16883f5-62f0-4c75-9282-e0dd18ab2963", "28557ea8-a958-4402-bd34-2d8f197e4ff4", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "a39d08d0-bc15-4003-8144-aa2eef919a77", 0, "1b406dbb-08ff-4531-9110-56e25f598481", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEGO29cxe6+MzqEjNf0Yd12l6yNIYm6vbtD6HNyvOs25irKrDOqFXK0WtlmhuRS0vPw==", null, false, null, "d16883f5-62f0-4c75-9282-e0dd18ab2963", "f1a72605-ae19-450b-a232-d24f0c8b60d3", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "c4a5cf1a-e17a-4c4c-aefa-ca1c78abb703", 0, "adf02eca-0483-4fc0-93eb-fd3e91d72981", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEOxGsvy/3z+o+5InnSCpNuKO3wSJQYVdu+EFEZvYbjKXMTZwSkI8MGux6UW6rlyT7Q==", null, false, null, "d16883f5-62f0-4c75-9282-e0dd18ab2963", "4740fab9-2930-4cbd-b6f4-c4ce54570374", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "4f0df77a-59f4-4716-acb8-cc282cc7526d", "6095890e-d648-4068-a573-bec722a25dc9" },
                    { "e3c6c525-2475-4fe4-ac7d-1df5bb6f0ed1", "a39d08d0-bc15-4003-8144-aa2eef919a77" },
                    { "3225bc27-4b0c-48b8-a33c-18fddd1e3649", "c4a5cf1a-e17a-4c4c-aefa-ca1c78abb703" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_PropertyId",
                table: "Amenity",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupBookingOrderOrderId",
                table: "AspNetUsers",
                column: "GroupBookingOrderOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProviderId",
                table: "AspNetUsers",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedDate_PropertyId",
                table: "BlockedDate",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingMessage_ApplicationUserId",
                table: "BookingMessage",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrder_ApplicationUserId",
                table: "BookingOrder",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrder_PaymentId",
                table: "BookingOrder",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrder_PropertyId",
                table: "BookingOrder",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ApplicationUserId",
                table: "ChatMessage",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ChatId",
                table: "ChatMessage",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_PropertyId",
                table: "Discount",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ApplicationUserId",
                table: "Favorites",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PropertyId",
                table: "Favorites",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBookingOrder_ApplicationUserId",
                table: "GroupBookingOrder",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBookingOrder_GroupId",
                table: "GroupBookingOrder",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBookingOrder_PropertyId",
                table: "GroupBookingOrder",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOrderPayment_ApplicationUserId",
                table: "GroupOrderPayment",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOrderPayment_GroupBookingOrderOrderId",
                table: "GroupOrderPayment",
                column: "GroupBookingOrderOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOrderPayment_PaymentId",
                table: "GroupOrderPayment",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ChatId",
                table: "Groups",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoteOrder_ApplicationUserId",
                table: "PromoteOrder",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoteOrder_PaymentId",
                table: "PromoteOrder",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoteOrder_PropertyId",
                table: "PromoteOrder",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOrder_ApplicationUserId",
                table: "PromotionOrder",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOrder_PaymentId",
                table: "PromotionOrder",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOrder_PropertyId",
                table: "PromotionOrder",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ApplicationUserId",
                table: "Rating",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddedProperty_ApplicationUserId",
                table: "UserAddedProperty",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddedProperty_GroupId",
                table: "UserAddedProperty",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddedProperty_PropertyId",
                table: "UserAddedProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVote_ApplicationUserId",
                table: "UserVote",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVote_GroupId",
                table: "UserVote",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVote_PropertyId",
                table: "UserVote",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GroupBookingOrder_GroupBookingOrderOrderId",
                table: "AspNetUsers",
                column: "GroupBookingOrderOrderId",
                principalTable: "GroupBookingOrder",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupBookingOrder_Property_PropertyId",
                table: "GroupBookingOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupBookingOrder_AspNetUsers_ApplicationUserId",
                table: "GroupBookingOrder");

            migrationBuilder.DropTable(
                name: "Amenity");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlockedDate");

            migrationBuilder.DropTable(
                name: "BookingMessage");

            migrationBuilder.DropTable(
                name: "BookingOrder");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "GroupOrderPayment");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "PromoteOrder");

            migrationBuilder.DropTable(
                name: "PromotionOrder");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "UserAddedProperty");

            migrationBuilder.DropTable(
                name: "UserVote");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetProviders");

            migrationBuilder.DropTable(
                name: "GroupBookingOrder");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Chat");
        }
    }
}
