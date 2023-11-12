using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProject.Data.Migrations
{
    public partial class LikeEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    SourceUserId = table.Column<int>(type: "integer", nullable: false),
                    TargetUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.SourceUserId, x.TargetUserId });
                    table.ForeignKey(
                        name: "FK_Likes_appUser_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "appUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_appUser_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "appUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_TargetUserId",
                table: "Likes",
                column: "TargetUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
