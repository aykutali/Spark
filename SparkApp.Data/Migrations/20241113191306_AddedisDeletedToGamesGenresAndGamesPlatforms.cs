using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparkApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedisDeletedToGamesGenresAndGamesPlatforms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GamesPlatforms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GamesGenres",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GamesPlatforms");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GamesGenres");
        }
    }
}
