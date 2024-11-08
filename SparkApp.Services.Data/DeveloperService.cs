
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Developer;

namespace SparkApp.Services.Data
{
    public class DeveloperService : BaseService, IDeveloperService
    {
        private readonly IRepository<Developer, Guid> developerRepository;

        public DeveloperService(IRepository<Developer, Guid> developerRepository)
        {
            this.developerRepository = developerRepository;
        }

        public async Task AddDeveloperAsync(AddDeveloperInputModel model)
        {
            Developer devData = new Developer
            {
                Name = model.Name,
                LogoUrl = model.LogoUrl
            };

            await developerRepository.AddAsync(devData);
        }
    }
}
