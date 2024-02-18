using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class Payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "1ea9abb9-f0ee-456f-9d80-5c75d95ca7a1");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "4b8bcf90-dfa6-4394-9d53-9686cd0e9ed9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "42630d5b-ba89-4955-a36a-e31dd8cf7705", "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "acb67e3b-29f7-409a-af25-8dac9168fe56", "2de95519-31eb-459e-8ad1-5021989213f5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "61656fea-ed23-460c-bed2-acae223beeef", "48996947-a573-4470-8d32-7797a2bac0e5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42630d5b-ba89-4955-a36a-e31dd8cf7705");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61656fea-ed23-460c-bed2-acae223beeef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acb67e3b-29f7-409a-af25-8dac9168fe56");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2de95519-31eb-459e-8ad1-5021989213f5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "48996947-a573-4470-8d32-7797a2bac0e5");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "4b915b1f-8b74-45cf-979e-b10f8b98f919");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Payment",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "255ff096-e66e-42cd-8770-4e0f573f36d8", "google", "GOOGLE" },
                    { "396401a6-194f-4dc8-af2d-a14ecc22586e", "microsoft", "MICROSOFT" },
                    { "fea16581-01e7-42aa-8394-a7430f4e63af", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "67bb1c77-333c-41c9-b0e2-8fca172132ad", "be1c835b-f38d-4d44-9b83-696d703a0049", "landlord", "LANDLORD" },
                    { "719de307-49bd-49ac-bfd5-773a68435e4f", "35ef4be8-b043-428d-91ba-f9a7dfa3c3de", "user", "USER" },
                    { "bee65ac6-98c7-4c0b-89a6-80ea12bffe43", "97c0582e-f294-4347-bdfe-86ad028fc5d3", "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0729feac-6b11-483c-8424-40003dbbd6f6", 0, "2252f022-98ec-4245-96cb-02d809126260", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEKutLISSzrcHCaYdLKQoBYONv9S4tXqCZ8Yc5TpWCdPzKS/m+m59BuV8E7q99vJVbA==", null, false, null, "fea16581-01e7-42aa-8394-a7430f4e63af", "beb055af-562b-4b75-b60d-c062892b689f", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "156c9391-1ebc-4ad1-acf8-37a70a5fa01a", 0, "922bbb01-6817-4cf4-bc14-cbc71598492f", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEHxJM8LE+l1FRobLOhGQa3GB7C+Hp+MLAD7b6EfU8f+94iAed+IQEyiW89Y3ndeMOw==", null, false, null, "fea16581-01e7-42aa-8394-a7430f4e63af", "8a9c24d7-11bd-446f-946f-63f6462c8329", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "9a3b52e3-6972-46e0-b3a6-ea0d8a9f4bdd", 0, "38d2a501-e410-431e-8344-6ab5d805cca1", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEAll09Ho5Ep7U/rr+YmIzaVKVSVNmYDG+jb84cwQYhU1ulggPVXUV5FEq1TfwIx1kg==", null, false, null, "fea16581-01e7-42aa-8394-a7430f4e63af", "e5edf62f-db31-4582-b27e-45189483d229", false, "bookingbuddy.admin@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "67bb1c77-333c-41c9-b0e2-8fca172132ad", "0729feac-6b11-483c-8424-40003dbbd6f6" },
                    { "719de307-49bd-49ac-bfd5-773a68435e4f", "156c9391-1ebc-4ad1-acf8-37a70a5fa01a" },
                    { "bee65ac6-98c7-4c0b-89a6-80ea12bffe43", "9a3b52e3-6972-46e0-b3a6-ea0d8a9f4bdd" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "255ff096-e66e-42cd-8770-4e0f573f36d8");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "396401a6-194f-4dc8-af2d-a14ecc22586e");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "67bb1c77-333c-41c9-b0e2-8fca172132ad", "0729feac-6b11-483c-8424-40003dbbd6f6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "719de307-49bd-49ac-bfd5-773a68435e4f", "156c9391-1ebc-4ad1-acf8-37a70a5fa01a" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bee65ac6-98c7-4c0b-89a6-80ea12bffe43", "9a3b52e3-6972-46e0-b3a6-ea0d8a9f4bdd" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67bb1c77-333c-41c9-b0e2-8fca172132ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "719de307-49bd-49ac-bfd5-773a68435e4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bee65ac6-98c7-4c0b-89a6-80ea12bffe43");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0729feac-6b11-483c-8424-40003dbbd6f6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "156c9391-1ebc-4ad1-acf8-37a70a5fa01a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9a3b52e3-6972-46e0-b3a6-ea0d8a9f4bdd");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "fea16581-01e7-42aa-8394-a7430f4e63af");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ea9abb9-f0ee-456f-9d80-5c75d95ca7a1", "microsoft", "MICROSOFT" },
                    { "4b8bcf90-dfa6-4394-9d53-9686cd0e9ed9", "google", "GOOGLE" },
                    { "4b915b1f-8b74-45cf-979e-b10f8b98f919", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "42630d5b-ba89-4955-a36a-e31dd8cf7705", "5e69ee78-9632-42a3-9e56-15bb46da26e7", "admin", "ADMIN" },
                    { "61656fea-ed23-460c-bed2-acae223beeef", "3f93db4b-e044-4640-96a3-40093425fab0", "user", "USER" },
                    { "acb67e3b-29f7-409a-af25-8dac9168fe56", "4f564740-89b2-4e57-93f5-579aab7b661f", "landlord", "LANDLORD" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362", 0, "5cd1d3aa-e538-44c5-8af6-e59520e2a973", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJtcHc1qCjtJnehNWdJ2Gk5wV41Dd+AKDkkOvzVMUOIJ/haHphiSaGKBKgl0mib7+A==", null, false, null, "4b915b1f-8b74-45cf-979e-b10f8b98f919", "676321b8-1748-4e2d-9b6e-602849ecf26c", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "2de95519-31eb-459e-8ad1-5021989213f5", 0, "01c1e30e-888f-4f11-91e9-875ba385cbe5", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEJil2s4PH5/oiIIzET0MZif5nEwFry6KmL4TzvDxC04ljdbVtJ4oyvW3IKxjeeT73Q==", null, false, null, "4b915b1f-8b74-45cf-979e-b10f8b98f919", "f372039e-cc5c-4961-95d5-daba427cf305", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "48996947-a573-4470-8d32-7797a2bac0e5", 0, "1865f161-f61a-45cd-a8f5-f7c58e2a040a", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEEzFRjlpejUeQ2FduLqTCg+K1pAhDOKOgbm9hiZ2mwlYaTN718YVLhUXnZ96HXOvmQ==", null, false, null, "4b915b1f-8b74-45cf-979e-b10f8b98f919", "286c81f7-66b2-460e-97b3-c0876b8b5b16", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "42630d5b-ba89-4955-a36a-e31dd8cf7705", "2c3afa8a-56eb-47b4-bd81-3bdce7ce3362" },
                    { "acb67e3b-29f7-409a-af25-8dac9168fe56", "2de95519-31eb-459e-8ad1-5021989213f5" },
                    { "61656fea-ed23-460c-bed2-acae223beeef", "48996947-a573-4470-8d32-7797a2bac0e5" }
                });
        }
    }
}
