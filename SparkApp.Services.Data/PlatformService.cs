
using Microsoft.EntityFrameworkCore;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Services.Data
{
	public class PlatformService : BaseService, IPlatformService
	{
		private readonly IRepository<Platform, Guid> platformRepository;

		public PlatformService(IRepository<Platform, Guid> platformRepository)
		{
			this.platformRepository = platformRepository;
		}

		public async Task AddPlatformAsync(AddPlatformInputModel model)
		{
			Platform platformData = new Platform
			{
				Name = model.Name
			};

			await platformRepository.AddAsync(platformData);
		}

		public async Task<PlatformDetailsViewModel?> GetPlatformDetailsAsync(string name)
		{
			Platform? platform = await platformRepository
				.GetAllAttached()
				.Include(p => p.GamesPlatform)
				.ThenInclude(gp => gp.Game)
				.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

			if (platform != null)
			{
				List<GameAllViewModel?> games = new List<GameAllViewModel?>();

				foreach (var gp in platform.GamesPlatform)
				{
					if (!gp.IsDeleted)
					{
						games.Add(new GameAllViewModel()
						{
							Id = gp.GameId,
							ImageUrl = gp.Game.ImageUrl,
							Title = gp.Game.Title
						});
					}
					
				}

				PlatformDetailsViewModel viewModel = new PlatformDetailsViewModel()
				{
					Id = platform.Id.ToString(),
					Name = name,
					Games = games.ToHashSet()
				};

				return viewModel;
			}

			return null;
		}
	}
}
