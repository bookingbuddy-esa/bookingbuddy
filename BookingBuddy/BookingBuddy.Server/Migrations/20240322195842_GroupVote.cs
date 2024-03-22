using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class GroupVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "VotesId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GroupVote",
                columns: table => new
                {
                    VoteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupVote", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_GroupVote_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "AmenityId", "DisplayName", "Name", "PropertyId" },
                values: new object[,]
                {
                    { "03cc5c34-5a32-4998-a7ee-9e8c1bdd929f", "Wifi", "Wifi", null },
                    { "1b119d4b-edf3-4b86-8704-8a11c2622647", "Animais", "Animais", null },
                    { "1ea37b13-0498-489f-b9ca-93f06f54c107", "Cozinha", "Cozinha", null },
                    { "37fd8e33-0bcd-4478-9adf-2347c57e62a3", "Máquina de Lavar", "MaquinaLavar", null },
                    { "5df2d695-c64e-4acf-ba82-b3f4f8924ab5", "Câmaras", "Camaras", null },
                    { "75166b43-3ee6-48ed-89c7-32583632de2b", "Estacionamento", "Estacionamento", null },
                    { "8d36fdf4-ec90-4c7d-b8ae-8d9b9c06a63f", "Microondas", "Microondas", null },
                    { "95337898-be1a-49d1-b1ec-1eb6904c635d", "Piscina Partilhada", "PiscinaPartilhada", null },
                    { "a5292d64-9c6c-47e9-a269-603cbc4551db", "Varanda", "Varanda", null },
                    { "bb2435fe-ff2d-49e1-9f41-b8e45ed92e32", "TV", "Tv", null },
                    { "cff53275-4665-43f6-a549-93f250fd762e", "Piscina Individual", "PiscinaIndividual", null },
                    { "d7dbab50-94ca-42d6-819a-058d87589b0e", "Quintal", "Quintal", null },
                    { "f203b758-ce83-41f1-91b1-499cbb600afd", "Frigorífico", "Frigorifico", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "157bbae4-6e13-4edc-b636-8cdf61a1671d", "microsoft", "MICROSOFT" },
                    { "1dd78d4c-a2ae-4f1b-b325-fa055ba3de93", "google", "GOOGLE" },
                    { "86803fef-ef31-4f80-9350-a9885eeee21c", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13e010fa-4139-4f9c-afb8-6555e558b191", "fbabfe90-d3de-43e6-9af5-28c32fe4e49e", "admin", "ADMIN" },
                    { "37e624aa-e14b-44cb-8dae-f2f8675c8607", "92df6152-6e74-4058-8fab-b1860bc4cfcc", "user", "USER" },
                    { "3955027d-9d00-43a4-b20a-fb84071392b4", "5c25ea50-885a-4d61-99b2-06e05a85185e", "landlord", "LANDLORD" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "GroupBookingOrderOrderId", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2c338e26-829c-4f5d-b999-b9e392b70b09", 0, "c47264a6-dac5-4402-9ade-daf34eadc426", "bookingbuddy.user@bookingbuddy.com", true, null, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEGM+bLgQXl0NCvruv927qTBWaLJGwBbv9NCq00Oh2kig0rHjdC6dH12k8sJulX8qKw==", null, false, null, "86803fef-ef31-4f80-9350-a9885eeee21c", "a001a964-31b0-4eff-bd16-c39a5b0f7f46", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "7557a265-fa03-4e32-bf43-18a6bbaff8f9", 0, "8e3b0769-920f-429f-a4d4-893623461bb1", "bookingbuddy.landlord@bookingbuddy.com", true, null, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEH1sVkHuGi7GPJ+qM+zHreUDur/iHosqPwo3VWsQHVYsCZql9JntaghtQufyI1lmLg==", null, false, null, "86803fef-ef31-4f80-9350-a9885eeee21c", "f3cee27b-fafc-4d33-9f9d-bd5a22f38731", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "efff8980-9085-46bc-83f7-3b9d5a6d3c08", 0, "b7d85db0-6cea-4241-9a16-d1d1f72da7a8", "bookingbuddy.admin@bookingbuddy.com", true, null, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAENzxgF+lHQdSbWwhCGHQY05MEwuDByWi1TbgW9sbX7snfFVv5v0/cOrUPdsZ70IRKw==", null, false, null, "86803fef-ef31-4f80-9350-a9885eeee21c", "fea97c7b-ed87-4e8e-a37d-df7a21304e8c", false, "bookingbuddy.admin@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "37e624aa-e14b-44cb-8dae-f2f8675c8607", "2c338e26-829c-4f5d-b999-b9e392b70b09" },
                    { "3955027d-9d00-43a4-b20a-fb84071392b4", "7557a265-fa03-4e32-bf43-18a6bbaff8f9" },
                    { "13e010fa-4139-4f9c-afb8-6555e558b191", "efff8980-9085-46bc-83f7-3b9d5a6d3c08" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupVote_GroupId",
                table: "GroupVote",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupVote");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "03cc5c34-5a32-4998-a7ee-9e8c1bdd929f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "1b119d4b-edf3-4b86-8704-8a11c2622647");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "1ea37b13-0498-489f-b9ca-93f06f54c107");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "37fd8e33-0bcd-4478-9adf-2347c57e62a3");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "5df2d695-c64e-4acf-ba82-b3f4f8924ab5");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "75166b43-3ee6-48ed-89c7-32583632de2b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "8d36fdf4-ec90-4c7d-b8ae-8d9b9c06a63f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "95337898-be1a-49d1-b1ec-1eb6904c635d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a5292d64-9c6c-47e9-a269-603cbc4551db");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "bb2435fe-ff2d-49e1-9f41-b8e45ed92e32");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "cff53275-4665-43f6-a549-93f250fd762e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "d7dbab50-94ca-42d6-819a-058d87589b0e");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f203b758-ce83-41f1-91b1-499cbb600afd");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "157bbae4-6e13-4edc-b636-8cdf61a1671d");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "1dd78d4c-a2ae-4f1b-b325-fa055ba3de93");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "37e624aa-e14b-44cb-8dae-f2f8675c8607", "2c338e26-829c-4f5d-b999-b9e392b70b09" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3955027d-9d00-43a4-b20a-fb84071392b4", "7557a265-fa03-4e32-bf43-18a6bbaff8f9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "13e010fa-4139-4f9c-afb8-6555e558b191", "efff8980-9085-46bc-83f7-3b9d5a6d3c08" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13e010fa-4139-4f9c-afb8-6555e558b191");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37e624aa-e14b-44cb-8dae-f2f8675c8607");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3955027d-9d00-43a4-b20a-fb84071392b4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c338e26-829c-4f5d-b999-b9e392b70b09");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7557a265-fa03-4e32-bf43-18a6bbaff8f9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "efff8980-9085-46bc-83f7-3b9d5a6d3c08");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "86803fef-ef31-4f80-9350-a9885eeee21c");

            migrationBuilder.DropColumn(
                name: "VotesId",
                table: "Groups");

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
    }
}
