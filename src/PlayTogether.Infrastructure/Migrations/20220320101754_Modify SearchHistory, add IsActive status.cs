using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class ModifySearchHistoryaddIsActivestatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchHistories_AppUsers_HirerId",
                table: "SearchHistories");

            migrationBuilder.DropIndex(
                name: "IX_SearchHistories_HirerId",
                table: "SearchHistories");

            migrationBuilder.DropColumn(
                name: "HirerId",
                table: "SearchHistories");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SearchHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistories_UserId",
                table: "SearchHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchHistories_AppUsers_UserId",
                table: "SearchHistories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchHistories_AppUsers_UserId",
                table: "SearchHistories");

            migrationBuilder.DropIndex(
                name: "IX_SearchHistories_UserId",
                table: "SearchHistories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SearchHistories");

            migrationBuilder.AddColumn<string>(
                name: "HirerId",
                table: "SearchHistories",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistories_HirerId",
                table: "SearchHistories",
                column: "HirerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchHistories_AppUsers_HirerId",
                table: "SearchHistories",
                column: "HirerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
