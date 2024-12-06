using Castle.Components.DictionaryAdapter.Xml;
using MockQueryable;
using Moq;

using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Tests
{
	[TestFixture]
	public class GenreServicesTests
	{
		private Mock<IRepository<Genre, Guid>> genreRepository;
		private Mock<IRepository<Game, Guid>> gameRepository;

		private IList<Genre> genreData = new List<Genre>()
		{
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
			}
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
				IsConfirmed = true,
				IsDeleted = false,
			}
		};

		[SetUp]
		public void SetUp()
		{
			this.genreRepository = new Mock<IRepository<Genre, Guid>>();
			this.gameRepository = new Mock<IRepository<Game, Guid>>();
		}

		[Test]
		public async Task IsGetAllAsyncWorkCorrect()
		{
			IQueryable<Genre> genresMockQueryable = genreData.BuildMock();
			this.genreRepository
				.Setup(r => r.GetAllAttached())
				.Returns(genresMockQueryable);

			IGenreService genreService = new GenreService(genreRepository.Object, gameRepository.Object);

			IEnumerable<GenreViewModel>? genresActual = await genreService.GetAllAsync();

			Assert.AreEqual(this.genreData.Count(), genresActual.Count());
		}

		[Test]
		[TestCase("Metroidvania")]
		[TestCase("Open world")]
		[TestCase("Souls-like")]
		[TestCase("invalid")]
		public async Task IsGetDetailsWorkCorrect(string name)
		{
			IQueryable<Genre> genresMockQueryable = genreData.BuildMock();
			this.genreRepository
				.Setup(r => r.GetAllAttached())
				.Returns(genresMockQueryable);

			IQueryable<Game> gamesMockQueryable = gameData.BuildMock();
			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gamesMockQueryable);

			IGenreService genreService = new GenreService(genreRepository.Object, gameRepository.Object);

			GenreDetailsViewModel? genreModel = await genreService.GetGenreDetailsAsync(name);

			if (name == "invalid")
			{
				Assert.IsNull(genreModel);
			}
			else
			{
				Assert.AreEqual(name, genreModel.Name);
			}
		}

		[Test]

		public async Task IsAddGenreWorkCorrect()
		{

			IQueryable<Genre> genreMockQueryable = genreData.BuildMock();
			this.genreRepository
				.Setup(r => r.GetAllAttached())
				.Returns(genreMockQueryable);

			IQueryable<Game> gamesMockQueryable = gameData.BuildMock();
			this.gameRepository
				.Setup(r => r.GetAllAttached())
				.Returns(gamesMockQueryable);

			IGenreService genreService = new GenreService(genreRepository.Object, gameRepository.Object);

			var result = genreService.AddGenreAsync(new AddGenreInputModel()
			{
				Name = "Some Genre",
				Description = "Some Genre Description"
			}).IsCompletedSuccessfully;

			Assert.AreEqual(true, result);
		}
	}
}
