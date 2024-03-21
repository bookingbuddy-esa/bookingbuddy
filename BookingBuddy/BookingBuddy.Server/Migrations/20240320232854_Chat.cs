using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class Chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "10988608-f96f-475e-bf7c-4cd44015978d", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "2a70fc7b-3f74-4149-aff4-3bf3987b5227", "Câmaras", "Camaras", null },
                    { "2d02d683-782b-463d-aaa1-53dd8491aefa", "Varanda", "Varanda", null },
                    { "2dc133f1-3ea6-481a-9c4a-ad2e4fd248f4", "Wifi", "Wifi", null },
                    { "655f9c15-dfa6-46a0-9dc2-897093cac33c", "Cozinha", "Cozinha", null },
                    { "6aafb065-0d12-4a81-80b7-a229dfd99f97", "Microondas", "Microondas", null },
                    { "7cea49c4-062c-49c5-bc35-dd401a13f92e", "Piscina Individual", "PiscinaIndividual", null },
                    { "82e2f3c2-2f6f-48d5-901b-f9db9f17f9ca", "Estacionamento", "Estacionamento", null },
                    { "895a357b-1930-4f3a-80bf-a304d39fc5c4", "Frigorífico", "Frigorifico", null },
                    { "c304b202-b7c2-4978-afee-a79e81a50d74", "Máquina de Lavar", "MaquinaLavar", null },
                    { "d2d9e518-b471-45cb-a928-2fda18247e94", "Quintal", "Quintal", null },
                    { "e11f04aa-ace6-46a3-870b-f7f86f2c042d", "TV", "Tv", null },
                    { "f58507ef-34dc-4136-bd65-fd54271581e9", "Animais", "Animais", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15e787e0-c464-4198-8c45-a87303657821", "microsoft", "MICROSOFT" },
                    { "bb76ea25-f095-4211-9b68-0517945b216d", "google", "GOOGLE" },
                    { "f9e6df76-c2df-4f5a-b0aa-962d05492cbd", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "63bda852-f468-4966-9332-81d78f1af10d", "d00325e5-16fa-466e-85b3-7e048dd4f7cb", "user", "USER" },
                    { "97f1abdd-2e6f-4e0d-84f1-f91ca18a33d8", "39575e69-b44b-4af8-b95e-3ac1cfad292b", "landlord", "LANDLORD" },
                    { "ba825567-4152-4fac-a624-6829a6200bf2", "c9c39c66-30ab-4ac3-b5ac-a7254fb5384f", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "11b26de1-8046-461c-ab1f-cf9cd15dcf9a", 0, "31e422a9-1ad8-4427-ac15-d49d5ed6973a", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEIRx0/IQuAXv2GtOaIttHgQXYF9Lyojlu4CEXBxhtxfVl4IkF6yqgJc4Ds39bIKatw==", null, false, null, "f9e6df76-c2df-4f5a-b0aa-962d05492cbd", "6580c667-8662-4c09-bef3-085c9bd2ed5f", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "3c2942e4-ff24-4c07-83d3-5e06dd0c8f8c", 0, "23646fcf-bdb0-41a7-bc41-91bf0a7808dc", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEDDCO2O+qX1Fu0wCuTAozzhbVpJUH9vjkwIfI2oBCA2XOWkWMQrvlhYBMwbqyOSYQw==", null, false, null, "f9e6df76-c2df-4f5a-b0aa-962d05492cbd", "7f8a6fcf-dca6-4a3a-8a16-398da4b0d90e", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "81c34685-405d-4c18-9eb7-5700a3a9b958", 0, "2aa6c86c-bbaa-4a4b-92af-74117e98bf5c", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEMaH3/e++T1n60CtaVSwkwvurFWhGGCMjSUd2sgEjdoTcvcWVTozR+UGNzbwP8TRmA==", null, false, null, "f9e6df76-c2df-4f5a-b0aa-962d05492cbd", "e3a11459-8b48-4b84-9567-9589234dca71", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ba825567-4152-4fac-a624-6829a6200bf2", "11b26de1-8046-461c-ab1f-cf9cd15dcf9a" },
                    { "97f1abdd-2e6f-4e0d-84f1-f91ca18a33d8", "3c2942e4-ff24-4c07-83d3-5e06dd0c8f8c" },
                    { "63bda852-f468-4966-9332-81d78f1af10d", "81c34685-405d-4c18-9eb7-5700a3a9b958" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ApplicationUserId",
                table: "ChatMessage",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ChatId",
                table: "ChatMessage",
                column: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "10988608-f96f-475e-bf7c-4cd44015978d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2a70fc7b-3f74-4149-aff4-3bf3987b5227");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2d02d683-782b-463d-aaa1-53dd8491aefa");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2dc133f1-3ea6-481a-9c4a-ad2e4fd248f4");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "655f9c15-dfa6-46a0-9dc2-897093cac33c");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "6aafb065-0d12-4a81-80b7-a229dfd99f97");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "7cea49c4-062c-49c5-bc35-dd401a13f92e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "82e2f3c2-2f6f-48d5-901b-f9db9f17f9ca");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "895a357b-1930-4f3a-80bf-a304d39fc5c4");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c304b202-b7c2-4978-afee-a79e81a50d74");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "d2d9e518-b471-45cb-a928-2fda18247e94");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "e11f04aa-ace6-46a3-870b-f7f86f2c042d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f58507ef-34dc-4136-bd65-fd54271581e9");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "15e787e0-c464-4198-8c45-a87303657821");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "bb76ea25-f095-4211-9b68-0517945b216d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ba825567-4152-4fac-a624-6829a6200bf2", "11b26de1-8046-461c-ab1f-cf9cd15dcf9a" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "97f1abdd-2e6f-4e0d-84f1-f91ca18a33d8", "3c2942e4-ff24-4c07-83d3-5e06dd0c8f8c" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "63bda852-f468-4966-9332-81d78f1af10d", "81c34685-405d-4c18-9eb7-5700a3a9b958" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63bda852-f468-4966-9332-81d78f1af10d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97f1abdd-2e6f-4e0d-84f1-f91ca18a33d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba825567-4152-4fac-a624-6829a6200bf2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "11b26de1-8046-461c-ab1f-cf9cd15dcf9a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c2942e4-ff24-4c07-83d3-5e06dd0c8f8c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "81c34685-405d-4c18-9eb7-5700a3a9b958");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "f9e6df76-c2df-4f5a-b0aa-962d05492cbd");

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
        }
    }
}
