
using Moq;

using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data;
using SparkApp.Services.Data.Interfaces;

namespace SparkApp.Tests
{
	[TestFixture]
	public class GameServiceTests
	{
		private IGameService gameService;

		[SetUp]
		public void TestInitialize()
		{
			var gameRepo = new Mock<IRepository<Game, Guid>>();
			var genreRepo = new Mock<IRepository<Genre, Guid>>();
			var platformRepo = new Mock<IRepository<Platform, Guid>>();
			var devRepo = new Mock<IRepository<Developer, Guid>>();
			var dirRepo = new Mock<IRepository<Director, Guid>>();
			var gameGenreRepo = new Mock<IRepository<GameGenre, object>>();
			var gamePlatformRepo = new Mock<IRepository<GamePlatform, object>>();

			this.gameService = new GameService(gameRepo.Object,
				dirRepo.Object,
				genreRepo.Object,
				devRepo.Object,
				platformRepo.Object,
				gamePlatformRepo.Object,
				gameGenreRepo.Object);
		}

		[Test]
		public async Task GetGameFromService()
		{
			var game = await gameService.GetAllGamesAsync();
			var gameList = game.ToList();

			Assert.That(gameList, Is.Empty);

		}
	}
}
