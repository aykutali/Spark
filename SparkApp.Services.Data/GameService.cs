
using static SparkApp.Common.EntityValidationConstants.Game;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Director;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Genre;
using System.Globalization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SparkApp.Data;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Services.Data
{
	public class GameService : BaseService, IGameService
	{
		private readonly IRepository<Game, Guid> gameRepository;
		private readonly IRepository<Developer, Guid> devRepository;
		private readonly IRepository<Director, Guid> dirRepository;
		private readonly IRepository<Genre, Guid> genreRepository;
		private readonly IRepository<Platform, Guid> platformRepository;
		private readonly IRepository<GamePlatform, object> gamePlatformRepository;
		private readonly IRepository<GameGenre, object> gameGenreRepository;

		public GameService(IRepository<Game, Guid> gameRepository,
						   IRepository<Director, Guid> dirRepository,
						   IRepository<Genre, Guid> genreRepository,
						   IRepository<Developer, Guid> devRepository,
						   IRepository<Platform, Guid> platformRepository,
						   IRepository<GamePlatform, object> gamePlatformRepository,
						   IRepository<GameGenre, object> gameGenreRepository)
		{
			this.gameRepository = gameRepository;
			this.dirRepository = dirRepository;
			this.genreRepository = genreRepository;
			this.devRepository = devRepository;
			this.platformRepository = platformRepository;
			this.gamePlatformRepository = gamePlatformRepository;
			this.gameGenreRepository = gameGenreRepository;

		}

		public async Task<IQueryable<GameAllViewModel>> GetAllGamesAsync()
		{
			IQueryable<GameAllViewModel> games = gameRepository
				.GetAllAttached()
				.Where(g => g.IsConfirmed && !g.IsDeleted)
				.Select(g => new GameAllViewModel
				{
					Id = g.Id,
					Title = g.Title,
					ImageUrl = g.ImageUrl
				})
				.OrderBy(g => g.Title);

			return games;
		}

		/// <summary>
		/// Check the given guid is valid and find the game
		/// </summary>
		/// <param name="id"></param>
		/// <returns>If there is a game with given guid return a game data, if is no game with that guid return null</returns>
		public async Task<Game?> GetGameByIdAsync(string id)
		{
			Guid parsedGuid = Guid.Empty;
			if (IsGuidValid(id, ref parsedGuid))
			{
				Game? gameData = await gameRepository.GetByIdAsync(Guid.Parse(id));

				return gameData;
			}

			return null;
		}

		public async Task<GameEditViewModel?> GetEditGameModelAsync(string id)
		{
			Game? gameData = await GetGameByIdAsync(id);

			if (gameData != null)
			{
				AddGameInputModel gameListModel = await GetInputGameModelAsync();
				GameEditViewModel gameEditModel = new GameEditViewModel
				{
					Id = gameData.Id.ToString(),
					Title = gameData.Title,
					Description = gameData.Description,
					ImageUrl = gameData.ImageUrl,
					ReleasedDate = gameData.ReleaseDate.ToString(format: ReleasedDateFormat),
					DeveloperId = gameData.DeveloperId.ToString(),
					LeadGameDirectorId = gameData.LeadGameDirectorId.ToString(),
					MainGenreId = gameData.MainGenreId.ToString(),

					Developers = gameListModel.Developers,
					Genres = gameListModel.Genres,
					Directors = gameListModel.Directors,
				};

				return gameEditModel;
			}

			return null;
		}

		public async Task<GameEditViewModel> GetEditGameModelAsync(GameEditViewModel gameData)
		{
			AddGameInputModel gameListModel = await GetInputGameModelAsync();

			GameEditViewModel gameEditModel = new GameEditViewModel
			{
				Developers = gameListModel.Developers,
				Genres = gameListModel.Genres,
				Directors = gameListModel.Directors,
			};

			return gameEditModel;
		}

		public async Task<bool> EditGameAsync(Game gameToEdit, GameEditViewModel editModel)
		{
			editModel.Title = Sanitize(editModel.Title);
			editModel.Description = Sanitize(editModel.Description);
			editModel.ImageUrl = Sanitize(editModel.ImageUrl);

			if (!IsModelValid(editModel))
			{
				return false;
			}

			try
			{
				gameToEdit.Title = editModel.Title;
				gameToEdit.Description = editModel.Description;
				gameToEdit.ImageUrl = editModel.ImageUrl;
				gameToEdit.ReleaseDate = DateTime.Parse(editModel.ReleasedDate);
				gameToEdit.DeveloperId = Guid.Parse(editModel.DeveloperId);
				gameToEdit.LeadGameDirectorId = Guid.Parse(editModel.LeadGameDirectorId);
				gameToEdit.MainGenreId = Guid.Parse(editModel.MainGenreId);

				await gameRepository.UpdateAsync(gameToEdit);

				return true;
			}
			catch (Exception e)
			{
				return false;
			}

		}


		public async Task<GameDetailsViewModel?> GetGameDetailsAsync(string title)
		{
			Game? game = await gameRepository.GetAllAttached()
				.Where(g => g.Title == title &&
								 g.IsConfirmed &&
								 g.IsDeleted == false)
				.Include(g => g.LeadGameDirector)
				.Include(g => g.Developer)
				.Include(g => g.MainGenre)
				.Include(g => g.GamePlatforms)
				.ThenInclude(gp => gp.Platform)
				.Include(g => g.SideGenres)
				.ThenInclude(gg => gg.Genre)
				.FirstOrDefaultAsync();

			if (game != null)
			{
				GameDetailsViewModel gameDetailsModel = new GameDetailsViewModel
				{
					Id = game.Id.ToString(),
					Title = game.Title,
					Description = game.Description,
					ImageUrl = game.ImageUrl,
					Developer = game.Developer,
					LeadDirector = game.LeadGameDirector,
					MainGenre = game.MainGenre,
					ReleasedDate = game.ReleaseDate,
					SubGenres = game.SideGenres,
					PlatformsList = game.GamePlatforms
				};
				return gameDetailsModel;
			}

			return null;
		}

		public async Task<AddGameInputModel> GetInputGameModelAsync()
		{
			IEnumerable<Genre> genresList = genreRepository.GetAllAttached();
			IEnumerable<Developer> devList = devRepository.GetAllAttached();
			IEnumerable<Director> directorList = dirRepository.GetAllAttached();

			List<GenreViewModel> genreModelsList = genresList
				.Select(x => (new GenreViewModel
				{
					Id = x.Id,
					Name = x.Name,
				}))
				.ToList();

			List<DeveloperViewModel> devModelsList = devList
				.Select(x => (new DeveloperViewModel
				{
					Id = x.Id,
					Name = x.Name,

				}))
				.ToList();

			List<DirectorViewModel> directorModelsList = directorList
				.Select(x => (new DirectorViewModel
				{
					Id = x.Id,
					Name = x.Name,
				}))
				.ToList();

			AddGameInputModel gameModel = new AddGameInputModel()
			{
				Genres = genreModelsList,
				Developers = devModelsList,
				Directors = directorModelsList
			};

			return gameModel;
		}

		public async Task<AddGameInputModel> GetInputGameModelAsync(AddGameInputModel gameModel)
		{
			AddGameInputModel gameLists = await GetInputGameModelAsync();

			gameModel.Genres = gameLists.Genres;
			gameModel.Developers = gameLists.Developers;
			gameModel.Directors = gameLists.Directors;

			return gameModel;
		}

		public async Task<AddPlatformsToGameInputModel?> GetInputPlatformsToGameModelAsync(string id)
		{
			Game? game = await GetGameByIdAsync(id);
			AddPlatformsToGameInputModel? viewModel = null;

			if (game != null)
			{
				viewModel = new AddPlatformsToGameInputModel()
				{
					Id = id,
					Title = game.Title,

					Platforms = await platformRepository
						.GetAllAttached()
						.Include(p => p.GamesPlatform)
						.ThenInclude(gp => gp.Game)
						.Select(p => new PlatformCheckBoxInputModel()
						{
							Id = p.Id.ToString(),
							Name = p.Name,
							IsSelected = p.GamesPlatform
								.Any(gp => gp.GameId == Guid.Parse(id) && gp.IsDeleted == false),

							LinkToPlatform = gamePlatformRepository
								.GetAllAttached()
								.Where(gp => gp.GameId == Guid.Parse(id) &&
														gp.PlatformId == p.Id)
								.Select(gp => gp.LinkToPlatform)
								.FirstOrDefault()
						})
						.ToListAsync()
				};

			}

			return viewModel;
		}

		public async Task<AddSubGenresToGameInputModel?> GetInputGenresToGameModelAsync(string id)
		{
			Game? game = await GetGameByIdAsync(id);
			AddSubGenresToGameInputModel? genresViewModel = null;

			if (game != null)
			{
				genresViewModel = new AddSubGenresToGameInputModel()
				{
					Id = id,
					Title = game.Title,

					SubGenres = await genreRepository
						.GetAllAttached()
						.Include(g => g.GamesGenre)
						.ThenInclude(gg => gg.Game)
						.Where(g => g.Id != game.MainGenreId)
						.Select(g => new GenreCheckBoxInputModel()
						{
							Id = g.Id.ToString(),
							Name = g.Name,
							IsSelected = g.GamesGenre
								.Any(gg => gg.GameId == game.Id && gg.IsDeleted == false)
						})
						.ToListAsync()

				};
			}

			return genresViewModel;
		}

		public async Task<bool> AddGameAsync(AddGameInputModel model, bool isUserMod)
		{
			var dateTimeString = $"{model.ReleasedDate}";

			DateTime.TryParseExact(dateTimeString, ReleasedDateFormat, CultureInfo.InvariantCulture,
				DateTimeStyles.None, out DateTime parseDateTime);

			model.Title = Sanitize(model.Title);
			model.Description = Sanitize(model.Description);
			model.ImageUrl = Sanitize(model.ImageUrl);

			if (!IsModelValid(model))
			{
				return false;
			}

			var gameData = new Game
			{
				Title = model.Title,
				Description = model.Description,
				ImageUrl = model.ImageUrl,
				ReleaseDate = parseDateTime,
				DeveloperId = Guid.Parse(model.DeveloperId),
				MainGenreId = Guid.Parse(model.MainGenreId),
				LeadGameDirectorId = Guid.Parse(model.LeadGameDirectorId)
			};

			if (isUserMod)
			{
				gameData.IsConfirmed = true;
			}

			await gameRepository.AddAsync(gameData);
			return true;
		}

		public async Task AddPlatformsToGameAsync(AddPlatformsToGameInputModel model)
		{
			Game? game = await GetGameByIdAsync(model.Id);
			if (game == null || game.IsDeleted)
			{
				return;
			}

			List<GamePlatform> platformsToAdd = new List<GamePlatform>();

			foreach (var platform in model.Platforms)
			{
				GamePlatform? gamePlatform = await gamePlatformRepository
					.FirstOrDefaultAsync(gp => gp.GameId == game.Id && gp.PlatformId == Guid.Parse(platform.Id));

				if (platform.IsSelected)
				{
					if (gamePlatform == null)
					{
						platformsToAdd.Add(new GamePlatform()
						{
							GameId = game.Id,
							PlatformId = Guid.Parse(platform.Id),
							LinkToPlatform = Sanitize(platform.LinkToPlatform)
						});
					}
					else
					{
						gamePlatform.IsDeleted = false;
					}
				}
				else
				{
					if (gamePlatform != null)
					{
						gamePlatform.IsDeleted = true;
					}
				}
			}

			await gamePlatformRepository.AddRangeAsync(platformsToAdd.ToArray());
		}

		public async Task AddSubGenresToGameAsync(AddSubGenresToGameInputModel model)
		{
			Game? game = await GetGameByIdAsync(model.Id);
			if (game == null || game.IsDeleted)
			{
				return;
			}

			List<GameGenre> genresToAdd = new List<GameGenre>();

			foreach (var subGenre in model.SubGenres)
			{
				GameGenre? gameGenre = await gameGenreRepository
					.FirstOrDefaultAsync(gg => gg.GameId == game.Id && gg.GenreId == Guid.Parse(subGenre.Id));

				if (subGenre.IsSelected)
				{
					if (gameGenre == null)
					{
						genresToAdd.Add(new GameGenre()
						{
							GameId = game.Id,
							GenreId = Guid.Parse(subGenre.Id)
						});
					}
					else
					{
						gameGenre.IsDeleted = false;
					}
				}
				else
				{
					if (gameGenre != null)
					{
						gameGenre.IsDeleted = true;
					}
				}
			}

			await gameGenreRepository.AddRangeAsync(genresToAdd.ToArray());
		}

		public async Task DeleteAGame(Guid id)
		{
			Game? game = await gameRepository.GetByIdAsync(id);

			if (game != null)
			{
				game.IsDeleted = true;
				await gameRepository.UpdateAsync(game);
			}
		}

		public async Task<List<GameAllViewModel>> GetAllGamesToConfirmAsync()
		{
			var games = await gameRepository.GetAllAttached()
				.Where(g => g.IsConfirmed == false &&
							g.IsDeleted == false)
				.Select(g => new GameAllViewModel()
				{
					Id = g.Id,
					Title = g.Title,
					ImageUrl = g.ImageUrl
				})
				.ToListAsync();

			return games;
		}
	}
}
