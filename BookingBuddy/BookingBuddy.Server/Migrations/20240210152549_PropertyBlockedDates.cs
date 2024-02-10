using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingBuddy.Server.Migrations
{
    /// <inheritdoc />
    public partial class PropertyBlockedDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyId",
                table: "BlockedDate",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlockedDate_PropertyId",
                table: "BlockedDate",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedDate_Property_PropertyId",
                table: "BlockedDate",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedDate_Property_PropertyId",
                table: "BlockedDate");

            migrationBuilder.DropIndex(
                name: "IX_BlockedDate_PropertyId",
                table: "BlockedDate");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "BlockedDate");
        }
    }
}
