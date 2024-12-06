using MockQueryable;
using Moq;

using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Director;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Tests
{
	[TestFixture]
	public class DirectorServicesTests
	{
		private Mock<IRepository<Director, Guid>> dirRepository;

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
			this.dirRepository = new Mock<IRepository<Director, Guid>>();
		}

		[Test]
		public async Task IsGetAllAsyncWorkCorrect()
		{
			IQueryable<Director> dirMockQueryable = dirData.BuildMock();
			this.dirRepository
				.Setup(r => r.GetAllAttached())
				.Returns(dirMockQueryable);

			IDirectorService dirService = new DirectorService(dirRepository.Object);

			var directorsActual = await dirService.GetAllAsync();

			Assert.AreEqual(this.dirData.Count(), directorsActual.Count());
		}

		[Test]
		[TestCase("Hidetaka Miyazaki")]
		[TestCase("invalid")]
		public async Task IsGetDetailsWorkCorrect(string name)
		{
			dirData[0].Games.Add(gameData[0]);

			IQueryable<Director> directorsMockQueryable = dirData.BuildMock();
			this.dirRepository
				.Setup(r => r.GetAllAttached())
				.Returns(directorsMockQueryable);

			IDirectorService dirService = new DirectorService(dirRepository.Object);

			var directorActual = await dirService.GetDirectorDetailsAsync(name);

			if (name == "invalid")
			{
				Assert.IsNull(directorActual);
			}
			else
			{
				Assert.AreEqual(name, directorActual.Name);
				Assert.NotNull(directorActual.Games);
			}
		}

		[Test]
		[TestCase("Hidetaka Miyazaki")] // name which already exist
		[TestCase("Some Name")]
		public async Task IsAddDirectorAsyncWorkCorrect(string name)
		{
			IQueryable<Director> directorsMockQueryable = dirData.BuildMock();
			this.dirRepository
				.Setup(r => r.GetAllAttached())
				.Returns(directorsMockQueryable);

			IDirectorService dirService = new DirectorService(dirRepository.Object);

			var result = await dirService.AddDirectorAsync(new AddDirectorInputModel()
			{
				Name = name,
				About = "Some thing about some director"
			});

			if (name == "Some Name")
			{
				Assert.IsTrue(result);
			}
			else
			{
				Assert.IsFalse(result);
			}
		}
	}
}
