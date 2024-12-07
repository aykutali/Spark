
using Microsoft.EntityFrameworkCore;
using SparkApp.Data;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

using static SparkApp.Common.EntityValidationConstants.Game;

namespace SparkApp.Services.Data
{
	public class GuessGameService : BaseService, IGuessGameService
	{
		private readonly IRepository<Game, Guid> gameRepository;
		private readonly IRepository<GameOfTheDay, object> gameOfTheDayRepository;

		public GuessGameService(IRepository<Game, Guid> gameRepository,
								IRepository<GameOfTheDay, object> gameOfTheDayRepository)
		{
			this.gameRepository = gameRepository;
			this.gameOfTheDayRepository = gameOfTheDayRepository;
		}
		/// <summary>
		/// Set a game for a day, if is not already exist
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public async Task<bool> SetGameOfTheDayAsync(DateOnly date)
		{
			bool isGameOfTodayExist = await gameOfTheDayRepository.GetAllAttached()
				.AnyAsync(dg => dg.Day == date);

			if (isGameOfTodayExist == false)
			{
				List<Game> allGames = await gameRepository.GetAllAttached().ToListAsync();
				Random rnd = new Random();

				int randomIndex = rnd.Next(allGames.Count);
				Game randomGame = allGames[randomIndex];

				GameOfTheDay gameOfTheDayData = new GameOfTheDay()
				{
					Day = date,
					GameId = randomGame.Id
				};

				await gameOfTheDayRepository.AddAsync(gameOfTheDayData);
				return true;
			}

			return false;
		}

