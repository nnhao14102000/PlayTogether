using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class UpdateRelationShipOfUserBalanceAndTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistories_AppUsers_UserId",
                table: "TransactionHistories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TransactionHistories",
                newName: "UserBalanceId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionHistories_UserId",
                table: "TransactionHistories",
                newName: "IX_TransactionHistories_UserBalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistories_UserBalances_UserBalanceId",
                table: "TransactionHistories",
                column: "UserBalanceId",
                principalTable: "UserBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistories_UserBalances_UserBalanceId",
                table: "TransactionHistories");

            migrationBuilder.RenameColumn(
                name: "UserBalanceId",
                table: "TransactionHistories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionHistories_UserBalanceId",
                table: "TransactionHistories",
                newName: "IX_TransactionHistories_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistories_AppUsers_UserId",
                table: "TransactionHistories",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
