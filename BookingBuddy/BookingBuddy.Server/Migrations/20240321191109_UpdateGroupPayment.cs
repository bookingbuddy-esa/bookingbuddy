﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_GroupBookingOrder_GroupBookingOrderOrderId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_GroupBookingOrderOrderId",
                table: "Payment");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "08b9b067-c893-40c4-a55a-e5bbc50db10f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "10402366-3458-4288-959b-97e325862780");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "28dbd770-289a-4743-a296-027e50270600");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2e2cca70-22d4-47a2-b804-2b6f5924d3d6");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "34923ddc-6f20-4d0b-94d6-dfdc95520d5b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "34b34579-ef29-47d6-bdde-166ef26cecdb");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "3cb24154-ce16-4103-8d5c-7728cbae3f1a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "6fbbc417-687d-49ed-bf36-9550f8b3d6f0");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "79b38007-b616-4472-a186-8d1fe00ae430");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "96613c36-23f3-4682-bb6f-5d401b59684b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "96821fe6-e7f8-48b2-ad31-1142f43f2af0");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f65e181e-d963-4554-86d9-c9ff1e4e10b1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f6b32234-7506-44ea-b1f9-3fb4688e0714");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "3038d074-0d21-41f2-989a-1cb2f20e88a4");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "f20e6820-7dbc-46c1-bedc-ff86eeb851a5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7ab24145-e54a-4c2e-8f8b-879c47c87e03", "16288bfd-42c6-43c5-a61c-c131c79b97cc" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9375e488-b614-46a3-880d-8bdb700c3798", "795f7097-c31b-403e-80b4-4cd02e32c625" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a159784f-3384-4359-a76e-cd1a66bb2700", "a3422c87-d7f7-4354-8f54-52dcc9c91add" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ab24145-e54a-4c2e-8f8b-879c47c87e03");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9375e488-b614-46a3-880d-8bdb700c3798");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a159784f-3384-4359-a76e-cd1a66bb2700");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "16288bfd-42c6-43c5-a61c-c131c79b97cc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "795f7097-c31b-403e-80b4-4cd02e32c625");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a3422c87-d7f7-4354-8f54-52dcc9c91add");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "7a454b3b-18ed-4ecf-8ab7-86a1c70a0283");

            migrationBuilder.DropColumn(
                name: "GroupBookingOrderOrderId",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "PaymentIds",
                table: "GroupBookingOrder",
                newName: "GroupPaymentIds");

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
                    { "0492f28c-f605-4264-9610-529a22c5c37a", "Cozinha", "Cozinha", null },
                    { "0e13b042-c13c-4c52-a658-349263a75307", "Microondas", "Microondas", null },
                    { "1f499f89-c770-4c1a-95b1-b7263970c43c", "Frigorífico", "Frigorifico", null },
                    { "25aadef9-745d-4cb3-a1a0-823376717c2f", "Quintal", "Quintal", null },
                    { "40c3e8ac-d5f5-4237-ade7-d3f8653f037e", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "5ad6a3f6-67cd-4af7-a57d-1e5a5426c362", "Varanda", "Varanda", null },
                    { "8b6668ff-2cdd-4ae5-8cc6-4f7c47572753", "Estacionamento", "Estacionamento", null },
                    { "aa453564-b450-4658-ae61-6e9ad29ef64f", "Câmaras", "Camaras", null },
                    { "b83cea29-8a8e-4611-bd58-ae4b96ca998f", "Piscina Individual", "PiscinaIndividual", null },
                    { "bd01adcc-780e-4627-a7cb-8014bf21722a", "Animais", "Animais", null },
                    { "c562f53f-fd7c-4159-bf3f-7dba3785ca93", "Wifi", "Wifi", null },
                    { "ceb613f9-39b6-4aa2-bbf5-6b513626d701", "TV", "Tv", null },
                    { "d13df46e-57ce-4ab4-82c5-34171452ce26", "Máquina de Lavar", "MaquinaLavar", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "64a2992e-6c3e-4acd-84ba-f0909a283ccf", "local", "LOCAL" },
                    { "e4b4d1a2-b081-4b64-adc0-e358be124ba6", "google", "GOOGLE" },
                    { "fa4787b0-91a3-4b0c-a202-f6b888eca0d6", "microsoft", "MICROSOFT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3d3c9a02-9167-4865-9ac3-c0d59090ed10", "180f7198-34c5-48f6-be4b-1298b3f5f3eb", "user", "USER" },
                    { "402f1101-ea52-4da9-9806-28f016f1281d", "e8956faa-e635-4980-9ebf-80b282b695b1", "landlord", "LANDLORD" },
                    { "86775dab-6809-4f1d-85da-345dd2ed8b2f", "aeb7a444-9520-4049-9332-e485f744262a", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "24f32507-095b-436c-90d5-69c08a06b064", 0, "66b71ffd-ee63-42c7-ab5d-cd45f5fae6a6", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEIwt7QU8kcJg+An04XT62oxIu3d/H2TWaQ+lH1UlxLgY3s9/LPr0Wzm0XuB5MKNqOw==", null, false, null, "64a2992e-6c3e-4acd-84ba-f0909a283ccf", "7aa32d91-ea4b-4f34-afd7-700688609ae2", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "6cbb8832-1451-44b7-b3b2-1c1246eb272d", 0, "3f974901-d5c1-4650-8b2c-ffd60e64fc84", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJcGBnQXOImzr7Rf2zL2nHt3heiJyGxG/61d9/mENJEeSZg7tHvWQU6W8B4U58G9jg==", null, false, null, "64a2992e-6c3e-4acd-84ba-f0909a283ccf", "7abb5b1b-76c2-4420-82c6-db16d0b9069d", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "a2241e98-7f70-4645-85aa-e8487514e2b1", 0, "c6623671-8fda-4409-8357-59740a3f8872", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJ7opd7+Oxlpj4lEPCyR85g21cp92pntv/DIbzp5IWLUBHlX1IerXf/pRy9WqkBVlA==", null, false, null, "64a2992e-6c3e-4acd-84ba-f0909a283ccf", "14ba30f7-b8ed-4a8b-b9a6-96b5b4df0db9", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "86775dab-6809-4f1d-85da-345dd2ed8b2f", "24f32507-095b-436c-90d5-69c08a06b064" },
                    { "402f1101-ea52-4da9-9806-28f016f1281d", "6cbb8832-1451-44b7-b3b2-1c1246eb272d" },
                    { "3d3c9a02-9167-4865-9ac3-c0d59090ed10", "a2241e98-7f70-4645-85aa-e8487514e2b1" }
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupOrderPayment");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "0492f28c-f605-4264-9610-529a22c5c37a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "0e13b042-c13c-4c52-a658-349263a75307");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "1f499f89-c770-4c1a-95b1-b7263970c43c");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "25aadef9-745d-4cb3-a1a0-823376717c2f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "40c3e8ac-d5f5-4237-ade7-d3f8653f037e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "5ad6a3f6-67cd-4af7-a57d-1e5a5426c362");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "8b6668ff-2cdd-4ae5-8cc6-4f7c47572753");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "aa453564-b450-4658-ae61-6e9ad29ef64f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "b83cea29-8a8e-4611-bd58-ae4b96ca998f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "bd01adcc-780e-4627-a7cb-8014bf21722a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c562f53f-fd7c-4159-bf3f-7dba3785ca93");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "ceb613f9-39b6-4aa2-bbf5-6b513626d701");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "d13df46e-57ce-4ab4-82c5-34171452ce26");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "e4b4d1a2-b081-4b64-adc0-e358be124ba6");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "fa4787b0-91a3-4b0c-a202-f6b888eca0d6");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "86775dab-6809-4f1d-85da-345dd2ed8b2f", "24f32507-095b-436c-90d5-69c08a06b064" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "402f1101-ea52-4da9-9806-28f016f1281d", "6cbb8832-1451-44b7-b3b2-1c1246eb272d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3d3c9a02-9167-4865-9ac3-c0d59090ed10", "a2241e98-7f70-4645-85aa-e8487514e2b1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d3c9a02-9167-4865-9ac3-c0d59090ed10");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "402f1101-ea52-4da9-9806-28f016f1281d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86775dab-6809-4f1d-85da-345dd2ed8b2f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24f32507-095b-436c-90d5-69c08a06b064");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6cbb8832-1451-44b7-b3b2-1c1246eb272d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a2241e98-7f70-4645-85aa-e8487514e2b1");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "64a2992e-6c3e-4acd-84ba-f0909a283ccf");

            migrationBuilder.RenameColumn(
                name: "GroupPaymentIds",
                table: "GroupBookingOrder",
                newName: "PaymentIds");

            migrationBuilder.AddColumn<string>(
                name: "GroupBookingOrderOrderId",
                table: "Payment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "08b9b067-c893-40c4-a55a-e5bbc50db10f", "Piscina Individual", "PiscinaIndividual", null },
                    { "10402366-3458-4288-959b-97e325862780", "Varanda", "Varanda", null },
                    { "28dbd770-289a-4743-a296-027e50270600", "Microondas", "Microondas", null },
                    { "2e2cca70-22d4-47a2-b804-2b6f5924d3d6", "Câmaras", "Camaras", null },
                    { "34923ddc-6f20-4d0b-94d6-dfdc95520d5b", "TV", "Tv", null },
                    { "34b34579-ef29-47d6-bdde-166ef26cecdb", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "3cb24154-ce16-4103-8d5c-7728cbae3f1a", "Quintal", "Quintal", null },
                    { "6fbbc417-687d-49ed-bf36-9550f8b3d6f0", "Wifi", "Wifi", null },
                    { "79b38007-b616-4472-a186-8d1fe00ae430", "Estacionamento", "Estacionamento", null },
                    { "96613c36-23f3-4682-bb6f-5d401b59684b", "Cozinha", "Cozinha", null },
                    { "96821fe6-e7f8-48b2-ad31-1142f43f2af0", "Frigorífico", "Frigorifico", null },
                    { "f65e181e-d963-4554-86d9-c9ff1e4e10b1", "Máquina de Lavar", "MaquinaLavar", null },
                    { "f6b32234-7506-44ea-b1f9-3fb4688e0714", "Animais", "Animais", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3038d074-0d21-41f2-989a-1cb2f20e88a4", "microsoft", "MICROSOFT" },
                    { "7a454b3b-18ed-4ecf-8ab7-86a1c70a0283", "local", "LOCAL" },
                    { "f20e6820-7dbc-46c1-bedc-ff86eeb851a5", "google", "GOOGLE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7ab24145-e54a-4c2e-8f8b-879c47c87e03", "dc4b32ac-f688-431a-ba62-a3f223792baf", "admin", "ADMIN" },
                    { "9375e488-b614-46a3-880d-8bdb700c3798", "fa717685-be45-4796-8382-d4d6f35fb4e3", "user", "USER" },
                    { "a159784f-3384-4359-a76e-cd1a66bb2700", "26db206f-447e-4924-9d45-ed8e5d754c1d", "landlord", "LANDLORD" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "16288bfd-42c6-43c5-a61c-c131c79b97cc", 0, "a92ea8e4-845a-4990-8702-e6bde9c7ec5f", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEBuOQj5vO0g7to0vIogaDwNh0jVzoi93+JDhQjcRubQkjEgHLat/5dNy7fq1u2sy7g==", null, false, null, "7a454b3b-18ed-4ecf-8ab7-86a1c70a0283", "05115135-6ef0-4f36-8130-5c092f285acb", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "795f7097-c31b-403e-80b4-4cd02e32c625", 0, "fc9144b4-9cb8-44e6-aa3a-8385af6fe4d0", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEA+wiqT5EDEFvtFo/g4wqapNaNI98psGymTY0Tu1COWjXSXDkj1wONKIQXvlIhNGEA==", null, false, null, "7a454b3b-18ed-4ecf-8ab7-86a1c70a0283", "d88ef198-049f-40c4-927e-7c334711b189", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "a3422c87-d7f7-4354-8f54-52dcc9c91add", 0, "1d840da4-9124-4438-8bc8-a9f9a4b0ea83", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEK8YZJh4cdTsoaZ+ZxOgFpu8IaiwqGbg5cEd5NpCt0rqlZkIZ9nWSgmqjpwbZrBaYg==", null, false, null, "7a454b3b-18ed-4ecf-8ab7-86a1c70a0283", "71b2fab3-4511-4d11-813f-f83c3bff258c", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "7ab24145-e54a-4c2e-8f8b-879c47c87e03", "16288bfd-42c6-43c5-a61c-c131c79b97cc" },
                    { "9375e488-b614-46a3-880d-8bdb700c3798", "795f7097-c31b-403e-80b4-4cd02e32c625" },
                    { "a159784f-3384-4359-a76e-cd1a66bb2700", "a3422c87-d7f7-4354-8f54-52dcc9c91add" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GroupBookingOrderOrderId",
                table: "Payment",
                column: "GroupBookingOrderOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_GroupBookingOrder_GroupBookingOrderOrderId",
                table: "Payment",
                column: "GroupBookingOrderOrderId",
                principalTable: "GroupBookingOrder",
                principalColumn: "OrderId");
        }
    }
}
