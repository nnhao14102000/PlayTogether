using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class UpdateAppUserNameInUserBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBalances_AppUsers_UserId",
                table: "UserBalances");

            migrationBuilder.DropIndex(
                name: "IX_UserBalances_UserId",
                table: "UserBalances");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserBalances",
                newName: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBalances_AppUserId",
                table: "UserBalances",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBalances_AppUsers_AppUserId",
                table: "UserBalances",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBalances_AppUsers_AppUserId",
                table: "UserBalances");

            migrationBuilder.DropIndex(
                name: "IX_UserBalances_AppUserId",
                table: "UserBalances");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UserBalances",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBalances_UserId",
                table: "UserBalances",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBalances_AppUsers_UserId",
                table: "UserBalances",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
