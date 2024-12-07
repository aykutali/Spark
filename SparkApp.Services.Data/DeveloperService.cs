
using Microsoft.EntityFrameworkCore;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Services.Data
{
	public class DeveloperService : BaseService, IDeveloperService
	{
		private readonly IRepository<Developer, Guid> developerRepository;

		public DeveloperService(IRepository<Developer, Guid> developerRepository)
		{
			this.developerRepository = developerRepository;
		}

		public async Task<bool> AddDeveloperAsync(AddDeveloperInputModel model)
		{
			bool isDevAlreadyExist = await developerRepository.GetAllAttached()
				.AnyAsync(d => d.Name == model.Name);
			if (isDevAlreadyExist)
			{
				return false;
			}

			Developer devData = new Developer
			{
				Name = model.Name,
				LogoUrl = model.LogoUrl
			};

			await developerRepository.AddAsync(devData);
			return true;
		}

		public async Task<List<DeveloperViewModel>?> GetAllAsync()
		{
			return await developerRepository.GetAllAttached()
				.Select(d => new DeveloperViewModel()
				{
					Id = d.Id,
					Name = d.Name
				})
				.OrderBy(d => d.Name)
				.ToListAsync();

		}

		public async Task<DeveloperDetailsViewModel?> GetDeveloperDetailsAsync(string name)
		{
			Developer? developer = await developerRepository
				.GetAllAttached()
				.Include(d => d.Games)
				.FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());

			if (developer != null)
			{
				List<GameAllViewModel?> games = new List<GameAllViewModel?>();

				foreach (var game in developer.Games)
				{
					if (game.IsDeleted == false &&
						game.IsConfirmed == true)
					{
						games.Add(new GameAllViewModel()
						{
							Id = game.Id,
							Title = game.Title,
							ImageUrl = game.ImageUrl

						});
					}
				}

				DeveloperDetailsViewModel developerModel = new DeveloperDetailsViewModel()
				{
					Id = developer.Id.ToString(),
					Name = developer.Name,
					LogoUrl = developer.LogoUrl,
					Games = games.ToHashSet()
				};

				return developerModel;
			}

			return null;
		}
	}
}
