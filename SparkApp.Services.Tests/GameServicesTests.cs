using Microsoft.EntityFrameworkCore.Query.Internal;
using MockQueryable;
using Moq;

using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Services.Tests
{
	[TestFixture]
	public class GameServicesTests
	{
		private Mock<IRepository<Game, Guid>> gameRepository;
		private Mock<IRepository<Developer, Guid>> devRepository;
		private Mock<IRepository<Director, Guid>> dirRepository;
		private Mock<IRepository<Genre, Guid>> genreRepository;
		private Mock<IRepository<Platform, Guid>> platformRepository;
		private Mock<IRepository<GamePlatform, object>> gamePlatformRepository;
		private Mock<IRepository<GameGenre, object>> gameGenreRepository;

		private IList<Genre> genreData = new List<Genre>()
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
			new Genre()
			{
				Id = new Guid("69DDED8C-B117-4616-AE9C-10F763B26669"),
				Name = "Souls-like",
				Description = "git gud more"
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
				IsConfirmed = false,
				IsDeleted = false,
			}
		};

		private IList<GamePlatform> gpData = new List<GamePlatform>()
		{
			new GamePlatform()
			{
				GameId = new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3"),
				PlatformId = new Guid("F4A5C42C-5063-4CE2-6C84-08DD0012741D"),
				LinkToPlatform = "somelink.com"
			}
		};

		[SetUp]
		public void Setup()
		{
			this.gameRepository = new Mock<IRepository<Game, Guid>>();
			this.devRepository = new Mock<IRepository<Developer, Guid>>();
			this.dirRepository = new Mock<IRepository<Director, Guid>>();
			this.genreRepository = new Mock<IRepository<Genre, Guid>>();
			this.platformRepository = new Mock<IRepository<Platform, Guid>>();
			this.gamePlatformRepository = new Mock<IRepository<GamePlatform, object>>();
			this.gameGenreRepository = new Mock<IRepository<GameGenre, object>>();
		}

		[Test]
		public async Task IsGetGameByIdAsyncReturnCorrectPositive()
		{
			Guid gameGuid = new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3");

			Game gameMock = gameData[0];
			this.gameRepository
				.Setup(r => r.GetByIdAsync(gameGuid))
				.ReturnsAsync(gameMock);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var gamesActual = await gameService.GetGameByIdAsync(gameGuid.ToString());

			Assert.IsNotNull(gamesActual);

			Assert.AreEqual(gameMock.Id, gamesActual.Id);
		}

		[Test]
		public async Task IsGetGameByIdAsyncReturnsNullWhenGuidIsNotCorrect()
		{
			Guid gameGuid = new Guid("9128813E-A6DA-47E5-831C-1A3400915FA5");

			Game gameMock = new Game();
			this.gameRepository
				.Setup(r => r.GetByIdAsync(gameGuid))
				.ReturnsAsync(gameMock);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var gamesActual = await gameService.GetGameByIdAsync(gameGuid.ToString());

			Assert.IsNull(gamesActual.Title);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsGetAllGamesAsyncReturns(bool isConfirmed)
		{
			gameData[0].IsConfirmed = isConfirmed;
			gameData[0].IsDeleted = false;

			IQueryable<Game> allGamesQueryable = gameData.BuildMock();

			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(allGamesQueryable);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var gamesActual = await gameService.GetAllGamesAsync();
			if (isConfirmed == true)
			{
				Assert.AreEqual(gameData.Count(), gamesActual.Count());
			}
			else
			{
				Assert.AreEqual(0, gamesActual.Count());
			}
		}

		[Test]
		public async Task IsDeleteAGameWorkCorrect()
		{
			Guid gameGuid = new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3");

			Game gameMock = gameData[0];
			this.gameRepository
				.Setup(r => r.GetByIdAsync(gameGuid))
				.ReturnsAsync(gameMock);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			await gameService.DeleteAGame(gameGuid);

			Assert.AreEqual(true, gameMock.IsDeleted);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsGetAllGamesToConfirmAsyncWorkCorrect(bool isConfirmed)
		{
			gameData[0].IsConfirmed = isConfirmed;

			IQueryable<Game> allGamesQueryable = gameData.BuildMock();

			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(allGamesQueryable);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var actualGamesToConfirm = await gameService.GetAllGamesToConfirmAsync();

			if (isConfirmed == true)
			{
				Assert.AreEqual(0, actualGamesToConfirm.Count());
			}
			else
			{
				Assert.AreEqual(1, actualGamesToConfirm.Count());
			}
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsGetGameDetailsWorkCorrect(bool isConfirmed)
		{
			gameData[0].IsConfirmed = isConfirmed;

			IQueryable<Game> gamesQueryable = gameData.BuildMock();
			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gamesQueryable);


			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var actualGame = await gameService.GetGameDetailsAsync(gameData[0].Title);

			if (isConfirmed == true)
			{
				Assert.AreEqual(gameData[0].Id.ToString().ToLower(), actualGame.Id.ToLower());
			}
			else
			{
				Assert.IsNull(actualGame);
			}
		}

		[Test]
		public async Task IsGetGameInputModelAsyncWorkCorrect()
		{
			IQueryable<Genre> genresMockQueryable = genreData.BuildMock();
			this.genreRepository
				.Setup(r => r.GetAllAttached())
				.Returns(genresMockQueryable);

			IQueryable<Director> dirMockQueryable = dirData.BuildMock();
			this.dirRepository
				.Setup(r => r.GetAllAttached())
				.Returns(dirMockQueryable);

			IQueryable<Developer> devMockQueryable = devData.BuildMock();
			this.devRepository
				.Setup(r => r.GetAllAttached())
				.Returns(devMockQueryable);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var actualResult = await gameService.GetInputGameModelAsync();

			Assert.IsNotNull(actualResult);
			Assert.IsNotNull(actualResult.Developers);
			Assert.IsNotNull(actualResult.Genres);
			Assert.IsNotNull(actualResult.Directors);

			var secondResult = await gameService.GetInputGameModelAsync(actualResult);
			Assert.IsNotNull(secondResult);
			Assert.IsNotNull(secondResult.Genres);
			Assert.IsNotNull(secondResult.Directors);
			Assert.IsNotNull(secondResult.Developers);
		}

		[Test]
		[TestCase("9128813E-A6DA-47E5-831C-1A3400915FA3")]
		[TestCase("invalidGuid")]
		public async Task IsGetGameEditModelAsyncWorkCorrect(string gameId)
		{
			this.gameRepository
				.Setup(r => r.GetByIdAsync(new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3")))
				.ReturnsAsync(gameData[0]);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var actualResult = await gameService.GetEditGameModelAsync(gameId);

			if (gameId == "invalidGuid")
			{
				Assert.IsNull(actualResult);
			}
			else
			{
				Assert.IsNotNull(actualResult);
				Assert.AreEqual(gameData[0].Title, actualResult.Title);

				var secondResult = await gameService.GetEditGameModelAsync(actualResult);
				Assert.IsNotNull(secondResult);
				Assert.IsNotNull(secondResult.Genres);
			}
		}

		[Test]
		[TestCase("9128813E-A6DA-47E5-831C-1A3400915FA3")]
		[TestCase("invalidGuid")]
		public async Task IsGetInputGenresToGameModelAsyncWorkCorrect(string id)
		{
			this.gameRepository
				.Setup(r => r.GetByIdAsync(new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3")))
				.ReturnsAsync(gameData[0]);

			IQueryable<Genre> genresMockQueryable = genreData.BuildMock();
			this.genreRepository
				.Setup(r => r.GetAllAttached())
				.Returns(genresMockQueryable);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var actualResult = await gameService.GetInputGenresToGameModelAsync(id);

			if (id == "invalidGuid")
			{
				Assert.IsNull(actualResult);
			}
			else
			{
				Assert.IsNotNull(actualResult);
				Assert.IsNotNull(actualResult.SubGenres);
			}
		}

		[Test]
		[TestCase("9128813E-A6DA-47E5-831C-1A3400915FA3")]
		[TestCase("invalidGuid")]
		public async Task IsGetInputPlatformsToGameModelAsyncWorkCorrect(string id)
		{
			this.gameRepository
				.Setup(r => r.GetByIdAsync(new Guid("9128813E-A6DA-47E5-831C-1A3400915FA3")))
				.ReturnsAsync(gameData[0]);

			IQueryable<Platform> platformsMockQueryable = platformsData.BuildMock();
			this.platformRepository
				.Setup(r => r.GetAllAttached())
				.Returns(platformsMockQueryable);

			IQueryable<GamePlatform> gpMockQueryable = gpData.BuildMock();
			this.gamePlatformRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gpMockQueryable);


			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			var actualResult = await gameService.GetInputPlatformsToGameModelAsync(id);

			if (id == "invalidGuid")
			{
				Assert.IsNull(actualResult);
			}
			else
			{
				Assert.IsNotNull(actualResult);
				Assert.IsNotNull(actualResult.Platforms);
			}
		}

		[Test]
		[TestCase("validCase")]
		[TestCase("invalidCase")]
		public async Task IsEditGameAsyncWorkCorrect(string caseStatus)
		{
			Game gameMock = gameData[0];
			this.gameRepository
				.Setup(r => r.GetByIdAsync(gameMock.Id))
				.ReturnsAsync(gameMock);

			IGameService gameService = new GameService(gameRepository.Object,
				dirRepository.Object,
				genreRepository.Object,
				devRepository.Object,
				platformRepository.Object,
				gamePlatformRepository.Object,
				gameGenreRepository.Object);

			if (caseStatus == "validCase")
			{
				var gameToEdit = gameData[0];
				var gameEditModel = await gameService.GetEditGameModelAsync(gameToEdit.Id.ToString());

				gameEditModel.Title = "Edited Title";

				await gameService.EditGameAsync(gameToEdit, gameEditModel);

				Assert.AreEqual("Edited Title", gameToEdit.Title);
			}
			else
			{
				var result = gameService.EditGameAsync(null, null).Result;
				Assert.AreEqual(false,result);
			}
		}
	}
}