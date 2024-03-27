using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class RoomsAndGuestsNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "0f806ded-a234-4bc5-9d58-a9a5cb0779e1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2f1e2063-2ae0-44c0-bc5d-f5f33c1e388b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "3864f6d3-e1ef-4d48-bc59-9691e0abcfbb");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "58eaa0ab-bb91-4f91-aa15-f6a7eb050996");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "84d9cc6a-6094-4e78-a2aa-197be56d4d00");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "85f6680a-2a15-4a12-ab96-c344ed41278f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "8a6d80c8-56e4-4f18-a132-7b86b59b0cfb");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "934de990-0c67-458a-861a-0fea501a2cb9");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "9ff24cc1-07e4-4f1d-809e-a2e19e6b247e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a35b4bad-1de6-4bf8-84b9-eed1b17a0b81");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a5c5f972-9d24-4ac0-81db-f5547885173a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "fdb6529e-e156-4b87-8b25-b44dd59b2430");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "ffad6d62-94c5-41ad-848f-b58d6c13b880");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "2280fc42-2f7c-4838-90ea-d6cc16bab036");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "44c7cbe9-0da0-45fb-ab98-0c94ba6aa450");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e8c90d93-0d15-41e2-8714-2dcfeefaff9a", "4ea0a0ae-b9e0-46ab-810e-35d0939b6226" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bd62d2b7-116d-4fcd-ac7f-cf18299b960d", "75484460-addb-495a-b718-9ddc40daf59d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bd62d2b7-116d-4fcd-ac7f-cf18299b960d", "d4913ec6-098b-4533-973b-4854b7049e77" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "24d946fa-e52e-4c0d-94fd-eec332d465b3", "d5d42131-ba96-487c-8ccf-0eb49c17eb1c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24d946fa-e52e-4c0d-94fd-eec332d465b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd62d2b7-116d-4fcd-ac7f-cf18299b960d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8c90d93-0d15-41e2-8714-2dcfeefaff9a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4ea0a0ae-b9e0-46ab-810e-35d0939b6226");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "75484460-addb-495a-b718-9ddc40daf59d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4913ec6-098b-4533-973b-4854b7049e77");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5d42131-ba96-487c-8ccf-0eb49c17eb1c");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "ae0a7e55-396c-4835-8f55-217851e39c13");

            migrationBuilder.AddColumn<int>(
                name: "MaxGuestsNumber",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomsNumber",
                table: "Property",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "06814766-6a09-49b3-95ac-535f155c64ba", "TV", "Tv", null },
                    { "0957f10a-55e4-4b7c-aa87-3f57af619f0a", "Câmaras", "Camaras", null },
                    { "10c6c31b-5fc3-4768-a888-6046dbb450e2", "Estacionamento", "Estacionamento", null },
                    { "22b214a7-4cc0-4da2-90c8-3fa0462e20a3", "Wifi", "Wifi", null },
                    { "23f7acbf-2b68-4f7a-be66-c8cc18a1fcfb", "Quintal", "Quintal", null },
                    { "24fb5ffa-9d96-448e-9cc4-bb44773cd6ee", "Frigorífico", "Frigorifico", null },
                    { "2669a8ab-2ac9-4573-b478-01d7dd5030df", "Animais", "Animais", null },
                    { "6258aa2c-0239-4015-a547-fc2a163ff03f", "Varanda", "Varanda", null },
                    { "714a09b5-cc56-47ea-a003-da36666e71ef", "Microondas", "Microondas", null },
                    { "77d489bf-5f71-4069-906d-587477aa67b8", "Máquina de Lavar", "MaquinaLavar", null },
                    { "bcf831b8-548a-49c5-b8df-27b0e84fafd3", "Piscina Individual", "PiscinaIndividual", null },
                    { "cb1011ec-93f5-40d1-b2e4-10eba72f0e31", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "cb3d8ee1-6710-47cd-aa81-ac5ac1d47bf0", "Cozinha", "Cozinha", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11c595c2-7076-4bef-8835-e35d78b51f94", "google", "GOOGLE" },
                    { "7386f0a7-aaee-4fac-b44b-de6df0006eb3", "local", "LOCAL" },
                    { "c51be352-1122-4614-96d0-20629f6b4c64", "microsoft", "MICROSOFT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a52673e-e53c-473d-84d5-cc02f2c56563", "8ee49679-5a8b-46b7-9962-25d427d90332", "admin", "ADMIN" },
                    { "35ae2d5b-d108-4c7b-a44e-c8e448d11d48", "bcbe84c0-8800-4b4b-a50d-d8d9e3aba108", "user", "USER" },
                    { "7a4eaf82-57e3-4c0a-8299-7698a7631f76", "94fcec69-3856-439c-a8ec-2bb20e5ba467", "landlord", "LANDLORD" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Description", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6d4a2bed-7573-48af-b781-1fc5c841cac5", 0, "cd8e5005-8edc-43de-ae06-da53515c334c", "Esta é a descrição da conta de teste do JMeter.", "bookingbuddy.jmeter@bookingbuddy.com", true, null, false, null, "JMeter", "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEIItUR2MHdtQSt/BLeBsWSeum+TGbx7HKF9rQncncf4t6ah/MYd6Um85nyNM/n4ztA==", null, false, null, "7386f0a7-aaee-4fac-b44b-de6df0006eb3", "df37b3a1-1d86-431f-bf3f-1843c8aef578", false, "bookingbuddy.jmeter@bookingbuddy.com" },
                    { "6e9e2839-df6d-466f-9e37-f95b295243f3", 0, "89273dc9-ee5e-4c0c-8ca8-391260ad5327", "Esta é descrição da conta de administrador do BookingBuddy.", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEHKlNy1IVBfq5EArh7HBDvNpEYmGoMgL1Myu85yaU42dKpEP/KRzfjje+68NmNhEpw==", null, false, null, "7386f0a7-aaee-4fac-b44b-de6df0006eb3", "97ae916c-c6a4-42b2-a29a-6814fe83e523", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "a3bce16f-3455-48f4-a526-47947d9f3ef5", 0, "cf29fddc-3572-4d9c-a9ee-ca6cd6a64901", "Esta é descrição da conta de utilizador (padrão) do BookingBuddy.", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEKZCvT0ozpOmQlq430EIrWTeRz0qiEAw0ZgXWjKO4/YohrQziYWv+wP18aHGIOMlEQ==", null, false, null, "7386f0a7-aaee-4fac-b44b-de6df0006eb3", "9918a8ea-ed60-4850-a9cf-f1511806d8aa", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "dfd4f06f-3940-4e3f-970f-5c6116126178", 0, "6eb9d001-df86-40d6-bdd4-b0b9ad6d0e7a", "Esta é a descrição da conta de proprietário do BookingBuddy.", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEKME2tV/AI7JjZV3PFLzU9S3Y6WzzpugLk3C6eGF8f6R/oEYIfIXMM1p3fS4ryT2KA==", null, false, null, "7386f0a7-aaee-4fac-b44b-de6df0006eb3", "30d61053-e3c2-424b-918d-d929fe122af9", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "0a52673e-e53c-473d-84d5-cc02f2c56563", "6d4a2bed-7573-48af-b781-1fc5c841cac5" },
                    { "0a52673e-e53c-473d-84d5-cc02f2c56563", "6e9e2839-df6d-466f-9e37-f95b295243f3" },
                    { "35ae2d5b-d108-4c7b-a44e-c8e448d11d48", "a3bce16f-3455-48f4-a526-47947d9f3ef5" },
                    { "7a4eaf82-57e3-4c0a-8299-7698a7631f76", "dfd4f06f-3940-4e3f-970f-5c6116126178" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "06814766-6a09-49b3-95ac-535f155c64ba");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "0957f10a-55e4-4b7c-aa87-3f57af619f0a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "10c6c31b-5fc3-4768-a888-6046dbb450e2");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "22b214a7-4cc0-4da2-90c8-3fa0462e20a3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "23f7acbf-2b68-4f7a-be66-c8cc18a1fcfb");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "24fb5ffa-9d96-448e-9cc4-bb44773cd6ee");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2669a8ab-2ac9-4573-b478-01d7dd5030df");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "6258aa2c-0239-4015-a547-fc2a163ff03f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "714a09b5-cc56-47ea-a003-da36666e71ef");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "77d489bf-5f71-4069-906d-587477aa67b8");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "bcf831b8-548a-49c5-b8df-27b0e84fafd3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "cb1011ec-93f5-40d1-b2e4-10eba72f0e31");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "cb3d8ee1-6710-47cd-aa81-ac5ac1d47bf0");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "11c595c2-7076-4bef-8835-e35d78b51f94");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "c51be352-1122-4614-96d0-20629f6b4c64");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0a52673e-e53c-473d-84d5-cc02f2c56563", "6d4a2bed-7573-48af-b781-1fc5c841cac5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0a52673e-e53c-473d-84d5-cc02f2c56563", "6e9e2839-df6d-466f-9e37-f95b295243f3" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "35ae2d5b-d108-4c7b-a44e-c8e448d11d48", "a3bce16f-3455-48f4-a526-47947d9f3ef5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7a4eaf82-57e3-4c0a-8299-7698a7631f76", "dfd4f06f-3940-4e3f-970f-5c6116126178" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a52673e-e53c-473d-84d5-cc02f2c56563");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35ae2d5b-d108-4c7b-a44e-c8e448d11d48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a4eaf82-57e3-4c0a-8299-7698a7631f76");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d4a2bed-7573-48af-b781-1fc5c841cac5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e9e2839-df6d-466f-9e37-f95b295243f3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a3bce16f-3455-48f4-a526-47947d9f3ef5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dfd4f06f-3940-4e3f-970f-5c6116126178");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "7386f0a7-aaee-4fac-b44b-de6df0006eb3");

            migrationBuilder.DropColumn(
                name: "MaxGuestsNumber",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "RoomsNumber",
                table: "Property");

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "0f806ded-a234-4bc5-9d58-a9a5cb0779e1", "Wifi", "Wifi", null },
                    { "2f1e2063-2ae0-44c0-bc5d-f5f33c1e388b", "Piscina Individual", "PiscinaIndividual", null },
                    { "3864f6d3-e1ef-4d48-bc59-9691e0abcfbb", "Câmaras", "Camaras", null },
                    { "58eaa0ab-bb91-4f91-aa15-f6a7eb050996", "Quintal", "Quintal", null },
                    { "84d9cc6a-6094-4e78-a2aa-197be56d4d00", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "85f6680a-2a15-4a12-ab96-c344ed41278f", "Frigorífico", "Frigorifico", null },
                    { "8a6d80c8-56e4-4f18-a132-7b86b59b0cfb", "Máquina de Lavar", "MaquinaLavar", null },
                    { "934de990-0c67-458a-861a-0fea501a2cb9", "Microondas", "Microondas", null },
                    { "9ff24cc1-07e4-4f1d-809e-a2e19e6b247e", "Estacionamento", "Estacionamento", null },
                    { "a35b4bad-1de6-4bf8-84b9-eed1b17a0b81", "Cozinha", "Cozinha", null },
                    { "a5c5f972-9d24-4ac0-81db-f5547885173a", "TV", "Tv", null },
                    { "fdb6529e-e156-4b87-8b25-b44dd59b2430", "Varanda", "Varanda", null },
                    { "ffad6d62-94c5-41ad-848f-b58d6c13b880", "Animais", "Animais", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2280fc42-2f7c-4838-90ea-d6cc16bab036", "microsoft", "MICROSOFT" },
                    { "44c7cbe9-0da0-45fb-ab98-0c94ba6aa450", "google", "GOOGLE" },
                    { "ae0a7e55-396c-4835-8f55-217851e39c13", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24d946fa-e52e-4c0d-94fd-eec332d465b3", "7795494a-65b8-4698-b050-02d5993ba593", "landlord", "LANDLORD" },
                    { "bd62d2b7-116d-4fcd-ac7f-cf18299b960d", "34d52b55-4964-4efd-9579-32beca7a1919", "admin", "ADMIN" },
                    { "e8c90d93-0d15-41e2-8714-2dcfeefaff9a", "082fe8b4-b7ee-4fce-aa2e-b56f2fa66b0f", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Description", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4ea0a0ae-b9e0-46ab-810e-35d0939b6226", 0, "65bd4a24-84f4-46cc-af76-1386247018b7", "Esta é descrição da conta de utilizador (padrão) do BookingBuddy.", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEFG/tkTbq6/To0SENnF1CfMDQq01V9l/3c0tE0G3nQAsCExkdl3fAK2FZ+8GH992rw==", null, false, null, "ae0a7e55-396c-4835-8f55-217851e39c13", "6584214e-de09-49f1-afdf-ac34b65e1ac4", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "75484460-addb-495a-b718-9ddc40daf59d", 0, "04fdc3ca-3e33-4aa4-bfbf-b3a56d78c514", "Esta é descrição da conta de administrador do BookingBuddy.", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEKsXClQmyU6aI40uIYGquiuUmhIPqdEKz14QYohUxxayrOIr5t+yoU2SSirsa2jrDQ==", null, false, null, "ae0a7e55-396c-4835-8f55-217851e39c13", "a1bb3fae-6505-4a3f-ad0e-bcc6316d7e29", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "d4913ec6-098b-4533-973b-4854b7049e77", 0, "5a8a56e2-a675-4fc6-8cfe-0c3e2c7993e2", "Esta é a descrição da conta de teste do JMeter.", "bookingbuddy.jmeter@bookingbuddy.com", true, null, false, null, "JMeter", "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEMDV3/fqrrQhgVaAtJ9KLPD8Oq4jNx5Xqm0OFsP1R1fE6rkIAKUXpHmcbbbpysLH/Q==", null, false, null, "ae0a7e55-396c-4835-8f55-217851e39c13", "244009c0-8710-4c1f-9ec2-0112be3b975e", false, "bookingbuddy.jmeter@bookingbuddy.com" },
                    { "d5d42131-ba96-487c-8ccf-0eb49c17eb1c", 0, "be8e98d1-66ad-4183-8f22-870b75ed8c85", "Esta é a descrição da conta de proprietário do BookingBuddy.", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEEyZaGv6cH4zxqhOkc+pQ76RMzwu6RR076SUjj7vK6E5Ghzo99vbPNUmYwRi7mZoZg==", null, false, null, "ae0a7e55-396c-4835-8f55-217851e39c13", "00c888cf-31f3-4b07-9754-9fb0bb76c038", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "e8c90d93-0d15-41e2-8714-2dcfeefaff9a", "4ea0a0ae-b9e0-46ab-810e-35d0939b6226" },
                    { "bd62d2b7-116d-4fcd-ac7f-cf18299b960d", "75484460-addb-495a-b718-9ddc40daf59d" },
                    { "bd62d2b7-116d-4fcd-ac7f-cf18299b960d", "d4913ec6-098b-4533-973b-4854b7049e77" },
                    { "24d946fa-e52e-4c0d-94fd-eec332d465b3", "d5d42131-ba96-487c-8ccf-0eb49c17eb1c" }
                });
        }
    }
}
