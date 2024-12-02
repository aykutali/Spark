using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SparkApp.Data.Models;

namespace SparkApp.Data
{



	public class SparkDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{

		public SparkDbContext()
		{

		}

		public SparkDbContext(DbContextOptions options)
			: base(options)
		{

		}

		public virtual DbSet<Game> Games { get; set; }

		public virtual DbSet<Genre> Genres { get; set; }

		public virtual DbSet<Platform> Platforms { get; set; }

		public virtual DbSet<Developer> Developer { get; set; }

		public virtual DbSet<Director> Directors { get; set; }

		public virtual DbSet<GameGenre> GamesGenres { get; set; }


		public virtual DbSet<GamePlatform> GamesPlatforms { get; set; }

		public virtual DbSet<GameOfTheDay> GamesOfTheDays { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<GameGenre>()
				.HasKey(gsg => new { gsg.GameId, gsg.GenreId });

			modelBuilder.Entity<Game>()
				.HasOne(ga => ga.MainGenre)
				.WithMany()
				.HasForeignKey(ga => ga.MainGenreId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<GamePlatform>()
				.HasKey(gp => new { gp.GameId, gp.PlatformId });

			modelBuilder.Entity<Genre>()
				.HasData(
					new Genre()
					{
						Id = new Guid("3EDAE636-CDB6-4995-8707-EF0A69F2733C"),
						Name = "Action-Adventure",
						Description = "smash and explore"
					},
					new Genre()
					{
						Id = new Guid("EBEE4B43-16A8-435A-8448-BDBD863BA747"),
						Name = "Metroidvania",
						Description = "find a key ,go back to the door, open it find, a another key to another door"
					},
					new Genre()
					{
						Id = new Guid("63C7DFDC-BCD4-4213-94A5-7F8EEFB5A720"),
						Name = "Rogue-lite",
						Description = "die and dew it again"
					},
					new Genre()
					{
						Id = new Guid("2332B52F-6BD8-41FD-A527-7CCE41C1B19A"),
						Name = "Open world",
						Description = "big maps with lot side quests"
					},
					new Genre()
					{
						Id = new Guid("69DDED8C-B117-4616-AE9C-10F763B26669"),
						Name = "Souls-like",
						Description = "git gud more"
					});

			modelBuilder.Entity<Director>()
				.HasData(
					new Director()
					{
						Id = new Guid("9B89E6C5-2893-429A-8B65-08DD000CE7B6"),
						Name = "Hidetaka Miyazaki",
						About = "Creator of the Souls games and \"souls\"-\"souls-like\" genres",
						ImageUrl = "https://i.namu.wiki/i/qBSmQPJjYdPqrnDue2wc7H_44TEHRF3-l4e31U0iPUnGxd7vAmZnffhsynOvYckzPWHjyK1hrDVeFpeMtprgQA.webp"
					});

			modelBuilder.Entity<Developer>()
				.HasData(
					new Developer()
					{
						Id = new Guid("EE33DDCF-011E-4266-9811-A778A31223DB"),
						Name = "FromSoftware"
					});

			modelBuilder.Entity<Game>()
				.HasData(
					new Game()
					{
						Id = new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3"),
						Title = "Elden Ring",
						Description = "open world souls game",
						ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202110/2000/phvVT0qZfcRms5qDAk0SI3CM.png",
						ReleaseDate = new DateTime(2022,02,25),
						LeadGameDirectorId = new Guid("9B89E6C5-2893-429A-8B65-08DD000CE7B6"),
						DeveloperId = new Guid("EE33DDCF-011E-4266-9811-A778A31223DB"),
						MainGenreId = new Guid("69DDED8C-B117-4616-AE9C-10F763B26669")
					});
		}
	}
}