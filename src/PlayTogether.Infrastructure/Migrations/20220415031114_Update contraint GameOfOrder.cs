using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class UpdatecontraintGameOfOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrders_Games_GameId",
                table: "GameOfOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrders_Orders_OrderId",
                table: "GameOfOrders");

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "GameOfOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "GameOfOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOfOrders_Games_GameId",
                table: "GameOfOrders",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOfOrders_Orders_OrderId",
                table: "GameOfOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrders_Games_GameId",
                table: "GameOfOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrders_Orders_OrderId",
                table: "GameOfOrders");

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "GameOfOrders",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "GameOfOrders",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOfOrders_Games_GameId",
                table: "GameOfOrders",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOfOrders_Orders_OrderId",
                table: "GameOfOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
