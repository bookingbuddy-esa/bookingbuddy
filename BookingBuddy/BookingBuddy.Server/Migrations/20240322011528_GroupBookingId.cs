using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class GroupBookingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "GroupBookingId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "20db2184-6173-41ec-803a-4a78a04a2283", "Estacionamento", "Estacionamento", null },
                    { "23930b19-8442-40af-88dd-b5de8d31cf29", "Câmaras", "Camaras", null },
                    { "346424a1-1d96-43cf-9868-f890d3c65d47", "Frigorífico", "Frigorifico", null },
                    { "38c57a27-9e93-4f62-a588-9ff03c8a8b59", "TV", "Tv", null },
                    { "7dcd2fcb-3df2-47d1-b4c6-54a2528ce713", "Quintal", "Quintal", null },
                    { "846781ce-4a4c-4823-91a7-0ee2dcb9975d", "Cozinha", "Cozinha", null },
                    { "8856a693-905a-4a26-ad3e-7e9fd6f83f94", "Máquina de Lavar", "MaquinaLavar", null },
                    { "90090ade-52c9-4986-8b8f-d623beb21ee0", "Microondas", "Microondas", null },
                    { "9fe5673d-f464-4b16-86d0-dbd84bd7203f", "Animais", "Animais", null },
                    { "bbe421fb-521d-47ca-bce7-b3ddc5b09d18", "Varanda", "Varanda", null },
                    { "cf7a5d47-1580-4439-9875-7d1d1f048824", "Piscina Individual", "PiscinaIndividual", null },
                    { "d91e0c40-7ba2-453e-972a-31aaecd20eb4", "Wifi", "Wifi", null },
                    { "ffd92540-4a88-46ce-b11f-800e8b506612", "Piscina Partilhada", "PiscinaPartilhada", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3705a3ec-f709-4280-9fcd-5b539a9f979d", "local", "LOCAL" },
                    { "7c682ea5-308d-4200-a3fb-6480c2b1d436", "google", "GOOGLE" },
                    { "acbf771c-f1d5-4b33-8c4a-f9cf669ff5be", "microsoft", "MICROSOFT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0067b0f8-f13e-47e1-b7b9-50ad47455787", "11ab71f7-dce9-40f1-84d3-a859d6c9332f", "user", "USER" },
                    { "2de4ba6a-ce30-4342-8fca-b5de17bcba8e", "bcd87a7a-7100-4493-852d-84b5921cfeac", "landlord", "LANDLORD" },
                    { "a590a723-ef34-4bdb-9d86-750a3f4f275f", "e0e28780-372f-48b4-8e53-be1b634d14ed", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "02bc65a8-8cc5-4aa5-af86-efbf2fb209e7", 0, "0f7df634-14ce-46e9-b3ba-a2c8ced2b5a2", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJFmlVsXO3MktpgNrtNx7voPvIbZw/y/OIBc+eo4Fvgx/2PDR5Tk/j1sqn8jQ3Pd9g==", null, false, null, "3705a3ec-f709-4280-9fcd-5b539a9f979d", "baae0281-32d6-4c1d-9e1b-dba675b0ef7c", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "83120563-485e-48ae-b519-d098aa902761", 0, "95e35bcc-02ba-4626-b1ab-841ea50b1bb7", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAELXYk92aVr/pJigtzleualggPmqc4PqFQK5dBtHtJlW4YZiG8WpTk9oJS0aCjuB/0A==", null, false, null, "3705a3ec-f709-4280-9fcd-5b539a9f979d", "6c8b6850-b9d4-4e92-8360-08715ed01faa", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "8e6afbc3-d814-4b9b-a265-97de795432dd", 0, "e00bb065-3657-4f88-a5dc-aa16ca03545e", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAECjUZfYejVSNCRTQwTxh/TPpF59kFeTEScB/cgjtglUTFsSlHGm0uG/JJ/8o5LLqVQ==", null, false, null, "3705a3ec-f709-4280-9fcd-5b539a9f979d", "47425c6e-3e35-41d7-95d1-0e3abf07e222", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a590a723-ef34-4bdb-9d86-750a3f4f275f", "02bc65a8-8cc5-4aa5-af86-efbf2fb209e7" },
                    { "2de4ba6a-ce30-4342-8fca-b5de17bcba8e", "83120563-485e-48ae-b519-d098aa902761" },
                    { "0067b0f8-f13e-47e1-b7b9-50ad47455787", "8e6afbc3-d814-4b9b-a265-97de795432dd" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "20db2184-6173-41ec-803a-4a78a04a2283");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "23930b19-8442-40af-88dd-b5de8d31cf29");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "346424a1-1d96-43cf-9868-f890d3c65d47");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "38c57a27-9e93-4f62-a588-9ff03c8a8b59");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "7dcd2fcb-3df2-47d1-b4c6-54a2528ce713");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "846781ce-4a4c-4823-91a7-0ee2dcb9975d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "8856a693-905a-4a26-ad3e-7e9fd6f83f94");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "90090ade-52c9-4986-8b8f-d623beb21ee0");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "9fe5673d-f464-4b16-86d0-dbd84bd7203f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "bbe421fb-521d-47ca-bce7-b3ddc5b09d18");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "cf7a5d47-1580-4439-9875-7d1d1f048824");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "d91e0c40-7ba2-453e-972a-31aaecd20eb4");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "ffd92540-4a88-46ce-b11f-800e8b506612");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "7c682ea5-308d-4200-a3fb-6480c2b1d436");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "acbf771c-f1d5-4b33-8c4a-f9cf669ff5be");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a590a723-ef34-4bdb-9d86-750a3f4f275f", "02bc65a8-8cc5-4aa5-af86-efbf2fb209e7" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2de4ba6a-ce30-4342-8fca-b5de17bcba8e", "83120563-485e-48ae-b519-d098aa902761" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0067b0f8-f13e-47e1-b7b9-50ad47455787", "8e6afbc3-d814-4b9b-a265-97de795432dd" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0067b0f8-f13e-47e1-b7b9-50ad47455787");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2de4ba6a-ce30-4342-8fca-b5de17bcba8e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a590a723-ef34-4bdb-9d86-750a3f4f275f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02bc65a8-8cc5-4aa5-af86-efbf2fb209e7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "83120563-485e-48ae-b519-d098aa902761");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e6afbc3-d814-4b9b-a265-97de795432dd");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "3705a3ec-f709-4280-9fcd-5b539a9f979d");

            migrationBuilder.DropColumn(
                name: "GroupBookingId",
                table: "Groups");

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
        }
    }
}
