using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class RemoveDateActiveinUserDisable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisableUser_AppUsers_UserId",
                table: "DisableUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DisableUser",
                table: "DisableUser");

            migrationBuilder.DropColumn(
                name: "DateActive",
                table: "DisableUser");

            migrationBuilder.RenameTable(
                name: "DisableUser",
                newName: "DisableUsers");

            migrationBuilder.RenameIndex(
                name: "IX_DisableUser_UserId",
                table: "DisableUsers",
                newName: "IX_DisableUsers_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateDisable",
                table: "DisableUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisableUsers",
                table: "DisableUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DisableUsers_AppUsers_UserId",
                table: "DisableUsers",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisableUsers_AppUsers_UserId",
                table: "DisableUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DisableUsers",
                table: "DisableUsers");

            migrationBuilder.RenameTable(
                name: "DisableUsers",
                newName: "DisableUser");

            migrationBuilder.RenameIndex(
                name: "IX_DisableUsers_UserId",
                table: "DisableUser",
                newName: "IX_DisableUser_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateDisable",
                table: "DisableUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateActive",
                table: "DisableUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisableUser",
                table: "DisableUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DisableUser_AppUsers_UserId",
                table: "DisableUser",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
