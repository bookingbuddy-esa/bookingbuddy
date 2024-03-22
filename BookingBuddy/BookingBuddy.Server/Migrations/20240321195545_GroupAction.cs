using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class GroupAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "08b9b067-c893-40c4-a55a-e5bbc50db10f");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "10402366-3458-4288-959b-97e325862780");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "28dbd770-289a-4743-a296-027e50270600");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2e2cca70-22d4-47a2-b804-2b6f5924d3d6");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "34923ddc-6f20-4d0b-94d6-dfdc95520d5b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "34b34579-ef29-47d6-bdde-166ef26cecdb");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "3cb24154-ce16-4103-8d5c-7728cbae3f1a");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "6fbbc417-687d-49ed-bf36-9550f8b3d6f0");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "79b38007-b616-4472-a186-8d1fe00ae430");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "96613c36-23f3-4682-bb6f-5d401b59684b");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "96821fe6-e7f8-48b2-ad31-1142f43f2af0");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f65e181e-d963-4554-86d9-c9ff1e4e10b1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f6b32234-7506-44ea-b1f9-3fb4688e0714");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "3038d074-0d21-41f2-989a-1cb2f20e88a4");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "f20e6820-7dbc-46c1-bedc-ff86eeb851a5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7ab24145-e54a-4c2e-8f8b-879c47c87e03", "16288bfd-42c6-43c5-a61c-c131c79b97cc" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9375e488-b614-46a3-880d-8bdb700c3798", "795f7097-c31b-403e-80b4-4cd02e32c625" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a159784f-3384-4359-a76e-cd1a66bb2700", "a3422c87-d7f7-4354-8f54-52dcc9c91add" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ab24145-e54a-4c2e-8f8b-879c47c87e03");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9375e488-b614-46a3-880d-8bdb700c3798");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a159784f-3384-4359-a76e-cd1a66bb2700");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "16288bfd-42c6-43c5-a61c-c131c79b97cc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "795f7097-c31b-403e-80b4-4cd02e32c625");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a3422c87-d7f7-4354-8f54-52dcc9c91add");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "7a454b3b-18ed-4ecf-8ab7-86a1c70a0283");

            migrationBuilder.AddColumn<int>(
                name: "GroupAction",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "080bec24-ed41-4ceb-a34a-7f3c5feec8b1");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "0ad0b821-d199-413b-b76a-58530d302121");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "2986bf18-0dd5-4c5c-8ca2-63794f0337d4");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "4fd6c4ba-0e5e-4c11-93a5-5dae75554299");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "60484f24-9ff0-4bc4-998c-f1483f27a553");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "86620353-69b2-4742-a67c-4ceec52e7950");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "89e8556f-0e64-425b-85d5-fefb8f875965");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "a72d42b8-945d-4b3d-bd7e-3bce4a52800d");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c6491ef1-7952-4eb4-b980-aef82bbb4799");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "c9d088f5-0814-46d0-9e2b-11dcea9985d2");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "dfcdf6c4-4b8f-4249-9ea6-8887464fe617");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f54a3cb4-2467-4da9-b76d-7a40a1d0f4b5");

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "AmenityId",
                keyValue: "f6e6abed-2392-40e0-b407-437d6b89ad51");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "1e659a58-e3c9-41ff-8587-b74424ebbf3a");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "47828d77-ab7f-4e07-9965-7d76b9c6a6f7");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ffe0180e-dc28-4ad8-9fa8-6db0d101100e", "4bc9364d-95b9-43c0-a155-47b1cb2e521f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2ac02251-a8c1-4efd-bb68-1563b12971e9", "72ac50f2-d54b-48c9-ab43-6efa4a6bb63e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "49660557-31df-44d2-a73e-967563cddcae", "ed784e11-f95d-46e2-8c42-4503aaaf888b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ac02251-a8c1-4efd-bb68-1563b12971e9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49660557-31df-44d2-a73e-967563cddcae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffe0180e-dc28-4ad8-9fa8-6db0d101100e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4bc9364d-95b9-43c0-a155-47b1cb2e521f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "72ac50f2-d54b-48c9-ab43-6efa4a6bb63e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed784e11-f95d-46e2-8c42-4503aaaf888b");

            migrationBuilder.DeleteData(
                table: "AspNetProviders",
                keyColumn: "AspNetProviderId",
                keyValue: "bb84b905-521e-43f7-9109-3f11bae13b61");

            migrationBuilder.DropColumn(
                name: "GroupAction",
                table: "Groups");
        }
    }
}
