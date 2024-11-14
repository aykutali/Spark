
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Data.Interfaces
{
    public interface IGenreService : IBaseService
    {
        Task AddGenreAsync(AddGenreInputModel model);

        Task<GenreDetailsViewModel?> GetGenreDetailsAsync(string name);

    }
}
