using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class PaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c53013c-ebec-471b-aaf2-3f2361c9c522", "local", "LOCAL" },
                    { "45677807-439d-4c7a-99a5-5cc7985c4ecc", "google", "GOOGLE" },
                    { "615bda1e-68bd-4190-9732-3396b31249a0", "microsoft", "MICROSOFT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39708a80-9206-4cef-9d2f-fc59d6826e9c", "2ebb45d6-f955-4385-9990-0b554c64c512", "admin", "ADMIN" },
                    { "a7b2cc8a-03fa-4a1c-ad0d-9e8bf30dea0c", "37f84d2f-4eaf-4e9f-af6b-963b8a1b7626", "landlord", "LANDLORD" },
                    { "c9d9335a-5d78-4eee-926a-0e4f72b38949", "651a52af-b07b-4c88-b522-b7c460dba9e1", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "66f05769-998e-4d7d-a994-cf0ba22cfd16", 0, "52b6155a-db8f-4bba-b9c6-bc907fce06aa", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAELX4kYbRFoUO6f8bSSYw24YUSgBAV2qqPz3C9MX/IDK+B1iK6XOkxRgCx3S4Fqq3eA==", null, false, null, "0c53013c-ebec-471b-aaf2-3f2361c9c522", "4deb2d73-06e9-4ba9-b49c-3b247d725911", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "fad782f6-d92a-4644-bf63-f6d3a32bc659", 0, "d7d71f74-ea3f-4cb4-9ea4-79e52594ce06", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEFW6lfz38pESC+tlwJpUMEbBUtgBwryWgY7hc89dG1j+CNXgxaUB7DpRmgcLlGqJPQ==", null, false, null, "0c53013c-ebec-471b-aaf2-3f2361c9c522", "aa74dfc5-25be-43d3-af9a-d36ad21e540d", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "fd71abac-8d5a-4ad2-86f6-1b0b7bde0382", 0, "29bdc279-79bd-4019-9854-a26ec845b56f", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEC5ksS873gQFb9xoFu6ZeLNAq9a45XNirdDHqZJyaj4ytCuXVHceXICF/fuHHDGKqg==", null, false, null, "0c53013c-ebec-471b-aaf2-3f2361c9c522", "e139138d-d745-43bc-8ef5-74737a472063", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "39708a80-9206-4cef-9d2f-fc59d6826e9c", "66f05769-998e-4d7d-a994-cf0ba22cfd16" },
                    { "c9d9335a-5d78-4eee-926a-0e4f72b38949", "fad782f6-d92a-4644-bf63-f6d3a32bc659" },
                    { "a7b2cc8a-03fa-4a1c-ad0d-9e8bf30dea0c", "fd71abac-8d5a-4ad2-86f6-1b0b7bde0382" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "45677807-439d-4c7a-99a5-5cc7985c4ecc");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "615bda1e-68bd-4190-9732-3396b31249a0");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39708a80-9206-4cef-9d2f-fc59d6826e9c", "66f05769-998e-4d7d-a994-cf0ba22cfd16" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c9d9335a-5d78-4eee-926a-0e4f72b38949", "fad782f6-d92a-4644-bf63-f6d3a32bc659" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a7b2cc8a-03fa-4a1c-ad0d-9e8bf30dea0c", "fd71abac-8d5a-4ad2-86f6-1b0b7bde0382" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39708a80-9206-4cef-9d2f-fc59d6826e9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7b2cc8a-03fa-4a1c-ad0d-9e8bf30dea0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9d9335a-5d78-4eee-926a-0e4f72b38949");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66f05769-998e-4d7d-a994-cf0ba22cfd16");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fad782f6-d92a-4644-bf63-f6d3a32bc659");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fd71abac-8d5a-4ad2-86f6-1b0b7bde0382");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "0c53013c-ebec-471b-aaf2-3f2361c9c522");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payment");

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
    }
}
