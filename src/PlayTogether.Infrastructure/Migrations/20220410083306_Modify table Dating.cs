using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class ModifytableDating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFRI",
                table: "Datings");

            migrationBuilder.DropColumn(
                name: "IsMON",
                table: "Datings");

            migrationBuilder.DropColumn(
                name: "IsSAT",
                table: "Datings");

            migrationBuilder.DropColumn(
                name: "IsSUN",
                table: "Datings");

            migrationBuilder.DropColumn(
                name: "IsTHU",
                table: "Datings");

            migrationBuilder.DropColumn(
                name: "IsTUE",
                table: "Datings");

            migrationBuilder.DropColumn(
                name: "IsWED",
                table: "Datings");

            migrationBuilder.AddColumn<int>(
                name: "DayInWeek",
                table: "Datings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayInWeek",
                table: "Datings");

            migrationBuilder.AddColumn<bool>(
                name: "IsFRI",
                table: "Datings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMON",
                table: "Datings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSAT",
                table: "Datings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSUN",
                table: "Datings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTHU",
                table: "Datings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTUE",
                table: "Datings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWED",
                table: "Datings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
