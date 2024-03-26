using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class JMeterUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "0c6456a1-95fc-4c4b-9083-2f2b6cc76f6e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2a724a46-9e16-4843-8884-da5a0b9f4767");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "48505009-7d59-4612-8daa-3b352a0ae64e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "57b92f1a-5f10-4526-8d7a-82deb74a4d0c");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "57cb1ce1-6c30-47b0-9e30-3bc48209f15b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "6647ad35-34b6-4473-b1e4-6303e1894bfb");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a44cfb5c-7d5c-4f33-9209-7285bd5bea50");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "b7274cca-a8d4-4bfc-b9d5-d8adf208ffbc");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "bd4dd96b-8064-49dd-868a-af3141c6ad88");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "dcd962fd-05bb-41e7-bc97-d57e4de9ae7b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "e2846290-cc9e-47c5-bcf5-824738babddd");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "e5907ede-577f-471f-96a6-06288fe717a3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "faf8cea5-6984-4221-b248-f9e6563a3594");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "d50a2395-aa10-486c-8d3f-f895eea740e5");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "f21a2ab8-04ae-49c6-bd26-f1bd38b4cc90");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ce923822-d6ff-48ea-a3e2-11f29e5c2bfb", "80de2007-e6b2-47b2-8e2e-e52e80adee65" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6e5a3337-b945-4299-a94d-07c7a9c022de", "bf0511a0-e4fe-4700-8004-d18a3cd3f83e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3c7a28de-3fec-4bb1-ba21-3df2d87b599e", "f3a899f1-f2a7-49e0-842c-aa6ee654731c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c7a28de-3fec-4bb1-ba21-3df2d87b599e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e5a3337-b945-4299-a94d-07c7a9c022de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce923822-d6ff-48ea-a3e2-11f29e5c2bfb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80de2007-e6b2-47b2-8e2e-e52e80adee65");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bf0511a0-e4fe-4700-8004-d18a3cd3f83e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f3a899f1-f2a7-49e0-842c-aa6ee654731c");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "a4f158ad-5a48-4dda-8ee4-c2125eace1e7");

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "1050cd64-ed31-44ff-a9ba-3fb022ca885a", "Máquina de Lavar", "MaquinaLavar", null },
                    { "2f3a1f65-f036-4978-9d6f-5f031726664f", "Animais", "Animais", null },
                    { "40e65daa-e1e9-42f7-b4b9-586cf908cc6d", "Cozinha", "Cozinha", null },
                    { "4aade733-3dac-4ed6-bff0-387c6de6a074", "Quintal", "Quintal", null },
                    { "51742483-fbe7-412f-9e63-80899fa0d0a9", "TV", "Tv", null },
                    { "63e5c4bb-d273-48cd-94d7-988d323cd6c2", "Estacionamento", "Estacionamento", null },
                    { "6b567bfd-abc9-4e2d-8e83-57d5584e84b1", "Frigorífico", "Frigorifico", null },
                    { "71584ea6-42e4-4f90-ad66-def5f3212841", "Câmaras", "Camaras", null },
                    { "9f5fff27-78c7-4ffa-954a-4257116fb18c", "Varanda", "Varanda", null },
                    { "c714d783-075d-4729-959f-3008b0bb4fe5", "Piscina Individual", "PiscinaIndividual", null },
                    { "e3d3b9c9-4d8d-4797-82ae-36fbe302e191", "Wifi", "Wifi", null },
                    { "e545bf8f-692c-4b58-8991-e80a835f1bc5", "Microondas", "Microondas", null },
                    { "fb636f6a-4211-4760-a69f-53d231e83b4b", "Piscina Partilhada", "PiscinaPartilhada", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "761510fb-db82-4be1-97eb-20b0cc6795a8", "microsoft", "MICROSOFT" },
                    { "9848706e-c156-449f-af45-dcfa3b627723", "local", "LOCAL" },
                    { "fa148e27-6f4e-41f3-819b-85ff8a509dd6", "google", "GOOGLE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38cc8526-f041-4e97-a08e-d5cf0f5f4da0", "1ee8a9f1-1fdb-421e-889c-0d3af8978ea9", "admin", "ADMIN" },
                    { "84f3d03f-692f-470e-a453-ba650535c5a2", "0a11b321-287a-4419-b6ce-f50ef752d0ad", "landlord", "LANDLORD" },
                    { "86f1c591-208a-4a57-8ceb-87984399936d", "3eb08ce1-a530-42e3-94e3-688b54ee755e", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "18843d3e-356e-4540-83e2-2905b22f82d6", 0, "3a342290-72ea-4b3c-b1ab-5819a6eeefd3", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEPXoBeD8yh2+GFcf+OWsw60uZ3PVQuvlMHsH/iBJpJk9jxQpyJ0NQSr3PeupiUrw9A==", null, false, null, "9848706e-c156-449f-af45-dcfa3b627723", "25cb7365-7b5e-48c8-9b4a-4cc73126213d", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "3ed4274f-ff08-4c22-af32-f0dd27b38b3c", 0, "bcdef86a-d607-4853-9819-719cf93ea5c1", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEGnueQF/EF+fJuMi9PF09wEzm4D5Ixnpu55Ce3ufDINzDdcZIMQcsH+ik9sSZFlWxQ==", null, false, null, "9848706e-c156-449f-af45-dcfa3b627723", "2e9e7ef6-c720-42de-b030-b66aad3d951b", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "48c3ec0c-581f-43c1-9079-053dba8f2b16", 0, "e34e7e68-04b6-4ee6-ab4d-7e37f8b67746", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEBvBeTO5NPd3MdbQKh4oJsuq1wLVws+ObcpNPBHvYflc6LwNcoBqCoky2U++kJAGsQ==", null, false, null, "9848706e-c156-449f-af45-dcfa3b627723", "677fc775-0441-463a-9ff4-e31960ee0328", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "6a2be41a-5d4e-41a7-bc30-520013505a5f", 0, "4cb95860-d88e-49cd-a40f-1fc1e21a35f7", "bookingbuddy.jmeter@bookingbuddy.com", true, null, false, null, "JMeter", "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.JMETER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJGQ+kxYkhCpDrKithhOenRMGiztISzFIeJCt9g//Eqy16RnCS95UWhuY8DucxRS1Q==", null, false, null, "9848706e-c156-449f-af45-dcfa3b627723", "70a42016-2c2d-48f3-a1aa-72ba588c9fbc", false, "bookingbuddy.jmeter@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "84f3d03f-692f-470e-a453-ba650535c5a2", "18843d3e-356e-4540-83e2-2905b22f82d6" },
                    { "86f1c591-208a-4a57-8ceb-87984399936d", "3ed4274f-ff08-4c22-af32-f0dd27b38b3c" },
                    { "38cc8526-f041-4e97-a08e-d5cf0f5f4da0", "48c3ec0c-581f-43c1-9079-053dba8f2b16" },
                    { "38cc8526-f041-4e97-a08e-d5cf0f5f4da0", "6a2be41a-5d4e-41a7-bc30-520013505a5f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "1050cd64-ed31-44ff-a9ba-3fb022ca885a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2f3a1f65-f036-4978-9d6f-5f031726664f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "40e65daa-e1e9-42f7-b4b9-586cf908cc6d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "4aade733-3dac-4ed6-bff0-387c6de6a074");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "51742483-fbe7-412f-9e63-80899fa0d0a9");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "63e5c4bb-d273-48cd-94d7-988d323cd6c2");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "6b567bfd-abc9-4e2d-8e83-57d5584e84b1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "71584ea6-42e4-4f90-ad66-def5f3212841");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "9f5fff27-78c7-4ffa-954a-4257116fb18c");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c714d783-075d-4729-959f-3008b0bb4fe5");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "e3d3b9c9-4d8d-4797-82ae-36fbe302e191");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "e545bf8f-692c-4b58-8991-e80a835f1bc5");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "fb636f6a-4211-4760-a69f-53d231e83b4b");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "761510fb-db82-4be1-97eb-20b0cc6795a8");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "fa148e27-6f4e-41f3-819b-85ff8a509dd6");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "84f3d03f-692f-470e-a453-ba650535c5a2", "18843d3e-356e-4540-83e2-2905b22f82d6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "86f1c591-208a-4a57-8ceb-87984399936d", "3ed4274f-ff08-4c22-af32-f0dd27b38b3c" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "38cc8526-f041-4e97-a08e-d5cf0f5f4da0", "48c3ec0c-581f-43c1-9079-053dba8f2b16" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "38cc8526-f041-4e97-a08e-d5cf0f5f4da0", "6a2be41a-5d4e-41a7-bc30-520013505a5f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38cc8526-f041-4e97-a08e-d5cf0f5f4da0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84f3d03f-692f-470e-a453-ba650535c5a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86f1c591-208a-4a57-8ceb-87984399936d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "18843d3e-356e-4540-83e2-2905b22f82d6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ed4274f-ff08-4c22-af32-f0dd27b38b3c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "48c3ec0c-581f-43c1-9079-053dba8f2b16");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6a2be41a-5d4e-41a7-bc30-520013505a5f");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "9848706e-c156-449f-af45-dcfa3b627723");

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "0c6456a1-95fc-4c4b-9083-2f2b6cc76f6e", "Varanda", "Varanda", null },
                    { "2a724a46-9e16-4843-8884-da5a0b9f4767", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "48505009-7d59-4612-8daa-3b352a0ae64e", "Estacionamento", "Estacionamento", null },
                    { "57b92f1a-5f10-4526-8d7a-82deb74a4d0c", "Animais", "Animais", null },
                    { "57cb1ce1-6c30-47b0-9e30-3bc48209f15b", "Câmaras", "Camaras", null },
                    { "6647ad35-34b6-4473-b1e4-6303e1894bfb", "TV", "Tv", null },
                    { "a44cfb5c-7d5c-4f33-9209-7285bd5bea50", "Microondas", "Microondas", null },
                    { "b7274cca-a8d4-4bfc-b9d5-d8adf208ffbc", "Máquina de Lavar", "MaquinaLavar", null },
                    { "bd4dd96b-8064-49dd-868a-af3141c6ad88", "Cozinha", "Cozinha", null },
                    { "dcd962fd-05bb-41e7-bc97-d57e4de9ae7b", "Piscina Individual", "PiscinaIndividual", null },
                    { "e2846290-cc9e-47c5-bcf5-824738babddd", "Frigorífico", "Frigorifico", null },
                    { "e5907ede-577f-471f-96a6-06288fe717a3", "Quintal", "Quintal", null },
                    { "faf8cea5-6984-4221-b248-f9e6563a3594", "Wifi", "Wifi", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a4f158ad-5a48-4dda-8ee4-c2125eace1e7", "local", "LOCAL" },
                    { "d50a2395-aa10-486c-8d3f-f895eea740e5", "microsoft", "MICROSOFT" },
                    { "f21a2ab8-04ae-49c6-bd26-f1bd38b4cc90", "google", "GOOGLE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3c7a28de-3fec-4bb1-ba21-3df2d87b599e", "4c0e25c0-093b-4f0c-9dea-54db02e12c9b", "user", "USER" },
                    { "6e5a3337-b945-4299-a94d-07c7a9c022de", "de6b1dfe-2155-4620-a922-b9a7ba13aaf4", "landlord", "LANDLORD" },
                    { "ce923822-d6ff-48ea-a3e2-11f29e5c2bfb", "8d8bdfc3-8c1d-4e12-b956-9230bdb958bd", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "80de2007-e6b2-47b2-8e2e-e52e80adee65", 0, "e39f371a-7f10-4ec4-a5a6-a5c75b18bd4b", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEKRzCd8JV1yyJQZtSV6hXkGWftU6NDJ6nMUqGnP3SqIxpOgX41q2x0vQzM0tGUcdCQ==", null, false, null, "a4f158ad-5a48-4dda-8ee4-c2125eace1e7", "2daed617-b6d1-43ec-962f-ecbcccf3c53f", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "bf0511a0-e4fe-4700-8004-d18a3cd3f83e", 0, "4f54f5ae-b404-4494-bf70-9295e66284bd", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAECKQu5oVKaNe9bnmkfM7fpxoOYqWGXH9AYQQynmtDUhYh31djYR+8GA08Kb9wJUn9A==", null, false, null, "a4f158ad-5a48-4dda-8ee4-c2125eace1e7", "a3057396-0948-4dc7-8760-bbe338a1249d", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "f3a899f1-f2a7-49e0-842c-aa6ee654731c", 0, "f5d023eb-665a-4638-8c23-848d0e4c1129", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAECA1licH3pqzDcqiqdmpFp730dlV0dsDRdcHhx+sNAlPc60adKy5EQRDulXNSwVqHw==", null, false, null, "a4f158ad-5a48-4dda-8ee4-c2125eace1e7", "b28d44c9-616a-4b4a-8f91-b2087233950c", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ce923822-d6ff-48ea-a3e2-11f29e5c2bfb", "80de2007-e6b2-47b2-8e2e-e52e80adee65" },
                    { "6e5a3337-b945-4299-a94d-07c7a9c022de", "bf0511a0-e4fe-4700-8004-d18a3cd3f83e" },
                    { "3c7a28de-3fec-4bb1-ba21-3df2d87b599e", "f3a899f1-f2a7-49e0-842c-aa6ee654731c" }
                });
        }
    }
}
