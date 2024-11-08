
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Data
{
    public class GenreService : BaseService, IGenreService
    {
        private readonly IRepository<Genre, Guid> genreRepository;

        public GenreService(IRepository<Genre, Guid> genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task AddGenreAsync(AddGenreInputModel model)
        {
            var genreData = new Genre
            {
                Name = model.Name,
                Description = model.Description
            };

            await genreRepository.AddAsync(genreData);
        }
    }
}
