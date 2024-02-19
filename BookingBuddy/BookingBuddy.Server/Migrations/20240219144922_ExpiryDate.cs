using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class ExpiryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ExpiryDate",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetProviders",
                columns: new[] { "AspNetProviderId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "45cdd019-aca3-4091-8e53-b870b256e9ba", "microsoft", "MICROSOFT" },
                    { "4bc9da14-f7a1-41ac-9b3e-f6c44912cf2f", "google", "GOOGLE" },
                    { "a3de67aa-c2f4-4299-a92a-65929c8c10b6", "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21917a74-9b97-4726-9ebb-6bc133def3e9", "4637aaf4-3535-4734-b83d-002904b4cd8a", "admin", "ADMIN" },
                    { "89dd821b-85e2-47ba-9911-eecac3f83571", "e072a319-c2c6-43a7-b7ec-cd9517d88267", "landlord", "LANDLORD" },
                    { "bd69334c-3b42-46ee-b0bd-643a4daf884d", "d2316314-221d-43c3-9edf-e22fe6b3b9e8", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "ProviderId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "058ebefb-da27-4c16-8556-b2b3b4665abd", 0, "70c31c2d-5f01-4bd8-ad7c-b99d364829a9", "bookingbuddy.admin@bookingbuddy.com", true, false, null, "admin", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "BOOKINGBUDDY.ADMIN@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEFO93kR9fJaAEp0kJvitrJKDiN2qpCtY7h2VgyAnf6icyy57VGclzPmv5bcNAHRe5w==", null, false, null, "a3de67aa-c2f4-4299-a92a-65929c8c10b6", "a6949cc5-657d-4180-a6a7-f0f9a2792e51", false, "bookingbuddy.admin@bookingbuddy.com" },
                    { "a984771f-b3ac-4685-a7b0-508597d557e6", 0, "8202b26a-2ecd-46f3-a273-b9ae239f8a11", "bookingbuddy.user@bookingbuddy.com", true, false, null, "user", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "BOOKINGBUDDY.USER@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEOX/IeFVlVQDtybX+bVRWvZctOcMmiSG7R3uyN2EqhuD2W3wt/Y0mykwAbo546jYSg==", null, false, null, "a3de67aa-c2f4-4299-a92a-65929c8c10b6", "78db9cda-f78d-4dbf-b43e-7ecd8318d652", false, "bookingbuddy.user@bookingbuddy.com" },
                    { "adb6e06c-7413-4ec6-975e-2256b9df87d4", 0, "e28b630d-ff87-4325-bbf1-127a975238d1", "bookingbuddy.landlord@bookingbuddy.com", true, false, null, "landlord", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "BOOKINGBUDDY.LANDLORD@BOOKINGBUDDY.COM", "AQAAAAIAAYagAAAAEHKojPNLNeouzUh3dRGD3+wagcH6pQAN/V0HI6nOdz2hWF5aLQypV48RGr45R21Z1Q==", null, false, null, "a3de67aa-c2f4-4299-a92a-65929c8c10b6", "05652e1c-5ea6-4285-ae8f-82e03ae3b061", false, "bookingbuddy.landlord@bookingbuddy.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "21917a74-9b97-4726-9ebb-6bc133def3e9", "058ebefb-da27-4c16-8556-b2b3b4665abd" },
                    { "bd69334c-3b42-46ee-b0bd-643a4daf884d", "a984771f-b3ac-4685-a7b0-508597d557e6" },
                    { "89dd821b-85e2-47ba-9911-eecac3f83571", "adb6e06c-7413-4ec6-975e-2256b9df87d4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "45cdd019-aca3-4091-8e53-b870b256e9ba");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "4bc9da14-f7a1-41ac-9b3e-f6c44912cf2f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "21917a74-9b97-4726-9ebb-6bc133def3e9", "058ebefb-da27-4c16-8556-b2b3b4665abd" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bd69334c-3b42-46ee-b0bd-643a4daf884d", "a984771f-b3ac-4685-a7b0-508597d557e6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "89dd821b-85e2-47ba-9911-eecac3f83571", "adb6e06c-7413-4ec6-975e-2256b9df87d4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21917a74-9b97-4726-9ebb-6bc133def3e9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89dd821b-85e2-47ba-9911-eecac3f83571");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd69334c-3b42-46ee-b0bd-643a4daf884d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "058ebefb-da27-4c16-8556-b2b3b4665abd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a984771f-b3ac-4685-a7b0-508597d557e6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "adb6e06c-7413-4ec6-975e-2256b9df87d4");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "a3de67aa-c2f4-4299-a92a-65929c8c10b6");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Payment");

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
    }
}
