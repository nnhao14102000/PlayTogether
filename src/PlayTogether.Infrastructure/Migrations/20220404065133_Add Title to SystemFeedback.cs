using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class AddTitletoSystemFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemFeedback_AppUsers_UserId",
                table: "SystemFeedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemFeedback",
                table: "SystemFeedback");

            migrationBuilder.RenameTable(
                name: "SystemFeedback",
                newName: "SystemFeedbacks");

            migrationBuilder.RenameIndex(
                name: "IX_SystemFeedback_UserId",
                table: "SystemFeedbacks",
                newName: "IX_SystemFeedbacks_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfFeedback",
                table: "SystemFeedbacks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "SystemFeedbacks",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SystemFeedbacks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemFeedbacks",
                table: "SystemFeedbacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemFeedbacks_AppUsers_UserId",
                table: "SystemFeedbacks",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemFeedbacks_AppUsers_UserId",
                table: "SystemFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemFeedbacks",
                table: "SystemFeedbacks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SystemFeedbacks");

            migrationBuilder.RenameTable(
                name: "SystemFeedbacks",
                newName: "SystemFeedback");

            migrationBuilder.RenameIndex(
                name: "IX_SystemFeedbacks_UserId",
                table: "SystemFeedback",
                newName: "IX_SystemFeedback_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfFeedback",
                table: "SystemFeedback",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "SystemFeedback",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemFeedback",
                table: "SystemFeedback",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemFeedback_AppUsers_UserId",
                table: "SystemFeedback",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
