using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class TransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistories_UserBalances_UserBalanceId",
                table: "TransactionHistories");

            migrationBuilder.AlterColumn<string>(
                name: "UserBalanceId",
                table: "TransactionHistories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfTransaction",
                table: "TransactionHistories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Operation",
                table: "TransactionHistories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistories_UserBalances_UserBalanceId",
                table: "TransactionHistories",
                column: "UserBalanceId",
                principalTable: "UserBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistories_UserBalances_UserBalanceId",
                table: "TransactionHistories");

            migrationBuilder.AlterColumn<string>(
                name: "UserBalanceId",
                table: "TransactionHistories",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfTransaction",
                table: "TransactionHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Operation",
                table: "TransactionHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistories_UserBalances_UserBalanceId",
                table: "TransactionHistories",
                column: "UserBalanceId",
                principalTable: "UserBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
