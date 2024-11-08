using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparkApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class GamesDescripNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "Description of the game",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Description of the game");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Description of the game",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Description of the game");
        }
    }
}
