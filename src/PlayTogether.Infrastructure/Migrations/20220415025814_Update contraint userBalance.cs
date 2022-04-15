using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class UpdatecontraintuserBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBalances_AppUsers_UserId",
                table: "UserBalances");

            migrationBuilder.DropIndex(
                name: "IX_UserBalances_UserId",
                table: "UserBalances");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBalances",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBalances_UserId",
                table: "UserBalances",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBalances_AppUsers_UserId",
                table: "UserBalances",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBalances_AppUsers_UserId",
                table: "UserBalances");

            migrationBuilder.DropIndex(
                name: "IX_UserBalances_UserId",
                table: "UserBalances");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBalances",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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
