using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class RemoveDepositUserWithdraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrder_Games_GameId",
                table: "GameOfOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrder_Orders_OrderId",
                table: "GameOfOrder");

            migrationBuilder.DropTable(
                name: "Deposits");

            migrationBuilder.DropTable(
                name: "UserWithdraws");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameOfOrder",
                table: "GameOfOrder");

            migrationBuilder.RenameTable(
                name: "GameOfOrder",
                newName: "GameOfOrders");

            migrationBuilder.RenameIndex(
                name: "IX_GameOfOrder_OrderId",
                table: "GameOfOrders",
                newName: "IX_GameOfOrders_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOfOrder_GameId",
                table: "GameOfOrders",
                newName: "IX_GameOfOrders_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameOfOrders",
                table: "GameOfOrders",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrders_Games_GameId",
                table: "GameOfOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_GameOfOrders_Orders_OrderId",
                table: "GameOfOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameOfOrders",
                table: "GameOfOrders");

            migrationBuilder.RenameTable(
                name: "GameOfOrders",
                newName: "GameOfOrder");

            migrationBuilder.RenameIndex(
                name: "IX_GameOfOrders_OrderId",
                table: "GameOfOrder",
                newName: "IX_GameOfOrder_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOfOrders_GameId",
                table: "GameOfOrder",
                newName: "IX_GameOfOrder_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameOfOrder",
                table: "GameOfOrder",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    MoneyDeposit = table.Column<double>(type: "float", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deposits_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWithdraws",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    MoneyWithdraw = table.Column<double>(type: "float", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWithdraws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWithdraws_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithdraws_UserId",
                table: "UserWithdraws",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameOfOrder_Games_GameId",
                table: "GameOfOrder",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameOfOrder_Orders_OrderId",
                table: "GameOfOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