		public async Task<Game> GetGameOfTheDayAsync(DateOnly date)
		{
			GameOfTheDay? gameOfTheDayData = await gameOfTheDayRepository.GetAllAttached()
				.Where(dg => dg.Day == date)
				.FirstOrDefaultAsync();

			Game? gameOfTheDay = await gameRepository.GetAllAttached()
				.Where(g => g.IsConfirmed &&
							g.IsDeleted == false &&
							g.Id == gameOfTheDayData.GameId)
				.Include(g => g.Developer)
				.Include(g => g.LeadGameDirector)
				.Include(g => g.MainGenre)
				.Include(g => g.GamePlatforms)
				.ThenInclude(gp => gp.Platform)
				.Include(g => g.SideGenres)
				.ThenInclude(gg => gg.Genre)
				.FirstOrDefaultAsync();

			return gameOfTheDay;
		}
		/// <summary>
		/// get title of the game of the day from yesterday and set a game for today if is not already
		/// </summary>
		/// <returns></returns>
		public async Task<string> GetGameTitleFromDayBefore()
		{
			DateTime today = DateTime.Now.Date;

			await SetGameOfTheDayAsync(DateOnly.FromDateTime(today));

			DateTime dateBefore = today.Subtract(TimeSpan.FromDays(1));
			DateOnly yesterday = DateOnly.FromDateTime(dateBefore);

			string gameTitle = await gameOfTheDayRepository.GetAllAttached()
				.Where(d=> d.Day == yesterday)
				.Include(d=> d.TheGame)
				.Select(g=> g.TheGame.Title)
				.FirstOrDefaultAsync();

			return gameTitle;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTitle"></param>
		/// <returns>Return a GuessTheGameViewModel or null if the guessed gameTitle is modified</returns>
		public async Task<GuessTheGameViewModel> GuessGameAsync(string gameTitle, DateOnly date)
		{
			Game gameOfTheDay = await GetGameOfTheDayAsync(date);

			Game? guessedGame = await gameRepository.GetAllAttached()
				.Where(g => g.IsConfirmed &&
							g.IsDeleted == false &&
							g.Title == gameTitle)
				.Include(g => g.Developer)
				.Include(g => g.LeadGameDirector)
				.Include(g => g.MainGenre)
				.Include(g => g.GamePlatforms)
				.ThenInclude(gp => gp.Platform)
				.Include(g => g.SideGenres)
				.ThenInclude(gg => gg.Genre)
				.FirstOrDefaultAsync();


			if (guessedGame != null && gameOfTheDay != null)
			{
				var titleDic = new Dictionary<string, char>();
				char titleChar;
				if (guessedGame.Title == gameOfTheDay.Title)
				{
					titleChar = '+';
				}
				else
				{
					titleChar = '-';
				}
				titleDic.Add(guessedGame.Title, titleChar);
				//
				var releaseDic = new Dictionary<string, char>();
				char releaseChar;
				if (guessedGame.ReleaseDate == gameOfTheDay.ReleaseDate)
				{
					releaseChar = '+';
				}
				else if (guessedGame.ReleaseDate > gameOfTheDay.ReleaseDate)
				{
					releaseChar = '-';
				}
				else
				{
					releaseChar = '*';
				}
				releaseDic.Add(guessedGame.ReleaseDate.ToString(ReleasedDateFormat), releaseChar);
				//
				var devDic = new Dictionary<string, char>();
				char devChar;
				if (guessedGame.DeveloperId == gameOfTheDay.DeveloperId)
				{
					devChar = '+';
				}
				else
				{
					devChar = '-';
				}
				devDic.Add(guessedGame.Developer.Name, devChar);
				//
				var dirDic = new Dictionary<string, char>();
				char dirChar;
				if (guessedGame.LeadGameDirectorId == gameOfTheDay.LeadGameDirectorId)
				{
					dirChar = '+';
				}
				else
				{
					dirChar = '-';
				}
				dirDic.Add(guessedGame.LeadGameDirector.Name, dirChar);
				//
				var mainGenreDic = new Dictionary<string, char>();
				char mainGenreChar;
				if (guessedGame.MainGenreId == gameOfTheDay.MainGenreId)
				{
					mainGenreChar = '+';
				}
				else
				{
					mainGenreChar = '-';
				}
				mainGenreDic.Add(guessedGame.MainGenre.Name, mainGenreChar);
				//
				var platformsDic = new Dictionary<string, char>();
				char platformsChar;

				List<string> guessGamePlatformsList = guessedGame.GamePlatforms
					.Where(gp => gp.IsDeleted == false)
					.Select(gp => gp.Platform.Name).ToList();

				List<string> gameOfTheDayPlatformsList = gameOfTheDay.GamePlatforms
					.Where(gp => gp.IsDeleted == false)
					.Select(gp => gp.Platform.Name).ToList();

				int correctCount = 0;

				foreach (var p in guessGamePlatformsList)
				{
					if (gameOfTheDayPlatformsList.Any(pl => pl == p))
					{
						correctCount++;
					}
				}

				if (correctCount == 0)
				{
					platformsChar = '-';
				}
				else if (correctCount == gameOfTheDayPlatformsList.Count)
				{
					platformsChar = '+';
				}
				else
				{
					platformsChar = '*';
				}

				platformsDic.Add(string.Join(", ", guessGamePlatformsList), platformsChar);
				//
				var subGenresDic = new Dictionary<string, char>();
				char subGenresChar;

				List<string> guessGameSubGenres = guessedGame.SideGenres
					.Where(sg => sg.IsDeleted == false)
					.Select(sg => sg.Genre.Name).ToList();
				List<string> gameOfTheDaySubGenres = gameOfTheDay.SideGenres
					.Where(sg => sg.IsDeleted == false)
					.Select(sg => sg.Genre.Name).ToList();

				correctCount = 0;

				foreach (var sg in guessGameSubGenres)
				{
					if (gameOfTheDaySubGenres.Any(subg => subg == sg))
					{
						correctCount++;
					}
				}
				if (gameOfTheDaySubGenres.Count == 0 && guessGameSubGenres.Count == 0)
				{
					subGenresChar = '+';
				}
				else if (correctCount == 0)
				{
					subGenresChar = '-';
				}
				else if (correctCount == gameOfTheDaySubGenres.Count)
				{
					subGenresChar = '+';
				}
				else
				{
					subGenresChar = '*';
				}


				subGenresDic.Add(string.Join(", ", guessGameSubGenres), subGenresChar);
				//
				var viewModel = new GuessTheGameViewModel()
				{
					Title = titleDic,
					ImageUrl = guessedGame.ImageUrl,
					ReleaseDate = releaseDic,
					Developer = devDic,
					LeadGameDirector = dirDic,
					MainGenre = mainGenreDic,
					Platforms = platformsDic,
					SubGenres = subGenresDic,
					IsGuessIsCorrect = guessedGame.Id == gameOfTheDay.Id
				};

				return viewModel;
			}

			return null;
		}
	}
}
