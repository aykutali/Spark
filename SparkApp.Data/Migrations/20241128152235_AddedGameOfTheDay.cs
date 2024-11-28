using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparkApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameOfTheDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameOfTheDayStreak",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GamesOfTheDays",
                columns: table => new
                {
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesOfTheDays", x => x.Day);
                    table.ForeignKey(
                        name: "FK_GamesOfTheDays_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamesOfTheDays_GameId",
                table: "GamesOfTheDays",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesOfTheDays");

            migrationBuilder.DropColumn(
                name: "GameOfTheDayStreak",
                table: "AspNetUsers");
        }
    }
}
