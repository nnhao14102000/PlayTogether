using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class ModifyBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorHistories_BehaviorPoints_BehaviorPointId",
                table: "BehaviorHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorPoints_AppUsers_UserId",
                table: "BehaviorPoints");

            migrationBuilder.DropIndex(
                name: "IX_BehaviorPoints_UserId",
                table: "BehaviorPoints");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BehaviorPoints",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BehaviorPointId",
                table: "BehaviorHistories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Num",
                table: "BehaviorHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TypePoint",
                table: "BehaviorHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorPoints_UserId",
                table: "BehaviorPoints",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorHistories_BehaviorPoints_BehaviorPointId",
                table: "BehaviorHistories",
                column: "BehaviorPointId",
                principalTable: "BehaviorPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorPoints_AppUsers_UserId",
                table: "BehaviorPoints",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorHistories_BehaviorPoints_BehaviorPointId",
                table: "BehaviorHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorPoints_AppUsers_UserId",
                table: "BehaviorPoints");

            migrationBuilder.DropIndex(
                name: "IX_BehaviorPoints_UserId",
                table: "BehaviorPoints");

            migrationBuilder.DropColumn(
                name: "Num",
                table: "BehaviorHistories");

            migrationBuilder.DropColumn(
                name: "TypePoint",
                table: "BehaviorHistories");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BehaviorPoints",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "BehaviorPointId",
                table: "BehaviorHistories",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorPoints_UserId",
                table: "BehaviorPoints",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorHistories_BehaviorPoints_BehaviorPointId",
                table: "BehaviorHistories",
                column: "BehaviorPointId",
                principalTable: "BehaviorPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorPoints_AppUsers_UserId",
                table: "BehaviorPoints",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
