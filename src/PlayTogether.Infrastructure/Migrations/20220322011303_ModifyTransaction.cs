using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class ModifyTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TransactionHistories");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "TransactionHistories",
                newName: "ReferenceTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferenceTransactionId",
                table: "TransactionHistories",
                newName: "TransactionId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TransactionHistories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
