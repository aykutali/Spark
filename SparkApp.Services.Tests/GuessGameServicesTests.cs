using MockQueryable;
using Moq;

using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data;
using SparkApp.Services.Data.Interfaces;

namespace SparkApp.Services.Tests
{
	[TestFixture]
	public class GuessGameServicesTests
	{
		private Mock<IRepository<GameOfTheDay, object>> gameOfTheDayRepository;
		private Mock<IRepository<Game, Guid>> gameRepository;

		private DateOnly dateToday = new DateOnly(2024, 05, 04);
		private DateOnly dateYesterday = new DateOnly(2024, 05, 03);

		private IList<Game> gameData = new List<Game>()
		{
			new Game()
			{
				Id = new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3"),
				Title = "Elden Ring",
				Description = "open world souls game",
				ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202110/2000/phvVT0qZfcRms5qDAk0SI3CM.png",
				ReleaseDate = new DateTime(2022, 02, 25),
				LeadGameDirectorId = new Guid("9B89E6C5-2893-429A-8B65-08DD000CE7B6"),
				DeveloperId = new Guid("EE33DDCF-011E-4266-9811-A778A31223DB"),
				MainGenreId = new Guid("69DDED8C-B117-4616-AE9C-10F763B26669"),
				IsConfirmed = true,
				IsDeleted = false,
			},
			new Game()
			{
				Id = new Guid("6B410ED2-44BD-496F-8A22-AD70EF8973BB"),
				Title = "Bloodborne",
				Description = "soul but with beast and blood",
				ImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202110/2000/phvVT0qZfcRms5qDAk0SI3CM.png",
				ReleaseDate = new DateTime(2016, 07, 20),
				LeadGameDirectorId = new Guid("9B89E6C5-2893-429A-8B65-08DD000CE7B6"),
				DeveloperId = new Guid("EE33DDCF-011E-4266-9811-A778A31223DB"),
				MainGenreId = new Guid("69DDED8C-B117-4616-AE9C-10F763B26669"),
				IsConfirmed = true,
				IsDeleted = false,
			}
		};
		private IList<Developer> devData = new List<Developer>()
		{
			new Developer()
			{
				Id = new Guid("EE33DDCF-011E-4266-9811-A778A31223DB"),
				Name = "FromSoftware",
			}
		};

		private IList<Genre> genreData = new List<Genre>()
		{
			new Genre()
			{
				Id = new Guid("69DDED8C-B117-4616-AE9C-10F763B26669"),
				Name = "Souls-like",
				Description = "git gud more"
			}
		};

		private IList<Genre> subGenresData = new List<Genre>()
		{
			new Genre()
			{
				Id = new Guid("3EDAE636-CDB6-4995-8707-EF0A69F2733C"),
				Name = "Action-Adventure",
				Description = "smash and explore",
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
		};

		private IList<Director> dirData = new List<Director>()
		{
			new Director()
			{
				Id = new Guid("9B89E6C5-2893-429A-8B65-08DD000CE7B6"),
				Name = "Hidetaka Miyazaki",
				About = "Creator of the Souls games and \"souls\"-\"souls-like\" genres",
				ImageUrl =
					"https://i.namu.wiki/i/qBSmQPJjYdPqrnDue2wc7H_44TEHRF3-l4e31U0iPUnGxd7vAmZnffhsynOvYckzPWHjyK1hrDVeFpeMtprgQA.webp"
			}
		};

		private IList<Platform> platformsData = new List<Platform>()
		{
			new Platform()
			{
				Id = new Guid("F4A5C42C-5063-4CE2-6C84-08DD0012741D"),
				Name = "PS5"
			},
			new Platform()
			{
				Id= new Guid("427715C3-7A02-4B6D-CB61-08DD041E6403"),
				Name = "PC"
			},
		};


		[SetUp]
		public void SetUp()
		{
			this.gameRepository = new Mock<IRepository<Game, Guid>>();
			this.gameOfTheDayRepository = new Mock<IRepository<GameOfTheDay, object>>();
		}

		[Test]
		[TestCase("today")]
		[TestCase("yesterday")]
		public async Task IsGetGameOfTheDayAsyncWorkCorrect(string whichDate)
		{
			DateOnly date;
			if (whichDate == "today")
			{
				date = dateToday;
			}
			else
			{
				date = dateYesterday;
			}

			IList<GameOfTheDay> gotdData = new List<GameOfTheDay>()
			{
				new GameOfTheDay()//game of the day today
				{
					Day = dateToday,
					GameId = gameData[0].Id,
					TheGame = gameData[0]
				},
				new GameOfTheDay()// game of the day from yesterday
				{
					Day = dateYesterday,
					GameId = gameData[1].Id,
					TheGame = gameData[1]
				}
			};

			IQueryable<GameOfTheDay> gotdMockQueryable = gotdData.BuildMock();
			this.gameOfTheDayRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gotdMockQueryable);

			IQueryable<Game> gamesMockQueryable = gameData.BuildMock();
			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gamesMockQueryable);

			IGuessGameService guessGameService = new GuessGameService(gameRepository.Object,
																	  gameOfTheDayRepository.Object);

