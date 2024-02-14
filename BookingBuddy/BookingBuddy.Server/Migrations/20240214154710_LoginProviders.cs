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
                    { "599b5787-f070-4d1d-8c91-17631c17bc7d", "local", "LOCAL" },
                    { "9d19f1a6-0a1a-478a-bcf9-02234f688367", "microsoft", "MICROSOFT" },
                    { "e2f08010-89a9-4225-8ddf-ab5f203e3494", "google", "GOOGLE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ac30bd48-cc51-4947-ad2e-9785aeabb38c", "d78d64f6-ff5e-4bf9-bca7-bd8c402d16e3", "landlord", "LANDLORD" },
                    { "af6f0000-a53d-4287-8d77-2b81c1a65d0d", "482674ba-d832-414d-b2ad-68571152157d", "user", "USER" },
                    { "e2eb363a-7bb4-4847-aff1-abbdd152fdfa", "5e8a3126-472b-4d27-9ee9-7aba0ff59e2a", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5266930d-952d-46bd-9fe3-495d7d66660e", 0, "a619f5ae-aaef-4211-be88-6ee1ee17ce4f", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAECx/ejlklKQ3t9LDtEjyOjoJw+Qzk2q4n0KgKRFcK26Ime4e1O050ahLlbP0I/rmjw==", null, false, null, "599b5787-f070-4d1d-8c91-17631c17bc7d", "e098a394-329d-476f-9f46-323f1b63d691", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9", 0, "5767ba9d-167e-42a8-b3ec-8501589ed7f0", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEFAlB/VVqc5VZXAvHDdyb/lp/GJvdZhfVJWhj2P43JTCsqrpMM1Y5CHyxof1NHFKxA==", null, false, null, "599b5787-f070-4d1d-8c91-17631c17bc7d", "693c4b38-bfb8-4e70-a718-d0962282add6", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "b7111238-968e-4b2f-8a49-9cc4969a5624", 0, "b7b2f876-b387-42b2-bf25-32b0d39634b2", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEN0bgrJ6qQsPY2pvqenJSsnqw1NAB8CpMTldTIciL3N3oyrIo8YZKIlm2vUm47vXiw==", null, false, null, "599b5787-f070-4d1d-8c91-17631c17bc7d", "0b65d11a-d2bf-4358-b950-095ad96f2fd4", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ac30bd48-cc51-4947-ad2e-9785aeabb38c", "5266930d-952d-46bd-9fe3-495d7d66660e" },
                    { "e2eb363a-7bb4-4847-aff1-abbdd152fdfa", "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9" },
                    { "af6f0000-a53d-4287-8d77-2b81c1a65d0d", "b7111238-968e-4b2f-8a49-9cc4969a5624" }
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
                keyValues: new object[] { "ac30bd48-cc51-4947-ad2e-9785aeabb38c", "5266930d-952d-46bd-9fe3-495d7d66660e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e2eb363a-7bb4-4847-aff1-abbdd152fdfa", "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "af6f0000-a53d-4287-8d77-2b81c1a65d0d", "b7111238-968e-4b2f-8a49-9cc4969a5624" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac30bd48-cc51-4947-ad2e-9785aeabb38c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af6f0000-a53d-4287-8d77-2b81c1a65d0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2eb363a-7bb4-4847-aff1-abbdd152fdfa");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5266930d-952d-46bd-9fe3-495d7d66660e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5af20bf1-d3b2-4515-9c4b-d2ac0bc5f9e9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b7111238-968e-4b2f-8a49-9cc4969a5624");

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
