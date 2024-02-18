using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class Clicks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "9d19f1a6-0a1a-478a-bcf9-02234f688367");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "e2f08010-89a9-4225-8ddf-ab5f203e3494");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ac30bd48-cc51-4947-ad2e-9785aeabb38c", "5266930d-952d-46bd-9fe3-495d7d66660e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e2eb363a-7bb4-4847-aff1-abbdd152fdfa", "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "af6f0000-a53d-4287-8d77-2b81c1a65d0d", "b7111238-968e-4b2f-8a49-9cc4969a5624" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac30bd48-cc51-4947-ad2e-9785aeabb38c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af6f0000-a53d-4287-8d77-2b81c1a65d0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2eb363a-7bb4-4847-aff1-abbdd152fdfa");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5266930d-952d-46bd-9fe3-495d7d66660e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b7111238-968e-4b2f-8a49-9cc4969a5624");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "599b5787-f070-4d1d-8c91-17631c17bc7d");

            migrationBuilder.AddColumn<int>(
                name: "Clicks",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingOrder",
                columns: table => new
                {
                    BookingOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingOrder", x => x.BookingOrderId);
                    table.ForeignKey(
                        name: "FK_BookingOrder_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromoteOrder",
                columns: table => new
                {
                    PromoteOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoteOrder", x => x.PromoteOrderId);
                    table.ForeignKey(
                        name: "FK_PromoteOrder_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionOrder",
                columns: table => new
                {
                    PromotionOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionOrder", x => x.PromotionOrderId);
                    table.ForeignKey(
                        name: "FK_PromotionOrder_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ea9abb9-f0ee-456f-9d80-5c75d95ca7a1", "microsoft", "MICROSOFT" },
                    { "4b8bcf90-dfa6-4394-9d53-9686cd0e9ed9", "google", "GOOGLE" },
                    { "4b915b1f-8b74-45cf-979e-b10f8b98f919", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "42630d5b-ba89-4955-a36a-e31dd8cf7705", "5e69ee78-9632-42a3-9e56-15bb46da26e7", "admin", "ADMIN" },
                    { "61656fea-ed23-460c-bed2-acae223beeef", "3f93db4b-e044-4640-96a3-40093425fab0", "user", "USER" },
                    { "acb67e3b-29f7-409a-af25-8dac9168fe56", "4f564740-89b2-4e57-93f5-579aab7b661f", "landlord", "LANDLORD" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362", 0, "5cd1d3aa-e538-44c5-8af6-e59520e2a973", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJtcHc1qCjtJnehNWdJ2Gk5wV41Dd+AKDkkOvzVMUOIJ/haHphiSaGKBKgl0mib7+A==", null, false, null, "4b915b1f-8b74-45cf-979e-b10f8b98f919", "676321b8-1748-4e2d-9b6e-602849ecf26c", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "2de95519-31eb-459e-8ad1-5021989213f5", 0, "01c1e30e-888f-4f11-91e9-875ba385cbe5", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJil2s4PH5/oiIIzET0MZif5nEwFry6KmL4TzvDxC04ljdbVtJ4oyvW3IKxjeeT73Q==", null, false, null, "4b915b1f-8b74-45cf-979e-b10f8b98f919", "f372039e-cc5c-4961-95d5-daba427cf305", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "48996947-a573-4470-8d32-7797a2bac0e5", 0, "1865f161-f61a-45cd-a8f5-f7c58e2a040a", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEEzFRjlpejUeQ2FduLqTCg+K1pAhDOKOgbm9hiZ2mwlYaTN718YVLhUXnZ96HXOvmQ==", null, false, null, "4b915b1f-8b74-45cf-979e-b10f8b98f919", "286c81f7-66b2-460e-97b3-c0876b8b5b16", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "42630d5b-ba89-4955-a36a-e31dd8cf7705", "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362" },
                    { "acb67e3b-29f7-409a-af25-8dac9168fe56", "2de95519-31eb-459e-8ad1-5021989213f5" },
                    { "61656fea-ed23-460c-bed2-acae223beeef", "48996947-a573-4470-8d32-7797a2bac0e5" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrder_OrderId",
                table: "BookingOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentId",
                table: "Order",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PropertyId",
                table: "Order",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoteOrder_OrderId",
                table: "PromoteOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOrder_OrderId",
                table: "PromotionOrder",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingOrder");

            migrationBuilder.DropTable(
                name: "PromoteOrder");

            migrationBuilder.DropTable(
                name: "PromotionOrder");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "1ea9abb9-f0ee-456f-9d80-5c75d95ca7a1");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "4b8bcf90-dfa6-4394-9d53-9686cd0e9ed9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "42630d5b-ba89-4955-a36a-e31dd8cf7705", "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "acb67e3b-29f7-409a-af25-8dac9168fe56", "2de95519-31eb-459e-8ad1-5021989213f5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "61656fea-ed23-460c-bed2-acae223beeef", "48996947-a573-4470-8d32-7797a2bac0e5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42630d5b-ba89-4955-a36a-e31dd8cf7705");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61656fea-ed23-460c-bed2-acae223beeef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acb67e3b-29f7-409a-af25-8dac9168fe56");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2de95519-31eb-459e-8ad1-5021989213f5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "48996947-a573-4470-8d32-7797a2bac0e5");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "4b915b1f-8b74-45cf-979e-b10f8b98f919");

            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Property");

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "599b5787-f070-4d1d-8c91-17631c17bc7d", "local", "LOCAL" },
                    { "9d19f1a6-0a1a-478a-bcf9-02234f688367", "microsoft", "MICROSOFT" },
                    { "e2f08010-89a9-4225-8ddf-ab5f203e3494", "google", "GOOGLE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ac30bd48-cc51-4947-ad2e-9785aeabb38c", "d78d64f6-ff5e-4bf9-bca7-bd8c402d16e3", "landlord", "LANDLORD" },
                    { "af6f0000-a53d-4287-8d77-2b81c1a65d0d", "482674ba-d832-414d-b2ad-68571152157d", "user", "USER" },
                    { "e2eb363a-7bb4-4847-aff1-abbdd152fdfa", "5e8a3126-472b-4d27-9ee9-7aba0ff59e2a", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5266930d-952d-46bd-9fe3-495d7d66660e", 0, "a619f5ae-aaef-4211-be88-6ee1ee17ce4f", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAECx/ejlklKQ3t9LDtEjyOjoJw+Qzk2q4n0KgKRFcK26Ime4e1O050ahLlbP0I/rmjw==", null, false, null, "599b5787-f070-4d1d-8c91-17631c17bc7d", "e098a394-329d-476f-9f46-323f1b63d691", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9", 0, "5767ba9d-167e-42a8-b3ec-8501589ed7f0", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEFAlB/VVqc5VZXAvHDdyb/lp/GJvdZhfVJWhj2P43JTCsqrpMM1Y5CHyxof1NHFKxA==", null, false, null, "599b5787-f070-4d1d-8c91-17631c17bc7d", "693c4b38-bfb8-4e70-a718-d0962282add6", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "b7111238-968e-4b2f-8a49-9cc4969a5624", 0, "b7b2f876-b387-42b2-bf25-32b0d39634b2", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEN0bgrJ6qQsPY2pvqenJSsnqw1NAB8CpMTldTIciL3N3oyrIo8YZKIlm2vUm47vXiw==", null, false, null, "599b5787-f070-4d1d-8c91-17631c17bc7d", "0b65d11a-d2bf-4358-b950-095ad96f2fd4", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ac30bd48-cc51-4947-ad2e-9785aeabb38c", "5266930d-952d-46bd-9fe3-495d7d66660e" },
                    { "e2eb363a-7bb4-4847-aff1-abbdd152fdfa", "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9" },
                    { "af6f0000-a53d-4287-8d77-2b81c1a65d0d", "b7111238-968e-4b2f-8a49-9cc4969a5624" }
                });
        }
    }
}
