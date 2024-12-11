using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparkApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalTouchs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Directors_LeadGameDirectorId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "IsSubGenre",
                table: "GamesGenres");

            migrationBuilder.DropColumn(
                name: "GameOfTheDayStreak",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadGameDirectorId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Lead game director of the game",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "Lead game director of the game");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Directors_LeadGameDirectorId",
                table: "Games",
                column: "LeadGameDirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Directors_LeadGameDirectorId",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubGenre",
                table: "GamesGenres",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Shows is the genre is a main or sub(secondary) of the game");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadGameDirectorId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Lead game director of the game",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Lead game director of the game");

            migrationBuilder.AddColumn<int>(
                name: "GameOfTheDayStreak",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Directors_LeadGameDirectorId",
                table: "Games",
                column: "LeadGameDirectorId",
                principalTable: "Directors",
                principalColumn: "Id");
        }
    }
}
