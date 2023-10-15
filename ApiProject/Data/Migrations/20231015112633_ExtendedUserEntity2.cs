using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProject.Data.Migrations
{
    public partial class ExtendedUserEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_appUser_AppUserid",
                table: "Photo");

            migrationBuilder.RenameColumn(
                name: "AppUserid",
                table: "Photo",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_AppUserid",
                table: "Photo",
                newName: "IX_Photo_AppUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActive",
                table: "appUser",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "appUser",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_appUser_AppUserId",
                table: "Photo",
                column: "AppUserId",
                principalTable: "appUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_appUser_AppUserId",
                table: "Photo");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Photo",
                newName: "AppUserid");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_AppUserId",
                table: "Photo",
                newName: "IX_Photo_AppUserid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActive",
                table: "appUser",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "appUser",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_appUser_AppUserid",
                table: "Photo",
                column: "AppUserid",
                principalTable: "appUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
