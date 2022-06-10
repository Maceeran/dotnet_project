using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Data.Migrations
{
    public partial class EditInterestedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInterestedOffer_Offer_OfferId1",
                table: "UserInterestedOffer");

            migrationBuilder.DropIndex(
                name: "IX_UserInterestedOffer_OfferId1",
                table: "UserInterestedOffer");

            migrationBuilder.DropColumn(
                name: "OfferId1",
                table: "UserInterestedOffer");

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "UserInterestedOffer",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterestedOffer_OfferId",
                table: "UserInterestedOffer",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterestedOffer_Offer_OfferId",
                table: "UserInterestedOffer",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInterestedOffer_Offer_OfferId",
                table: "UserInterestedOffer");

            migrationBuilder.DropIndex(
                name: "IX_UserInterestedOffer_OfferId",
                table: "UserInterestedOffer");

            migrationBuilder.AlterColumn<string>(
                name: "OfferId",
                table: "UserInterestedOffer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OfferId1",
                table: "UserInterestedOffer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserInterestedOffer_OfferId1",
                table: "UserInterestedOffer",
                column: "OfferId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterestedOffer_Offer_OfferId1",
                table: "UserInterestedOffer",
                column: "OfferId1",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
