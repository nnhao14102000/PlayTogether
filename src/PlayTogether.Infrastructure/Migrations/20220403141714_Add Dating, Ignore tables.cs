using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayTogether.Infrastructure.Migrations
{
    public partial class AddDatingIgnoretables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SatisfiedPoint",
                table: "BehaviorPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Datings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FromHour = table.Column<int>(type: "int", nullable: false),
                    ToHour = table.Column<int>(type: "int", nullable: false),
                    IsMON = table.Column<bool>(type: "bit", nullable: false),
                    IsTUE = table.Column<bool>(type: "bit", nullable: false),
                    IsWED = table.Column<bool>(type: "bit", nullable: false),
                    IsTHU = table.Column<bool>(type: "bit", nullable: false),
                    IsFRI = table.Column<bool>(type: "bit", nullable: false),
                    IsSAT = table.Column<bool>(type: "bit", nullable: false),
                    IsSUN = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Datings_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ignores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IgnoreUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeIgnore = table.Column<int>(type: "int", nullable: false),
                    IsForever = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ignores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ignores_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Datings_UserId",
                table: "Datings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ignores_UserId",
                table: "Ignores",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Datings");

            migrationBuilder.DropTable(
                name: "Ignores");

            migrationBuilder.DropColumn(
                name: "SatisfiedPoint",
                table: "BehaviorPoints");
        }
    }
}
