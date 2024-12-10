using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using SparkApp.Data.Models;
using SparkApp.Data.Seeding.DataObjects;

namespace SparkApp.Data.Seeding
{
	public class DbSeeder
	{
		private const string errorMessage = "Error occurred while seeding data in the database";

		public static async Task SeedDb(IServiceProvider services, string jsonPathGenres,
																	string jsonPathDevs,
																	string jsonPathDirectors,
																	string jsonPathPlatforms,
																	string jsonPathGames,
																	string jsonPathGamesGenres,
																	string jsonPathGamesPlatforms)
		{
			using SparkDbContext db = services.GetRequiredService<SparkDbContext>();

			ICollection<Genre> allGenres = db
				.Genres
				.ToArray();

			List<Genre> genresToAdd = new List<Genre>();

			try
			{
				string jsonInput = File
					.ReadAllText(jsonPathGenres);

				ImportGenreDto[] genreDtos =
					JsonConvert.DeserializeObject<ImportGenreDto[]>(jsonInput);

				foreach (var genreDto in genreDtos)
				{
					if (!IsValid(genreDto))
					{
						continue;
					}

					Guid genreGuid = Guid.Empty;
					if (!IsGuidValid(genreDto.Id, ref genreGuid))
					{
						continue;
					}

					if (allGenres.Any(g => g.Id.ToString().ToLowerInvariant() ==
										   genreGuid.ToString().ToLowerInvariant()))
					{
						continue;
					}

					Genre genre = new Genre()
					{
						Id = genreGuid,
						Name = genreDto.Name,
						Description = genreDto.Description
					};

					genresToAdd.Add(genre);
				}

				if (genresToAdd.Any())
				{
					db.Genres.AddRange(genresToAdd);
					db.SaveChanges();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(errorMessage);
			}

			ICollection<Developer> allDevs = db.Developer.ToArray();

			List<Developer> devsToAdd = new List<Developer>();

			try
			{
				string jsonInput = File
					.ReadAllText(jsonPathDevs);

				ImportDeveloperDto[] devsDtos =
					JsonConvert.DeserializeObject<ImportDeveloperDto[]>(jsonInput);

				foreach (var devDto in devsDtos)
				{
					if (!IsValid(devDto))
					{
						continue;
					}

					Guid devGuid = Guid.Empty;
					if (!IsGuidValid(devDto.Id, ref devGuid))
					{
						continue;
					}

					if (allDevs.Any(d => d.Id.ToString().ToLowerInvariant() ==
										   devGuid.ToString().ToLowerInvariant()))
					{
						continue;
					}

					Developer dev = new Developer()
					{
						Id = devGuid,
						Name = devDto.Name,
						LogoUrl = devDto.LogoUrl
					};

					devsToAdd.Add(dev);
				}

				if (devsToAdd.Any())
				{
					db.Developer.AddRange(devsToAdd);
					db.SaveChanges();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(errorMessage);
			}

			ICollection<Director> allDirectors = db.Directors.ToArray();

			List<Director> directorsToAdd = new List<Director>();

			try
			{
				string jsonInput = File
					.ReadAllText(jsonPathDirectors);

				ImportDirectorDto[] directorsDtos =
					JsonConvert.DeserializeObject<ImportDirectorDto[]>(jsonInput);

				foreach (var dirDto in directorsDtos)
				{
					if (!IsValid(dirDto))
					{
						continue;
					}

					Guid dirGuid = Guid.Empty;
					if (!IsGuidValid(dirDto.Id, ref dirGuid))
					{
						continue;
					}

					if (allDirectors.Any(d => d.Id.ToString().ToLowerInvariant() ==
										 dirGuid.ToString().ToLowerInvariant()))
					{
						continue;
					}

					Director director = new Director()
					{
						Id = dirGuid,
						Name = dirDto.Name,
						About = dirDto.About,
						ImageUrl = dirDto.ImageUrl
					};

					directorsToAdd.Add(director);
				}

				if (directorsToAdd.Any())
				{
					db.Directors.AddRange(directorsToAdd);
					db.SaveChanges();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(errorMessage);
			}


			ICollection<Platform> allPlatforms = db.Platforms.ToArray();

			List<Platform> platformsToAdd = new List<Platform>();

			try
			{
				string jsonInput = File
					.ReadAllText(jsonPathPlatforms);

				ImportPlatformDto[] platformsDtos =
					JsonConvert.DeserializeObject<ImportPlatformDto[]>(jsonInput);

				foreach (var platformDto in platformsDtos)
				{
					if (!IsValid(platformDto))
					{
						continue;
					}

					Guid platformGuid = Guid.Empty;
					if (!IsGuidValid(platformDto.Id, ref platformGuid))
					{
						continue;
					}

					if (allPlatforms.Any(d => d.Id.ToString().ToLowerInvariant() ==
											  platformGuid.ToString().ToLowerInvariant()))
					{
						continue;
					}

					Platform platform = new Platform()
					{
						Id = platformGuid,
						Name = platformDto.Name
					};

					platformsToAdd.Add(platform);
				}

				if (platformsToAdd.Any())
				{
					db.Platforms.AddRange(platformsToAdd);
					db.SaveChanges();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(errorMessage);
			}

			ICollection<Game> allGames = db.Games.ToArray();

			List<Game> gamesToAdd = new List<Game>();

			try
			{
				string jsonInput = File
					.ReadAllText(jsonPathGames);

				ImportGameDto[] gamesDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonInput);

				foreach (var gameDto in gamesDtos)
				{
					if (!IsValid(gameDto))
					{
						continue;
					}

					Guid gameGuid = Guid.Empty;
					Guid devGuid = Guid.Empty;
					Guid directorGuid = Guid.Empty;
					Guid genreGuid = Guid.Empty;

					if (!IsGuidValid(gameDto.Id, ref gameGuid))
					{
						continue;
					}
					if (!IsGuidValid(gameDto.DeveloperId, ref devGuid))
					{
						continue;
					}
					if (!IsGuidValid(gameDto.LeadGameDirectorId, ref directorGuid))
					{
						continue;
					}
					if (!IsGuidValid(gameDto.MainGenreId, ref genreGuid))
					{
						continue;
					}

					bool isReleaseDateValid = DateTime
						.TryParse(gameDto.ReleaseDate, CultureInfo.InvariantCulture, DateTimeStyles.None,
							out DateTime releaseDate);
					if (!isReleaseDateValid)
					{
						continue;
					}

					if (allGames.Any(g => g.Id.ToString().ToLowerInvariant() == gameGuid.ToString().ToLowerInvariant()))
					{
						continue;
					}

					Game game = new Game()
					{
						Id = gameGuid,
						DeveloperId = devGuid,
						LeadGameDirectorId = directorGuid,
						MainGenreId = genreGuid,
						Title = gameDto.Title,
						Description = gameDto.Description,
						ImageUrl = gameDto.ImageUrl,
						IsConfirmed = gameDto.IsConfirmed,
						IsDeleted = gameDto.IsDeleted,
						ReleaseDate = releaseDate
					};

					gamesToAdd.Add(game);
				}

				if (gamesToAdd.Any())
				{
					db.Games.AddRange(gamesToAdd);
					db.SaveChanges();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(errorMessage);
			}

			
			ICollection<GameGenre> allGameGenres = db.GamesGenres.ToArray();

			List<GameGenre> gameGenresToAdd = new List<GameGenre>();

			try
			{
				string jsonInput = File.ReadAllText(jsonPathGamesGenres);

				ImportGameGenreDto[] gamesGenresDtos = JsonConvert.DeserializeObject<ImportGameGenreDto[]>(jsonInput);

				foreach (var ggDto in gamesGenresDtos)
				{
					if (!IsValid(ggDto))
					{
						continue;
					}

					Guid gameGuid = Guid.Empty;
					Guid genreGuid = Guid.Empty;
					if (!IsGuidValid(ggDto.GameId, ref gameGuid))
					{
						continue;
					}
					if (!IsGuidValid(ggDto.GenreId, ref genreGuid))
					{
						continue;
					}

					if (allGameGenres.Any(gg => gg.GameId.ToString().ToLowerInvariant() == gameGuid.ToString().ToLowerInvariant() &&
														 gg.GenreId.ToString().ToLowerInvariant() == genreGuid.ToString().ToLowerInvariant()))
					{
						continue;
					}

					GameGenre gg = new GameGenre()
					{
						GenreId = genreGuid,
						GameId = gameGuid,
						IsDeleted = ggDto.IsDeleted
					};

					gameGenresToAdd.Add(gg);
				}

				if (gameGenresToAdd.Any())
				{
					db.GamesGenres.AddRange(gameGenresToAdd);
					db.SaveChanges();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(errorMessage);
			}

			ICollection<GamePlatform> allGamePlatforms = db.GamesPlatforms.ToArray();

			List<GamePlatform> gamePlatformsToAdd = new List<GamePlatform>();

			try
			{
				string jsonInput = File.ReadAllText(jsonPathGamesPlatforms);

				ImportGamePlatformDto[] gpDtos = JsonConvert.DeserializeObject<ImportGamePlatformDto[]>(jsonInput);

				foreach (var gpDto in gpDtos)
				{
					if (!IsValid(gpDto))
					{
						continue;
					}

					Guid gameGuid = Guid.Empty;
					Guid platformGuid = Guid.Empty;
					if (!IsGuidValid(gpDto.GameId, ref gameGuid))
					{
						continue;
					}

					if (!IsGuidValid(gpDto.PlatformId, ref platformGuid))
					{
						continue;
					}

					if (allGamePlatforms.Any(gp => gp.GameId.ToString().ToLowerInvariant() == gameGuid.ToString().ToLowerInvariant() &&
												 gp.PlatformId.ToString().ToLowerInvariant() == platformGuid.ToString().ToLowerInvariant()))
					{
						continue;
					}

					GamePlatform gp = new GamePlatform()
					{
						GameId = gameGuid,
						PlatformId = platformGuid,
						IsDeleted = gpDto.IsDeleted,
						LinkToPlatform = gpDto.LinkToPlatform
					};

					gamePlatformsToAdd.Add(gp);
				}

				if (gamePlatformsToAdd.Any())
				{
					db.GamesPlatforms.AddRange(gamePlatformsToAdd);
					db.SaveChanges();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(errorMessage);
			}
		}

		private static bool IsValid(object obj)
		{
			List<ValidationResult> validationResults = new List<ValidationResult>();

			var context = new ValidationContext(obj);
			var isValid = Validator.TryValidateObject(obj, context, validationResults);

			return isValid;
		}

		private static bool IsGuidValid(string id, ref Guid parsedGuid)
		{
			if (String.IsNullOrWhiteSpace(id))
			{
				return false;
			}

			bool isGuidValid = Guid.TryParse(id, out parsedGuid);
			if (!isGuidValid)
			{
				return false;
			}

			return true;
		}
	}
}
