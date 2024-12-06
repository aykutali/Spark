using Castle.Components.DictionaryAdapter.Xml;
using MockQueryable;
using Moq;

using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Services.Tests
{
	[TestFixture]
	public class PlatformServicesTests
	{
		public Mock<IRepository<Platform, Guid>> platformRepository;

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
				IsConfirmed = true,
				IsDeleted = false,
			}
		};

		[SetUp]
		public void SetUp()
		{
			this.platformRepository = new Mock<IRepository<Platform, Guid>>();
		}

		[Test]
		public async Task IsGetAllPlatformsAsyncWorkCorrect()
		{
			IQueryable<Platform> platformsMockQueryable = platformsData.BuildMock();
			this.platformRepository
				.Setup(r => r.GetAllAttached())
				.Returns(platformsMockQueryable);

			IPlatformService platformService = new PlatformService(platformRepository.Object);

			var platformsActual = await platformService.GetAllAsync();

			Assert.AreEqual(this.platformsData.Count(), platformsActual.Count());
		}

		[Test]
		[TestCase("PS5")]
		[TestCase("PC")]
		[TestCase("invalid")]
		public async Task IsGetPlatformDetailsAsyncWorkCorrect(string name)
		{
			foreach (var platform in platformsData)
			{
				platform.GamesPlatform.Add(new GamePlatform()
				{
					Game = gameData[0],
					GameId = gameData[0].Id,
					Platform = platform,
					PlatformId = platform.Id
				});
			}

			IQueryable<Platform> platformsMockQueryable = platformsData.BuildMock();
			this.platformRepository
				.Setup(r => r.GetAllAttached())
				.Returns(platformsMockQueryable);

			IPlatformService platformService = new PlatformService(platformRepository.Object);

			var platformActual = await platformService.GetPlatformDetailsAsync(name);

			if (name == "invalid")
			{
				Assert.IsNull(platformActual);
			}
			else
			{
				Assert.AreEqual(name, platformActual.Name);
				Assert.NotNull(platformActual.Games);
			}
		}

		[Test]
		[TestCase("XBOX")]
		[TestCase("PS5")] //name which is already exist
		public async Task IsAddPlatformAsyncWorkCorrect(string name)
		{
			IQueryable<Platform> platformQueryable = platformsData.BuildMock();
			this.platformRepository
				.Setup(r => r.GetAllAttached())
				.Returns(platformQueryable);

			IPlatformService platformService = new PlatformService(platformRepository.Object);

			if (name == "PS5")
			{
				var result = await platformService.AddPlatformAsync(new AddPlatformInputModel()
				{
					Name = name
				});

				Assert.IsFalse(result);
			}
			else
			{
				var result = await platformService.AddPlatformAsync(new AddPlatformInputModel()
				{
					Name = name
				});

				Assert.IsTrue(result);
			}
		}
	}
}
