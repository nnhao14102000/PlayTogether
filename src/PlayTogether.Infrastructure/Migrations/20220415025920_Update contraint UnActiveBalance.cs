using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class UpdatecontraintUnActiveBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnActiveBalances_Orders_OrderId",
                table: "UnActiveBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_UnActiveBalances_UserBalances_UserBalanceId",
                table: "UnActiveBalances");

            migrationBuilder.AlterColumn<string>(
                name: "UserBalanceId",
                table: "UnActiveBalances",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "UnActiveBalances",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UnActiveBalances_Orders_OrderId",
                table: "UnActiveBalances",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnActiveBalances_UserBalances_UserBalanceId",
                table: "UnActiveBalances",
                column: "UserBalanceId",
                principalTable: "UserBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnActiveBalances_Orders_OrderId",
                table: "UnActiveBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_UnActiveBalances_UserBalances_UserBalanceId",
                table: "UnActiveBalances");

            migrationBuilder.AlterColumn<string>(
                name: "UserBalanceId",
                table: "UnActiveBalances",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "UnActiveBalances",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_UnActiveBalances_Orders_OrderId",
                table: "UnActiveBalances",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnActiveBalances_UserBalances_UserBalanceId",
                table: "UnActiveBalances",
                column: "UserBalanceId",
                principalTable: "UserBalances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
