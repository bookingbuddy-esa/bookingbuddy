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

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "08da675b-4c42-4c92-bc80-8cf530451531", "Máquina de Lavar", "MaquinaLavar", null },
                    { "145e667c-a7fd-488b-9243-4379f528b903", "Cozinha", "Cozinha", null },
                    { "2913e613-e087-463f-8bea-abd99000ed32", "Microondas", "Microondas", null },
                    { "330c8afc-5843-4e2e-a482-e9476d3b5646", "TV", "Tv", null },
                    { "3a57446f-067d-4308-9bed-589137abcbcd", "Animais", "Animais", null },
                    { "885607aa-28a3-4c40-b0af-6548c32e3ad3", "Wifi", "Wifi", null },
                    { "8a9ce2c6-2a48-4291-b4e4-8d54b14a370d", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "a04dd010-bb74-49de-bc5e-66269a31ec4c", "Estacionamento", "Estacionamento", null },
                    { "a8cc57a0-430c-466e-bee3-bb0d62385f4a", "Câmaras", "Camaras", null },
                    { "ad5c3780-d476-4ee8-b69e-04cba14613e1", "Piscina Individual", "PiscinaIndividual", null },
                    { "d1264673-520b-44ea-b059-d1167884fe81", "Quintal", "Quintal", null },
                    { "dd8fb3cc-583c-462b-bed5-4f89e04d5b3b", "Frigorífico", "Frigorifico", null },
                    { "ed6fd758-5c19-4b53-94bc-61851ef0c57f", "Varanda", "Varanda", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c393618-868d-404d-bd1d-7f0a4cf9f5c5", "google", "GOOGLE" },
                    { "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2", "local", "LOCAL" },
                    { "eddd2412-121a-45f4-bde4-488bb49097c3", "microsoft", "MICROSOFT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e34358a-343b-4db0-9e91-e5f399cacbd6", "ed1b0558-59df-4b99-8422-ede7f06ef68d", "landlord", "LANDLORD" },
                    { "7ab35194-5997-4185-90ed-c2edabb63b96", "97f698ab-5ba5-424d-9c31-b43b65793e40", "admin", "ADMIN" },
                    { "c4111660-4886-42a0-9722-2830bfac1cc7", "b0a9d8f1-87af-4747-9264-215a2a1701bf", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "129808f1-c20d-4fa1-a34a-689bc0b89f9a", 0, "a2890ca3-40ba-431f-9d51-866245553700", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEP60aljRUtT9KdULM3RcINYm9ibZJHWqUwpmlB1uiKTeGeORy68O4Ivy4pxb4M0YkQ==", null, false, null, "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2", "27e72b63-7b68-4d93-82c0-28592ca74255", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "8f6ba4eb-12a6-4958-a554-da4df2de5bb5", 0, "99aabb2f-d474-41e6-a3c4-21f143efbd9f", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEFxssz0++kaf4JaAAJgSwkvYJj4OnfGRIPhgjRi2ZFoUU//ApvNNKRU7IY2AxZ2KXg==", null, false, null, "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2", "21c853d4-4571-422b-850b-42657b885c1c", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "d2a440c0-732b-48da-b815-1105710ef153", 0, "e39d7a5b-4e38-441e-bf98-4e6325cd3716", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAED+1wfPNRc5l/UxYsQpWNND1SjDyddoekC/ztbZamYM21iaG09Wx9pLLR5OhzdEVlg==", null, false, null, "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2", "b21e6875-8c29-42cf-bd53-28a2fb282395", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "5e34358a-343b-4db0-9e91-e5f399cacbd6", "129808f1-c20d-4fa1-a34a-689bc0b89f9a" },
                    { "7ab35194-5997-4185-90ed-c2edabb63b96", "8f6ba4eb-12a6-4958-a554-da4df2de5bb5" },
                    { "c4111660-4886-42a0-9722-2830bfac1cc7", "d2a440c0-732b-48da-b815-1105710ef153" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ApplicationUserId",
                table: "Rating",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "08da675b-4c42-4c92-bc80-8cf530451531");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "145e667c-a7fd-488b-9243-4379f528b903");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2913e613-e087-463f-8bea-abd99000ed32");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "330c8afc-5843-4e2e-a482-e9476d3b5646");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "3a57446f-067d-4308-9bed-589137abcbcd");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "885607aa-28a3-4c40-b0af-6548c32e3ad3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "8a9ce2c6-2a48-4291-b4e4-8d54b14a370d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a04dd010-bb74-49de-bc5e-66269a31ec4c");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a8cc57a0-430c-466e-bee3-bb0d62385f4a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "ad5c3780-d476-4ee8-b69e-04cba14613e1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "d1264673-520b-44ea-b059-d1167884fe81");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "dd8fb3cc-583c-462b-bed5-4f89e04d5b3b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "ed6fd758-5c19-4b53-94bc-61851ef0c57f");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "4c393618-868d-404d-bd1d-7f0a4cf9f5c5");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "eddd2412-121a-45f4-bde4-488bb49097c3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5e34358a-343b-4db0-9e91-e5f399cacbd6", "129808f1-c20d-4fa1-a34a-689bc0b89f9a" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7ab35194-5997-4185-90ed-c2edabb63b96", "8f6ba4eb-12a6-4958-a554-da4df2de5bb5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c4111660-4886-42a0-9722-2830bfac1cc7", "d2a440c0-732b-48da-b815-1105710ef153" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e34358a-343b-4db0-9e91-e5f399cacbd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ab35194-5997-4185-90ed-c2edabb63b96");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4111660-4886-42a0-9722-2830bfac1cc7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "129808f1-c20d-4fa1-a34a-689bc0b89f9a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8f6ba4eb-12a6-4958-a554-da4df2de5bb5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d2a440c0-732b-48da-b815-1105710ef153");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "4d30e315-1eeb-4ebf-93d4-b513d8d3bcb2");

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
