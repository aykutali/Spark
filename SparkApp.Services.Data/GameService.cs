
using static SparkApp.Common.EntityValidationConstants.Game;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Director;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Genre;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SparkApp.Data;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Services.Data
{
	public class GameService : BaseService, IGameService
	{
		private readonly SparkDbContext db;
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
						   IRepository<GameGenre, object> gameGenreRepository,
						   SparkDbContext db)
		{
			this.gameRepository = gameRepository;
			this.dirRepository = dirRepository;
			this.genreRepository = genreRepository;
			this.devRepository = devRepository;
			this.platformRepository = platformRepository;
			this.gamePlatformRepository = gamePlatformRepository;
			this.gameGenreRepository = gameGenreRepository;
			this.db = db;
		}

		public async Task<List<GameAllViewModel>> GetAllGamesAsync()
		{
			List<GameAllViewModel> games = await gameRepository.GetAllAttached()
				.Select(g => new GameAllViewModel
				{
					Id = g.Id,
					Title = g.Title,
					ImageUrl = g.ImageUrl
				})
				.ToListAsync();

			return games;
		}

		public async Task<Game> GetGameByIdAsync(string id)
		{
			Game gameData = await gameRepository.GetByIdAsync(Guid.Parse(id));

			return gameData;
		}

		public async Task<GameEditViewModel> GetEditGameModelAsync(string id)
		{
			Game gameData = await gameRepository.GetByIdAsync(Guid.Parse(id));
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
				.Where(g => g.Title == title)
				.Include(g => g.LeadGameDirector)
				.Include(g => g.Developer)
				.Include(g => g.MainGenre)
				.Include(g=>g.GamePlatforms)
				.ThenInclude(gp=>gp.Platform)
				.FirstOrDefaultAsync();

			var genres = await db.GamesGenres
				.Where(gg => gg.Game.Title == title)
				.Select(gg => gg.Genre)
				.ToListAsync();

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
					SubGenres = genres,
					PlatformsList = game.GamePlatforms
				};
				return gameDetailsModel;
			}

			return null;
		}

		public async Task<AddGameInputModel> GetInputGameModelAsync()
		{
			IEnumerable<Genre> genresList = await genreRepository.GetAllAsync();
			IEnumerable<Developer> devList = await devRepository.GetAllAsync();
			IEnumerable<Director> directorList = await dirRepository.GetAllAsync();

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
			Game? game = await gameRepository.GetByIdAsync(Guid.Parse(id));
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

							LinkToPlatform = p.GamesPlatform
								.Where(gp => gp.GameId == Guid.Parse(id))
								.Select(gp => gp.LinkToPlatform)
								.First()
						})
						.ToListAsync()
				};

			}

			return viewModel;
		}

		public async Task AddGameAsync(AddGameInputModel model)
		{
			var dateTimeString = $"{model.ReleasedDate}";

			DateTime.TryParseExact(dateTimeString, ReleasedDateFormat, CultureInfo.InvariantCulture,
				DateTimeStyles.None, out DateTime parseDateTime);

			var gameData = new Game
			{
				Title = model.Title,
				Description = model.Description,
				ImageUrl = model.ImageUrl,
				ReleaseDate = parseDateTime,
				DeveloperId = new Guid(model.DeveloperId),
				MainGenreId = new Guid(model.MainGenreId),
				LeadGameDirectorId = new Guid(model.LeadGameDirectorId)
			};

			await gameRepository.AddAsync(gameData);
		}

		public async Task AddPlatformsToGameAsync(AddPlatformsToGameInputModel model)
		{
			List<GamePlatform> platformsToAdd = new List<GamePlatform>();
			foreach (var platform in model.Platforms)
			{
				Game? game = await gameRepository.GetByIdAsync(Guid.Parse(model.Id));
				if (game == null || game.IsDeleted)
				{
					return;
				}

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
							LinkToPlatform = platform.LinkToPlatform

						});
					}
					else
					{
						gamePlatform.IsDeleted = false;
					}
				}
				else
				{
					if (gamePlatform!=null)
					{
						gamePlatform.IsDeleted=true;
					}
				}
			}

			await gamePlatformRepository.AddRangeAsync(platformsToAdd.ToArray());
		}
	}
}
