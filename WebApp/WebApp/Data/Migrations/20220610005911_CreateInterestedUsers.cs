using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Data.Migrations
{
    public partial class CreateInterestedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInterestedOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInterestedOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInterestedOffer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInterestedOffer_Offer_OfferId1",
                        column: x => x.OfferId1,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInterestedOffer_OfferId1",
                table: "UserInterestedOffer",
                column: "OfferId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterestedOffer_UserId",
                table: "UserInterestedOffer",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInterestedOffer");
        }
    }
}
