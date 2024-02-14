using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class LoginProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "562edf48-1874-4165-97ff-0634b3ad2c8a", "8b93731f-bccc-437c-afc6-6d8b8521d8e2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5e968922-a8ba-4698-bb00-bb60f0564813", "c9a650c7-d1f7-4610-b051-f0ff2be04898" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "aeceba13-bfba-4505-bb4e-19d342ab28ce", "fa7b6f8e-7d77-42fb-a091-f4943a7c98f3" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "562edf48-1874-4165-97ff-0634b3ad2c8a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e968922-a8ba-4698-bb00-bb60f0564813");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aeceba13-bfba-4505-bb4e-19d342ab28ce");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8b93731f-bccc-437c-afc6-6d8b8521d8e2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9a650c7-d1f7-4610-b051-f0ff2be04898");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fa7b6f8e-7d77-42fb-a091-f4943a7c98f3");

            migrationBuilder.AddColumn<string>(
                name: "ProviderId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AspNetProviders",
                columns: table => new
                {
                    AspNetProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetProviders", x => x.AspNetProviderId);
                });

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b8b2eb0-a85d-4054-8e83-7711ff05b931", "Google", "GOOGLE" },
                    { "841527c5-f63d-455b-a0b2-bd608bcdf5a2", "Microsoft", "MICROSOFT" },
                    { "e5e97d95-b506-43dd-9539-7e1053a5a1a0", "Local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "779e6b8f-1ca5-4432-80a4-1d5100aa6bc2", "8aa79a63-9d30-48a7-8a47-04df9ad0fd09", "admin", "ADMIN" },
                    { "88282e62-1b83-44ef-aa89-be90b85a61dd", "46c5c526-fe01-4d36-93b2-eda7f30127a9", "landlord", "LANDLORD" },
                    { "b4e2e8f3-652f-422b-8f2b-d8b436c25b18", "1d5d3b05-b731-4de3-9c06-6dbf9eaffc72", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "7555779c-065f-4736-83da-05997bc59354", 0, "9c5ed9ad-06d1-40d4-9317-2752fdfbab05", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEAYXakg4+YuNqcDbLLwsAi8brrfh4iLnkJQCp7JgdhRtxPdcXRhCPEBhkXcTb3vvwA==", null, false, null, "e5e97d95-b506-43dd-9539-7e1053a5a1a0", "bbbfa965-474b-45d8-8919-77721572d0c1", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "fa817682-9a5a-4c8f-8ab5-7bd5d3b1172e", 0, "70496d6b-571f-4db7-8dee-7038994d7043", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEEadtHKyQ7HVC8ychTl9Kkf3MggoIXtXtNASICeRTXS6zFx3VPsH/qIfQqeuU0pnvQ==", null, false, null, "e5e97d95-b506-43dd-9539-7e1053a5a1a0", "2791edb8-2f20-4740-bbf4-3b92403f8fa2", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "fb988bbe-bd4e-4b58-a56a-8f0a3b08aa09", 0, "b4a2a1a7-a4c5-4479-af8c-3104cc684625", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJFQETAhDJKYtlrdLvfFU9/uPPWUEcRJg5FTaEDmsEYBQpnWXXUp+4Itae/ZY+sDIg==", null, false, null, "e5e97d95-b506-43dd-9539-7e1053a5a1a0", "d0f59353-b2bf-4fbd-8b3a-496ca76ee087", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "b4e2e8f3-652f-422b-8f2b-d8b436c25b18", "7555779c-065f-4736-83da-05997bc59354" },
                    { "779e6b8f-1ca5-4432-80a4-1d5100aa6bc2", "fa817682-9a5a-4c8f-8ab5-7bd5d3b1172e" },
                    { "88282e62-1b83-44ef-aa89-be90b85a61dd", "fb988bbe-bd4e-4b58-a56a-8f0a3b08aa09" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProviderId",
                table: "AspNetUsers",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetProviders_ProviderId",
                table: "AspNetUsers",
                column: "ProviderId",
                principalTable: "AspNetProviders",
                principalColumn: "AspNetProviderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetProviders_ProviderId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetProviders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProviderId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b4e2e8f3-652f-422b-8f2b-d8b436c25b18", "7555779c-065f-4736-83da-05997bc59354" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "779e6b8f-1ca5-4432-80a4-1d5100aa6bc2", "fa817682-9a5a-4c8f-8ab5-7bd5d3b1172e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "88282e62-1b83-44ef-aa89-be90b85a61dd", "fb988bbe-bd4e-4b58-a56a-8f0a3b08aa09" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "779e6b8f-1ca5-4432-80a4-1d5100aa6bc2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88282e62-1b83-44ef-aa89-be90b85a61dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4e2e8f3-652f-422b-8f2b-d8b436c25b18");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7555779c-065f-4736-83da-05997bc59354");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fa817682-9a5a-4c8f-8ab5-7bd5d3b1172e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fb988bbe-bd4e-4b58-a56a-8f0a3b08aa09");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "562edf48-1874-4165-97ff-0634b3ad2c8a", null, "landlord", "LANDLORD" },
                    { "5e968922-a8ba-4698-bb00-bb60f0564813", null, "admin", "ADMIN" },
                    { "aeceba13-bfba-4505-bb4e-19d342ab28ce", null, "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8b93731f-bccc-437c-afc6-6d8b8521d8e2", 0, "8ee37cfa-c293-4ac6-8f90-24a5a67419ff", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEMWJ+hRFeiwBxiFBHh+2+y+LLQ7cbBMacKTAyjDdV5pZZJ/3XraJxRxPMw/IXgzdfQ==", null, false, null, "3cd749a9-fcf7-4886-a733-156801256d41", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "c9a650c7-d1f7-4610-b051-f0ff2be04898", 0, "38cbf367-b800-4266-bf53-fe7469eeaa4a", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEC/tbqJCOGtlsizcSh6JoJx/dp+WtZGNSkuzPmypBEjiiOw99QGbUfU17kMtBF2AQw==", null, false, null, "92209098-2e16-473f-86f1-c1055ccb779c", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "fa7b6f8e-7d77-42fb-a091-f4943a7c98f3", 0, "757ecdc5-564c-40d0-9599-b1f505e964d1", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEAM8KgkdWcPYaGf5Auh1h3Ce1HoL9PqXf53g30VjPmprn+QfuMjrJNzRxzo8/JXTAg==", null, false, null, "a2063ec4-0bbe-4fda-8ef9-6b356bc42d41", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "562edf48-1874-4165-97ff-0634b3ad2c8a", "8b93731f-bccc-437c-afc6-6d8b8521d8e2" },
                    { "5e968922-a8ba-4698-bb00-bb60f0564813", "c9a650c7-d1f7-4610-b051-f0ff2be04898" },
                    { "aeceba13-bfba-4505-bb4e-19d342ab28ce", "fa7b6f8e-7d77-42fb-a091-f4943a7c98f3" }
                });
        }
    }
}
