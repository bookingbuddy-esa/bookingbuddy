using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class PaymentEntityReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Entity",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "33111571-4c78-4e33-a807-8a210cb573b5", "google", "GOOGLE" },
                    { "66bb57d5-e9f2-4403-b227-4ede7fc79bb5", "microsoft", "MICROSOFT" },
                    { "b81bc9f6-9cdc-4d60-be3b-8367414d1811", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "497d10df-3747-46f1-8dcf-b66f20f5da3d", "7ebfa63a-4807-4544-a598-96dd57a18307", "admin", "ADMIN" },
                    { "5a1b2ba7-66fd-4d0e-bc71-1f2824f901a3", "c173d319-cd61-49a6-bf41-bcefe659f0d2", "user", "USER" },
                    { "5ddead2a-6e21-4c18-99d2-d755495ef627", "8f36dfb1-835d-443b-b6d8-26362684d25a", "landlord", "LANDLORD" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "b057c7e3-cdee-413a-8519-679d208487c6", 0, "c5d9313e-797a-4e3e-98a2-215453844ba5", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEE6F0+amt1r4upK81a+qylNK2z2mx+cMiL/IWVKI1IrSR83eskUGCT43mUquqdbsdQ==", null, false, null, "b81bc9f6-9cdc-4d60-be3b-8367414d1811", "5aee445b-771a-40c9-b802-b1fe6e72fd0b", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "fc70eb1f-a3a0-45eb-8c90-dcbc1f89b617", 0, "74782839-169b-493c-9bd2-dc9807480f60", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEMrzUxss7v/Jy+Y7mO5Xmkh9JQzsYofknRf+XREYuWGd2ky/s/jdCkB51t38kw2Emg==", null, false, null, "b81bc9f6-9cdc-4d60-be3b-8367414d1811", "c7f211f8-6bef-4f83-a8f5-7d0f3a677623", false, "bookingbuddy.landlord@bookingbuddy.com" },
                    { "fd492321-0413-4b81-ba6a-6cb3082ccfb4", 0, "7e39e048-d66f-4015-827e-24891cf03c5d", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAENWNhRQET30ZdlqH8SbLaxfLEjGyzN7QhkSLzsCY1cTwE+zyXQ9HY55kwFAoxU+N9g==", null, false, null, "b81bc9f6-9cdc-4d60-be3b-8367414d1811", "c1474d89-6528-451c-a499-57c65b4cd0ed", false, "bookingbuddy.user@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "497d10df-3747-46f1-8dcf-b66f20f5da3d", "b057c7e3-cdee-413a-8519-679d208487c6" },
                    { "5ddead2a-6e21-4c18-99d2-d755495ef627", "fc70eb1f-a3a0-45eb-8c90-dcbc1f89b617" },
                    { "5a1b2ba7-66fd-4d0e-bc71-1f2824f901a3", "fd492321-0413-4b81-ba6a-6cb3082ccfb4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "33111571-4c78-4e33-a807-8a210cb573b5");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "66bb57d5-e9f2-4403-b227-4ede7fc79bb5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "497d10df-3747-46f1-8dcf-b66f20f5da3d", "b057c7e3-cdee-413a-8519-679d208487c6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5ddead2a-6e21-4c18-99d2-d755495ef627", "fc70eb1f-a3a0-45eb-8c90-dcbc1f89b617" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5a1b2ba7-66fd-4d0e-bc71-1f2824f901a3", "fd492321-0413-4b81-ba6a-6cb3082ccfb4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "497d10df-3747-46f1-8dcf-b66f20f5da3d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a1b2ba7-66fd-4d0e-bc71-1f2824f901a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ddead2a-6e21-4c18-99d2-d755495ef627");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b057c7e3-cdee-413a-8519-679d208487c6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fc70eb1f-a3a0-45eb-8c90-dcbc1f89b617");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fd492321-0413-4b81-ba6a-6cb3082ccfb4");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "b81bc9f6-9cdc-4d60-be3b-8367414d1811");

            migrationBuilder.DropColumn(
                name: "Entity",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Payment");

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
    }
}
