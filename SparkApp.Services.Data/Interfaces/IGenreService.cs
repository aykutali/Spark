
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Services.Data.Interfaces
{
	public interface IGenreService : IBaseService
	{
		Task<bool> AddGenreAsync(AddGenreInputModel model);

		Task<List<GenreViewModel>?> GetAllAsync();

		Task<GenreDetailsViewModel?> GetGenreDetailsAsync(string name);

	}
}
