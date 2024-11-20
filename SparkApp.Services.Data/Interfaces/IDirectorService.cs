
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Services.Data.Interfaces
{
	public interface IDirectorService : IBaseService
	{
		public Task AddDirectorAsync(AddDirectorInputModel model);

		public Task<DirectorDetailsViewModel> GetDirectorDetailsAsync(string name);
	}
}
