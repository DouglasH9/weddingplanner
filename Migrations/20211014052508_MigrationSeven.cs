using Microsoft.EntityFrameworkCore.Migrations;

namespace weddingplanner.Migrations
{
    public partial class MigrationSeven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rsvps_RsvpId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RsvpId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RsvpId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Rsvps_UserId",
                table: "Rsvps",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rsvps_Users_UserId",
                table: "Rsvps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rsvps_Users_UserId",
                table: "Rsvps");

            migrationBuilder.DropIndex(
                name: "IX_Rsvps_UserId",
                table: "Rsvps");

            migrationBuilder.AddColumn<int>(
                name: "RsvpId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RsvpId",
                table: "Users",
                column: "RsvpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rsvps_RsvpId",
                table: "Users",
                column: "RsvpId",
                principalTable: "Rsvps",
                principalColumn: "RsvpId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
