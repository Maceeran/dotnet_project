using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Data.Migrations
{
    public partial class AlterUserOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Offer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_UserId",
                table: "Offer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_AspNetUsers_UserId",
                table: "Offer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_AspNetUsers_UserId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_UserId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Offer");
        }
    }
}
