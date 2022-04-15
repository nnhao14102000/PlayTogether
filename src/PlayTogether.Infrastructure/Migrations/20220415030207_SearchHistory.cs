using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class SearchHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchHistories_AppUsers_UserId",
                table: "SearchHistories");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SearchHistories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SearchHistories_AppUsers_UserId",
                table: "SearchHistories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchHistories_AppUsers_UserId",
                table: "SearchHistories");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SearchHistories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_SearchHistories_AppUsers_UserId",
                table: "SearchHistories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
