using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class ModifySystemFeedbackIsApprovechangetoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsApprove",
                table: "SystemFeedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsApprove",
                table: "SystemFeedbacks",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
