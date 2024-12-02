using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SparkApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMinimalSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Developer",
                columns: new[] { "Id", "LogoUrl", "Name" },
                values: new object[] { new Guid("ee33ddcf-011e-4266-9811-a778a31223db"), null, "FromSoftware" });

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "About", "ImageUrl", "Name" },
                values: new object[] { new Guid("9b89e6c5-2893-429a-8b65-08dd000ce7b6"), "Creator of the Souls games and \"souls\"-\"souls-like\" genres", "https://i.namu.wiki/i/qBSmQPJjYdPqrnDue2wc7H_44TEHRF3-l4e31U0iPUnGxd7vAmZnffhsynOvYckzPWHjyK1hrDVeFpeMtprgQA.webp", "Hidetaka Miyazaki" });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("2332b52f-6bd8-41fd-a527-7cce41c1b19a"), "big maps with lot side quests", "Open world" },
                    { new Guid("3edae636-cdb6-4995-8707-ef0a69f2733c"), "smash and explore", "Action-Adventure" },
                    { new Guid("63c7dfdc-bcd4-4213-94a5-7f8eefb5a720"), "die and dew it again", "Rogue-lite" },
                    { new Guid("69dded8c-b117-4616-ae9c-10f763b26669"), "git gud more", "Souls-like" },
                    { new Guid("ebee4b43-16a8-435a-8448-bdbd863ba747"), "find a key ,go back to the door, open it find, a another key to another door", "Metroidvania" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "DeveloperId", "ImageUrl", "IsConfirmed", "IsDeleted", "LeadGameDirectorId", "MainGenreId", "ReleaseDate", "Title" },
                values: new object[] { new Guid("9128813e-a6da-47e5-831c-1a3400915fa3"), "open world souls game", new Guid("ee33ddcf-011e-4266-9811-a778a31223db"), "https://image.api.playstation.com/vulcan/ap/rnd/202110/2000/phvVT0qZfcRms5qDAk0SI3CM.png", false, false, new Guid("9b89e6c5-2893-429a-8b65-08dd000ce7b6"), new Guid("69dded8c-b117-4616-ae9c-10f763b26669"), new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elden Ring" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("9128813e-a6da-47e5-831c-1a3400915fa3"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("2332b52f-6bd8-41fd-a527-7cce41c1b19a"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("3edae636-cdb6-4995-8707-ef0a69f2733c"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("63c7dfdc-bcd4-4213-94a5-7f8eefb5a720"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ebee4b43-16a8-435a-8448-bdbd863ba747"));

            migrationBuilder.DeleteData(
                table: "Developer",
                keyColumn: "Id",
                keyValue: new Guid("ee33ddcf-011e-4266-9811-a778a31223db"));

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: new Guid("9b89e6c5-2893-429a-8b65-08dd000ce7b6"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("69dded8c-b117-4616-ae9c-10f763b26669"));
        }
    }
}
