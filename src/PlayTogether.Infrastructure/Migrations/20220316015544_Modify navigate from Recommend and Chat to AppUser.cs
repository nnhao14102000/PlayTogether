using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class ModifynavigatefromRecommendandChattoAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HirerId",
                table: "Recommends",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "HirerGender",
                table: "Recommends",
                newName: "UserGender");

            migrationBuilder.RenameColumn(
                name: "HirerAge",
                table: "Recommends",
                newName: "UserAge");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Chats",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommends_UserId",
                table: "Recommends",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserId",
                table: "Chats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AppUsers_UserId",
                table: "Chats",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommends_AppUsers_UserId",
                table: "Recommends",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AppUsers_UserId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommends_AppUsers_UserId",
                table: "Recommends");

            migrationBuilder.DropIndex(
                name: "IX_Recommends_UserId",
                table: "Recommends");

            migrationBuilder.DropIndex(
                name: "IX_Chats_UserId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Recommends",
                newName: "HirerId");

            migrationBuilder.RenameColumn(
                name: "UserGender",
                table: "Recommends",
                newName: "HirerGender");

            migrationBuilder.RenameColumn(
                name: "UserAge",
                table: "Recommends",
                newName: "HirerAge");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Chats",
                newName: "SenderId");
        }
    }
}
