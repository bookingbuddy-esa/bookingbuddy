using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class GroupAction : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "GroupAction",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "080bec24-ed41-4ceb-a34a-7f3c5feec8b1", "Frigorífico", "Frigorifico", null },
                    { "0ad0b821-d199-413b-b76a-58530d302121", "Wifi", "Wifi", null },
                    { "2986bf18-0dd5-4c5c-8ca2-63794f0337d4", "Piscina Individual", "PiscinaIndividual", null },
                    { "4fd6c4ba-0e5e-4c11-93a5-5dae75554299", "Máquina de Lavar", "MaquinaLavar", null },
                    { "60484f24-9ff0-4bc4-998c-f1483f27a553", "Cozinha", "Cozinha", null },
                    { "86620353-69b2-4742-a67c-4ceec52e7950", "Quintal", "Quintal", null },
                    { "89e8556f-0e64-425b-85d5-fefb8f875965", "Microondas", "Microondas", null },
                    { "a72d42b8-945d-4b3d-bd7e-3bce4a52800d", "Varanda", "Varanda", null },
                    { "c6491ef1-7952-4eb4-b980-aef82bbb4799", "TV", "Tv", null },
                    { "c9d088f5-0814-46d0-9e2b-11dcea9985d2", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "dfcdf6c4-4b8f-4249-9ea6-8887464fe617", "Estacionamento", "Estacionamento", null },
                    { "f54a3cb4-2467-4da9-b76d-7a40a1d0f4b5", "Câmaras", "Camaras", null },
                    { "f6e6abed-2392-40e0-b407-437d6b89ad51", "Animais", "Animais", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e659a58-e3c9-41ff-8587-b74424ebbf3a", "microsoft", "MICROSOFT" },
                    { "47828d77-ab7f-4e07-9965-7d76b9c6a6f7", "google", "GOOGLE" },
                    { "bb84b905-521e-43f7-9109-3f11bae13b61", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ac02251-a8c1-4efd-bb68-1563b12971e9", "789d97b3-9ce0-4df2-89fb-ba5dc72d3ddb", "user", "USER" },
                    { "49660557-31df-44d2-a73e-967563cddcae", "88bc0921-2cc4-445c-9264-ab2ad1401eac", "landlord", "LANDLORD" },
                    { "ffe0180e-dc28-4ad8-9fa8-6db0d101100e", "ceceb778-32a4-471c-9168-b758b46ce030", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4bc9364d-95b9-43c0-a155-47b1cb2e521f", 0, "46261962-ede5-44a9-8dd8-c4950e705519", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEKSy/bAd+qPvYYY3Dar1kh991h2CnBD27iYN9XBMhn4kIOkVsHk/JAakls9FK3o4ng==", null, false, null, "bb84b905-521e-43f7-9109-3f11bae13b61", "17789ad4-fef2-4faf-bb64-8b7cfa0efd2c", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "72ac50f2-d54b-48c9-ab43-6efa4a6bb63e", 0, "e0a8d676-ce5b-4cc5-a4ba-cecdeafb3b42", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEMZqlQwWm3bJbD5eRL6ksNZtRC57SzRbf+gDwyAcmQ9pPnxOvWSSt4QA3Ch5gSrZ8w==", null, false, null, "bb84b905-521e-43f7-9109-3f11bae13b61", "b1ce909b-23cf-4cb4-9da7-5ad0ef2aebef", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "ed784e11-f95d-46e2-8c42-4503aaaf888b", 0, "4f33d362-58a6-4747-81f6-1820d5f1e79a", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEOQ2DRnChdORW437iebeXS8Sxue6NFrMtj2q9UA2J4YKKv0W0Wr5beOIfy3BbFDKXw==", null, false, null, "bb84b905-521e-43f7-9109-3f11bae13b61", "0cd4e3be-d6b1-4921-b694-b6e43b2b984b", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ffe0180e-dc28-4ad8-9fa8-6db0d101100e", "4bc9364d-95b9-43c0-a155-47b1cb2e521f" },
                    { "2ac02251-a8c1-4efd-bb68-1563b12971e9", "72ac50f2-d54b-48c9-ab43-6efa4a6bb63e" },
                    { "49660557-31df-44d2-a73e-967563cddcae", "ed784e11-f95d-46e2-8c42-4503aaaf888b" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "080bec24-ed41-4ceb-a34a-7f3c5feec8b1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "0ad0b821-d199-413b-b76a-58530d302121");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2986bf18-0dd5-4c5c-8ca2-63794f0337d4");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "4fd6c4ba-0e5e-4c11-93a5-5dae75554299");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "60484f24-9ff0-4bc4-998c-f1483f27a553");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "86620353-69b2-4742-a67c-4ceec52e7950");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "89e8556f-0e64-425b-85d5-fefb8f875965");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a72d42b8-945d-4b3d-bd7e-3bce4a52800d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c6491ef1-7952-4eb4-b980-aef82bbb4799");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c9d088f5-0814-46d0-9e2b-11dcea9985d2");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "dfcdf6c4-4b8f-4249-9ea6-8887464fe617");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f54a3cb4-2467-4da9-b76d-7a40a1d0f4b5");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f6e6abed-2392-40e0-b407-437d6b89ad51");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "1e659a58-e3c9-41ff-8587-b74424ebbf3a");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "47828d77-ab7f-4e07-9965-7d76b9c6a6f7");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ffe0180e-dc28-4ad8-9fa8-6db0d101100e", "4bc9364d-95b9-43c0-a155-47b1cb2e521f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2ac02251-a8c1-4efd-bb68-1563b12971e9", "72ac50f2-d54b-48c9-ab43-6efa4a6bb63e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "49660557-31df-44d2-a73e-967563cddcae", "ed784e11-f95d-46e2-8c42-4503aaaf888b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ac02251-a8c1-4efd-bb68-1563b12971e9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49660557-31df-44d2-a73e-967563cddcae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffe0180e-dc28-4ad8-9fa8-6db0d101100e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4bc9364d-95b9-43c0-a155-47b1cb2e521f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "72ac50f2-d54b-48c9-ab43-6efa4a6bb63e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed784e11-f95d-46e2-8c42-4503aaaf888b");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "bb84b905-521e-43f7-9109-3f11bae13b61");

            migrationBuilder.DropColumn(
                name: "GroupAction",
                table: "Groups");

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
