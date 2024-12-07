using MockQueryable;
using Moq;

using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Tests
{
	[TestFixture]
	public class DeveloperServicesTests
	{
		private Mock<IRepository<Developer, Guid>> devRepository;

		private IList<Developer> devData = new List<Developer>()
		{

			new Developer()
			{
				Id = new Guid("EE33DDCF-011E-4266-9811-A778A31223DB"),
				Name = "FromSoftware",
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
			this.devRepository = new Mock<IRepository<Developer, Guid>>();
		}

		[Test]
		public async Task IsGetAllAsyncWorkCorrect()
		{
			IQueryable<Developer> devMockQueryable = devData.BuildMock();
			this.devRepository
				.Setup(r => r.GetAllAttached())
				.Returns(devMockQueryable);

			IDeveloperService devService = new DeveloperService(devRepository.Object);

			var devActual = await devService.GetAllAsync();

			Assert.AreEqual(this.devData.Count(), devActual.Count());
		}

		[Test]
		[TestCase("FromSoftware")]
		[TestCase("invalid")]
		public async Task IsGetDetailsWorkCorrect(string name)
		{
			devData[0].Games.Add(gameData[0]);

			IQueryable<Developer> devMockQueryable = devData.BuildMock();
			this.devRepository
				.Setup(r => r.GetAllAttached())
				.Returns(devMockQueryable);

			IDeveloperService devService = new DeveloperService(devRepository.Object);

			var devDetailsActual = await devService.GetDeveloperDetailsAsync(name);

			if (name == "invalid")
			{
				Assert.IsNull(devDetailsActual);
			}
			else
			{
				Assert.AreEqual(devData[0].Id.ToString().ToLower(), devDetailsActual.Id.ToLower());
				Assert.NotNull(devDetailsActual.Games);
			}

		}

		[Test]
		[TestCase("Some Dev")]
		[TestCase("FromSoftware")] //name which is already exist
		public async Task IsAddDeveloperWorkCorrect(string name)
		{
			IQueryable<Developer> devMockQueryable = devData.BuildMock();
			this.devRepository
				.Setup(r => r.GetAllAttached())
				.Returns(devMockQueryable);

			IDeveloperService devService = new DeveloperService(devRepository.Object);

			bool resultActual = await devService.AddDeveloperAsync(new AddDeveloperInputModel()
			{
				Name = name
			});

			if (name == "FromSoftware")
			{
				Assert.IsFalse(resultActual);
			}
			else
			{
				Assert.IsTrue(resultActual);
			}
		}
	}
}

