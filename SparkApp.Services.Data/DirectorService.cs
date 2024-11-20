
using Microsoft.EntityFrameworkCore;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Director;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Services.Data
{
	public class DirectorService : BaseService, IDirectorService
	{
		private readonly IRepository<Director, Guid> directorRepository;

		public DirectorService(IRepository<Director, Guid> directorRepository)
		{
			this.directorRepository = directorRepository;
		}

		public async Task AddDirectorAsync(AddDirectorInputModel model)
		{
			Director directorData = new Director
			{
				Name = model.Name,
				About = model.About,
				ImageUrl = model.ImageUrl
			};

			await directorRepository.AddAsync(directorData);
		}

		public async Task<List<DirectorViewModel>?> GetAllAsync()
		{
			return await directorRepository.GetAllAttached()
				.Select(d => new DirectorViewModel()
				{
					Id = d.Id,
					Name = d.Name,
				})
				.OrderBy(d => d.Name)
				.ToListAsync();
		}

		public async Task<DirectorDetailsViewModel> GetDirectorDetailsAsync(string name)
		{
			Director? director = await directorRepository
				.GetAllAttached()
				.Include(d => d.Games)
				.FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());

			if (director != null)
			{
				DirectorDetailsViewModel viewModel = new DirectorDetailsViewModel()
				{
					Id = director.Id.ToString(),
					Name = director.Name,
					ImageUrl = director.ImageUrl,
					About = director.About,
					Games = director.Games
							.Select(g => new GameAllViewModel()
							{
								Id = g.Id,
								ImageUrl = g.ImageUrl,
								Title = g.Title,
							})
							.ToHashSet()
				};

				return viewModel;
			}

			return null;
		}
	}
}
