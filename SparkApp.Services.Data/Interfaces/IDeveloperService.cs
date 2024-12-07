
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Services.Data.Interfaces
{
    public interface IDeveloperService : IBaseService
    {
        Task<bool> AddDeveloperAsync(AddDeveloperInputModel model);

        Task<List<DeveloperViewModel>?> GetAllAsync();

        Task<DeveloperDetailsViewModel?> GetDeveloperDetailsAsync(string name);
    }
}