			var resultActual = await guessGameService.GetGameOfTheDayAsync(date);

			if (whichDate == "today")
			{
				Assert.AreEqual(this.gameData[0].Id, resultActual.Id);
			}
			else
			{
				Assert.AreEqual(this.gameData[1].Id, resultActual.Id);
			}
		}

		[Test]
		[TestCase("newDay")]
		[TestCase("existDay")]
		public async Task IsSetGameOfTheDayAsyncWorkCorrect(string whichDate)
		{
			DateOnly date;
			if (whichDate == "newDay")
			{
				date = new DateOnly(2024, 05, 05);
			}
			else
			{
				date = new DateOnly(2024, 05, 04);
			}

			IList<GameOfTheDay> gotdData = new List<GameOfTheDay>()
			{
				new GameOfTheDay()//game of the day today
				{
					Day = dateToday,
					GameId = gameData[0].Id,
					TheGame = gameData[0]
				},
				new GameOfTheDay()// game of the day from yesterday
				{
					Day = dateYesterday,
					GameId = gameData[1].Id,
					TheGame = gameData[1]
				}
			};

			IQueryable<GameOfTheDay> gotdMockQueryable = gotdData.BuildMock();
			this.gameOfTheDayRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gotdMockQueryable);

			IQueryable<Game> gamesMockQueryable = gameData.BuildMock();
			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gamesMockQueryable);

			IGuessGameService guessGameService = new GuessGameService(gameRepository.Object,
				gameOfTheDayRepository.Object);

			bool result = await guessGameService.SetGameOfTheDayAsync(date);

			if (whichDate == "newDay")
			{
				Assert.IsTrue(result);
			}
			else
			{
				Assert.IsFalse(result);
			}
		}

		[Test]
		[TestCase("Elden Ring")]
		[TestCase("Bloodborne")]
		[TestCase("invalid")]
		public async Task IsGuessGameAsyncWorkCorrect(string title)
		{
			DateOnly date = this.dateToday;

			foreach (var game in gameData)
			{
				game.Developer = devData[0];
				game.MainGenre = genreData[0];
				game.LeadGameDirector = dirData[0];
				game.GamePlatforms = new List<GamePlatform>()
				{
					new GamePlatform()
					{
						Game = game,
						GameId = game.Id,
						Platform = platformsData[0],
						PlatformId = platformsData[0].Id
					}
				};
				game.SideGenres = new List<GameGenre>()
				{
					new GameGenre()
					{
						Game = game,
						GameId = game.Id,
						Genre = subGenresData[0],
						GenreId = subGenresData[0].Id
					}
				};
			}

			IList<GameOfTheDay> gotdData = new List<GameOfTheDay>()
			{
				new GameOfTheDay()//game of the day today
				{
					Day = dateToday,
					GameId = gameData[0].Id,
					TheGame = gameData[0]
				},
				new GameOfTheDay()// game of the day from yesterday
				{
					Day = dateYesterday,
					GameId = gameData[1].Id,
					TheGame = gameData[1]
				}
			};

			IQueryable<GameOfTheDay> gotdMockQueryable = gotdData.BuildMock();
			this.gameOfTheDayRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gotdMockQueryable);

			IQueryable<Game> gamesMockQueryable = gameData.BuildMock();
			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gamesMockQueryable);

			IGuessGameService guessGameService = new GuessGameService(gameRepository.Object,
				gameOfTheDayRepository.Object);

			var resultActual = await guessGameService.GuessGameAsync(title, date);

			if (title == "invalid")
			{
				Assert.IsNull(resultActual);
			}
			else if (title == "Elden Ring")
			{
				Assert.IsNotNull(resultActual);
				Assert.AreEqual(true, resultActual.IsGuessIsCorrect);
			}
			else if (title == "Bloodborne")
			{
				Assert.IsNotNull(resultActual);
				Assert.AreEqual(false, resultActual.IsGuessIsCorrect);
			}
		}

		[Test]
		public async Task IsGetGameTitleFromDayBeforeWorkCorrect()
		{
			IList<GameOfTheDay> gotdData = new List<GameOfTheDay>()
			{
				new GameOfTheDay()//game of the day today
				{
					Day = DateOnly.FromDateTime(DateTime.Now),
					GameId = gameData[0].Id,
					TheGame = gameData[0]
				},
				new GameOfTheDay()// game of the day from yesterday
				{
					Day = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1))),
					GameId = gameData[1].Id,
					TheGame = gameData[1]
				}
			};

			IQueryable<GameOfTheDay> gotdMockQueryable = gotdData.BuildMock();
			this.gameOfTheDayRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gotdMockQueryable);

			IQueryable<Game> gamesMockQueryable = gameData.BuildMock();
			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gamesMockQueryable);

			IGuessGameService guessGameService = new GuessGameService(gameRepository.Object,
				gameOfTheDayRepository.Object);

			string titleExpected = gameData[1].Title;
			string titleActual = await guessGameService.GetGameTitleFromDayBefore();

			Assert.AreEqual(titleExpected, titleActual);
		}
	}
}
