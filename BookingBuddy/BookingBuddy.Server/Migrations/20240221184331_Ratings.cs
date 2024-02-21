using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class Ratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "02678946-c795-4f5f-a275-bdd1a47458e2");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "18de1faa-2410-4b3e-af21-70d8c1ab81e3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "207e817f-2216-417c-a2e8-f0c171c6ff9c");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "23160be8-58ec-49f0-ac55-f5fd901cc5c5");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "36bc61f1-ad84-4fb7-ab90-aebb53cd91b3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "48a6f285-ebb0-431c-b5fe-dad3ee78d5c1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "4db2672a-26e9-4ef9-b7bf-1d053ef5d19a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "618e02e6-70b8-4b3b-8a44-8c941705211b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "6fdf90e0-4ded-40c7-91f9-be5b39b159ad");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "7dea3adc-ee16-49ea-98cf-713e353fb452");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a6f87120-5941-4a8d-98c8-d7e0ca0b8ca8");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c8f9f45a-5559-427a-b43a-c0a29428237e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "da4682cd-5c3e-439a-b6f2-06d2e7b2d2e9");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "44d06b84-c32f-43d1-8416-a1d7350041e1");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "d5ed361d-4d58-481f-b4b2-9e15572dbaad");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d64f6808-3ab5-4d25-a8d0-1f26a42d09c5", "3e76b55b-85ae-40c7-86b0-07e8a45e5906" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b4b2606f-66d4-4d9e-b32e-d1769514d181", "44d6a801-6a06-43f4-89d4-29dde355fdac" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8a918fa2-3857-48c8-a760-70fefdefe3bd", "638373fa-2bd7-4d6a-acf7-03c1c6280dfc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a918fa2-3857-48c8-a760-70fefdefe3bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4b2606f-66d4-4d9e-b32e-d1769514d181");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d64f6808-3ab5-4d25-a8d0-1f26a42d09c5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3e76b55b-85ae-40c7-86b0-07e8a45e5906");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44d6a801-6a06-43f4-89d4-29dde355fdac");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "638373fa-2bd7-4d6a-acf7-03c1c6280dfc");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "2482cf58-d497-4dc9-9c6c-cc696ebf50ff");

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    RatingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.RatingId);
                });

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "07ab2812-bd3a-4c06-bfae-29c26f3f5f34", "Wifi", "Wifi", null },
                    { "13709112-c0cc-4e07-b997-c328034a868a", "Microondas", "Microondas", null },
                    { "19adb080-e7fd-428f-be9a-a6a82405ffd5", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "2538f3f0-8604-4283-befa-24975f39e47e", "Máquina de Lavar", "MaquinaLavar", null },
                    { "292f1b2c-36fb-4b13-91e4-d3b3d520792b", "Animais", "Animais", null },
                    { "299cc16a-c9ca-4841-ade4-535b4dd59fa3", "Quintal", "Quintal", null },
                    { "2aea0aba-61f8-4dec-a3fa-a32dcdc721a7", "Estacionamento", "Estacionamento", null },
                    { "31548fb5-ead0-47dc-b4c2-0f81c0c71946", "Varanda", "Varanda", null },
                    { "317cc2db-cef0-4c27-8ac9-bcc38e50f1ab", "TV", "Tv", null },
                    { "752d2a95-82c5-412d-bd1e-d7bc75ef8dc2", "Frigorífico", "Frigorifico", null },
                    { "91240cb2-bbb4-4a73-a845-06bb58e05413", "Piscina Individual", "PiscinaIndividual", null },
                    { "cfdf2bd7-c811-4002-abe6-7ca5b1eca58b", "Cozinha", "Cozinha", null },
                    { "f0a6a739-4031-4212-803a-859c42f5870e", "Câmaras", "Camaras", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5f81672c-0661-4c1f-94b1-f6f07be91e35", "local", "LOCAL" },
                    { "6e1ed02a-b052-4f02-a840-795d78ef2c41", "google", "GOOGLE" },
                    { "a44e0229-ab31-47af-af15-e1fc779fb752", "microsoft", "MICROSOFT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8b469e75-21f6-48e0-8e3d-c710506b5500", "89e77245-c8c6-4ef2-9f63-7cd047c151d2", "admin", "ADMIN" },
                    { "c347a4aa-e15e-437d-8d9c-fccfd4efc0b4", "ebebf1d1-5dbb-4212-a11c-b1e505276aee", "landlord", "LANDLORD" },
                    { "f2d3320c-78a2-45e7-a147-007f86e671eb", "d196d694-595a-4f20-b5da-d99ceae59d99", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "34582dcb-97b1-46a0-a888-804a25111527", 0, "b857bade-3638-4665-8095-3794b16724b9", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJ9HmPYNVXkyhOYwhCP7bwxDrWg1W2u1GRCQWSeZY1L6w9m9XONU874tGYLgHEq2xg==", null, false, null, "5f81672c-0661-4c1f-94b1-f6f07be91e35", "b216ded7-ca1c-4ca0-b48c-79e72314ab1a", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "39018233-203a-47ef-8a78-9d3a678c0aff", 0, "c6222d85-25f0-4c89-af2b-ee79fedae78a", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEOXa8F+siB+W9IURIZX8RXncx9C7BCn5RELl7jyqBPTn57+9H4Pn60n0gz9SqX2OKQ==", null, false, null, "5f81672c-0661-4c1f-94b1-f6f07be91e35", "4d976608-cea2-4d32-bf60-3d3e199e96e8", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "f8752e8b-4c81-4579-9a3c-57313c0f5647", 0, "cf9f61ad-b62f-4cf6-b5a9-71a7734418cf", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEEp0RIBjanhHQvMIyi+9TRqmcX+wgDQJ63OxD2cPupTgStEQ/BvMiGlnuerULC02wQ==", null, false, null, "5f81672c-0661-4c1f-94b1-f6f07be91e35", "b7bea9ad-d190-4728-a14a-33f6d35689ae", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8b469e75-21f6-48e0-8e3d-c710506b5500", "34582dcb-97b1-46a0-a888-804a25111527" },
                    { "c347a4aa-e15e-437d-8d9c-fccfd4efc0b4", "39018233-203a-47ef-8a78-9d3a678c0aff" },
                    { "f2d3320c-78a2-45e7-a147-007f86e671eb", "f8752e8b-4c81-4579-9a3c-57313c0f5647" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "07ab2812-bd3a-4c06-bfae-29c26f3f5f34");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "13709112-c0cc-4e07-b997-c328034a868a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "19adb080-e7fd-428f-be9a-a6a82405ffd5");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2538f3f0-8604-4283-befa-24975f39e47e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "292f1b2c-36fb-4b13-91e4-d3b3d520792b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "299cc16a-c9ca-4841-ade4-535b4dd59fa3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2aea0aba-61f8-4dec-a3fa-a32dcdc721a7");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "31548fb5-ead0-47dc-b4c2-0f81c0c71946");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "317cc2db-cef0-4c27-8ac9-bcc38e50f1ab");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "752d2a95-82c5-412d-bd1e-d7bc75ef8dc2");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "91240cb2-bbb4-4a73-a845-06bb58e05413");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "cfdf2bd7-c811-4002-abe6-7ca5b1eca58b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f0a6a739-4031-4212-803a-859c42f5870e");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "6e1ed02a-b052-4f02-a840-795d78ef2c41");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "a44e0229-ab31-47af-af15-e1fc779fb752");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8b469e75-21f6-48e0-8e3d-c710506b5500", "34582dcb-97b1-46a0-a888-804a25111527" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c347a4aa-e15e-437d-8d9c-fccfd4efc0b4", "39018233-203a-47ef-8a78-9d3a678c0aff" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f2d3320c-78a2-45e7-a147-007f86e671eb", "f8752e8b-4c81-4579-9a3c-57313c0f5647" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b469e75-21f6-48e0-8e3d-c710506b5500");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c347a4aa-e15e-437d-8d9c-fccfd4efc0b4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2d3320c-78a2-45e7-a147-007f86e671eb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "34582dcb-97b1-46a0-a888-804a25111527");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "39018233-203a-47ef-8a78-9d3a678c0aff");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f8752e8b-4c81-4579-9a3c-57313c0f5647");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "5f81672c-0661-4c1f-94b1-f6f07be91e35");

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "02678946-c795-4f5f-a275-bdd1a47458e2", "Animais", "Animais", null },
                    { "18de1faa-2410-4b3e-af21-70d8c1ab81e3", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "207e817f-2216-417c-a2e8-f0c171c6ff9c", "Varanda", "Varanda", null },
                    { "23160be8-58ec-49f0-ac55-f5fd901cc5c5", "Estacionamento", "Estacionamento", null },
                    { "36bc61f1-ad84-4fb7-ab90-aebb53cd91b3", "Wifi", "Wifi", null },
                    { "48a6f285-ebb0-431c-b5fe-dad3ee78d5c1", "Quintal", "Quintal", null },
                    { "4db2672a-26e9-4ef9-b7bf-1d053ef5d19a", "Máquina de Lavar", "MaquinaLavar", null },
                    { "618e02e6-70b8-4b3b-8a44-8c941705211b", "Cozinha", "Cozinha", null },
                    { "6fdf90e0-4ded-40c7-91f9-be5b39b159ad", "Frigorífico", "Frigorifico", null },
                    { "7dea3adc-ee16-49ea-98cf-713e353fb452", "Piscina Individual", "PiscinaIndividual", null },
                    { "a6f87120-5941-4a8d-98c8-d7e0ca0b8ca8", "TV", "Tv", null },
                    { "c8f9f45a-5559-427a-b43a-c0a29428237e", "Microondas", "Microondas", null },
                    { "da4682cd-5c3e-439a-b6f2-06d2e7b2d2e9", "Câmaras", "Camaras", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2482cf58-d497-4dc9-9c6c-cc696ebf50ff", "local", "LOCAL" },
                    { "44d06b84-c32f-43d1-8416-a1d7350041e1", "google", "GOOGLE" },
                    { "d5ed361d-4d58-481f-b4b2-9e15572dbaad", "microsoft", "MICROSOFT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a918fa2-3857-48c8-a760-70fefdefe3bd", "64ef41d3-ea51-430f-9ac9-ffc4290f08c4", "landlord", "LANDLORD" },
                    { "b4b2606f-66d4-4d9e-b32e-d1769514d181", "ef4819ff-4cc3-4d0f-89c6-f7558d07bbed", "user", "USER" },
                    { "d64f6808-3ab5-4d25-a8d0-1f26a42d09c5", "95981013-6ca3-42aa-a86a-86c0c80a8a71", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "3e76b55b-85ae-40c7-86b0-07e8a45e5906", 0, "66c13fa9-20ce-45b5-8d68-996e9296fbf1", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEDzvhtXom4BwlMFkXAVWrM/hehNXiyFtudXQN2iyz+6bpAfeQx1mUll/1ha3GkEZFg==", null, false, null, "2482cf58-d497-4dc9-9c6c-cc696ebf50ff", "63c327ce-869d-4ec1-b6ee-8e0417659d17", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "44d6a801-6a06-43f4-89d4-29dde355fdac", 0, "37d6cad2-a3bf-401c-9c58-5aef0c68e594", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEFtstzlbdE+l+QvPz5r/ryX503ekhElXzVG1GIGOGNAh7BWVMJybsRH3J0XwP5Ckcw==", null, false, null, "2482cf58-d497-4dc9-9c6c-cc696ebf50ff", "dc20d55f-6ec9-4b2b-b76b-d4318f9b3f69", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "638373fa-2bd7-4d6a-acf7-03c1c6280dfc", 0, "5d3727fe-6b90-4e69-b64d-ce7e9eecaf6a", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEMu7jvVIubedlaAP6Gj2Hwe6/LoWVo3sTyI1zW2z1KP/dQ+fnJ6kYtVacwUTMWLkSg==", null, false, null, "2482cf58-d497-4dc9-9c6c-cc696ebf50ff", "5a83c439-fc98-48a9-8da6-d2ab8ae78d61", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "d64f6808-3ab5-4d25-a8d0-1f26a42d09c5", "3e76b55b-85ae-40c7-86b0-07e8a45e5906" },
                    { "b4b2606f-66d4-4d9e-b32e-d1769514d181", "44d6a801-6a06-43f4-89d4-29dde355fdac" },
                    { "8a918fa2-3857-48c8-a760-70fefdefe3bd", "638373fa-2bd7-4d6a-acf7-03c1c6280dfc" }
                });
        }
    }
}
