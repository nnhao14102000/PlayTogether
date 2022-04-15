using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class UpdatecontraintDonate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donates_AppUsers_UserId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Charities_CharityId",
                table: "Donates");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Donates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CharityId",
                table: "Donates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_AppUsers_UserId",
                table: "Donates",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_Charities_CharityId",
                table: "Donates",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donates_AppUsers_UserId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Charities_CharityId",
                table: "Donates");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Donates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CharityId",
                table: "Donates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_AppUsers_UserId",
                table: "Donates",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_Charities_CharityId",
                table: "Donates",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
