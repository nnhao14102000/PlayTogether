using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class UpdatecontraintCharityWithdraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharityWithdraws_Charities_CharityId",
                table: "CharityWithdraws");

            migrationBuilder.AlterColumn<string>(
                name: "CharityId",
                table: "CharityWithdraws",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityWithdraws_Charities_CharityId",
                table: "CharityWithdraws",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharityWithdraws_Charities_CharityId",
                table: "CharityWithdraws");

            migrationBuilder.AlterColumn<string>(
                name: "CharityId",
                table: "CharityWithdraws",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityWithdraws_Charities_CharityId",
                table: "CharityWithdraws",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
