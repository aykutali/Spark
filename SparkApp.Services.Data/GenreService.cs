﻿
using Microsoft.EntityFrameworkCore;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Data
{
	public class GenreService : BaseService, IGenreService
	{
		private readonly IRepository<Genre, Guid> genreRepository;
		private readonly IRepository<Game, Guid> gameRepository;

		public GenreService(IRepository<Genre, Guid> genreRepository,
							IRepository<Game, Guid> gameRepository)
		{
			this.genreRepository = genreRepository;
			this.gameRepository = gameRepository;
		}

		public async Task<bool> AddGenreAsync(AddGenreInputModel model)
		{
			bool isGenreAlreadyExist = await genreRepository.GetAllAttached()
				.AnyAsync(g => g.Name == model.Name);
			if (isGenreAlreadyExist)
			{
				return false;
			}

			model.Name = Sanitize(model.Name);
			model.Description = Sanitize(model.Description);

			if (!IsModelValid(model))
			{
				return false;
			}

			var genreData = new Genre
			{
				Name = model.Name,
				Description = model.Description
			};

			await genreRepository.AddAsync(genreData);

			return true;
		}

		public async Task<List<GenreViewModel>?> GetAllAsync()
		{
			return await genreRepository.GetAllAttached()
				.Select(g => new GenreViewModel()
				{
					Id = g.Id,
					Name = g.Name,
				})
				.OrderBy(g => g.Name)
				.ToListAsync();
		}
		public async Task<GenreDetailsViewModel?> GetGenreDetailsAsync(string name)
		{
			Genre? genre = await genreRepository
				.GetAllAttached()
				.Include(g => g.GamesGenre)
				.ThenInclude(gg => gg.Game)
				.FirstOrDefaultAsync(g => g.Name == name);

			if (genre != null)
			{
				List<GameAllViewModel?> games = await gameRepository
					.GetAllAttached()
					.Where(g => g.MainGenreId == genre.Id)
					.Select(g => new GameAllViewModel()
					{
						Id = g.Id,
						ImageUrl = g.ImageUrl,
						Title = g.Title
					})
					.ToListAsync();

				foreach (var gg in genre.GamesGenre)
				{
					if (!gg.IsDeleted && !gg.Game.IsDeleted)
					{
						games.Add(new GameAllViewModel()
						{
							Id = gg.GameId,
							ImageUrl = gg.Game.ImageUrl,
							Title = gg.Game.Title
						});
					}
				}

				GenreDetailsViewModel genreModel = new GenreDetailsViewModel()
				{
					Id = genre.Id.ToString(),
					Name = name,
					Description = genre.Description,
					Games = games.ToHashSet()
				};

				return genreModel;
			}

			return null;
		}
	}
}
