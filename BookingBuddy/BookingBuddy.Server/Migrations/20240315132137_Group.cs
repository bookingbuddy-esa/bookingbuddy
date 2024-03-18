using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class Group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Property",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MembersId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertiesId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChoosenProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessagesId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Property_GroupId",
                table: "Property",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Groups_GroupId",
                table: "Property",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Groups_GroupId",
                table: "Property");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Property_GroupId",
                table: "Property");

           

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Property");

          
        }
    }
}
