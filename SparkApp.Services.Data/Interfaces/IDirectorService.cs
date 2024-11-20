
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Services.Data.Interfaces
{
	public interface IDirectorService : IBaseService
	{
		Task AddDirectorAsync(AddDirectorInputModel model);

		Task<List<DirectorViewModel>?> GetAllAsync();

		Task<DirectorDetailsViewModel> GetDirectorDetailsAsync(string name);
	}
}
