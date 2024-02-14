using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class DefaultUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedDate_Property_PropertyId",
                table: "BlockedDate");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_AspNetUsers_ApplicationUserId",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Property_ApplicationUserId",
                table: "Property");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyId",
                table: "BlockedDate",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedDate_Property_PropertyId",
                table: "BlockedDate",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedDate_Property_PropertyId",
                table: "BlockedDate");

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

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Property",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyId",
                table: "BlockedDate",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Property_ApplicationUserId",
                table: "Property",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedDate_Property_PropertyId",
                table: "BlockedDate",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_AspNetUsers_ApplicationUserId",
                table: "Property",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
