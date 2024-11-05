
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Data.Interfaces
{
    public interface IGenreService
    {
        Task AddGenreAsync(AddGenreInputModel model);

    }
}
