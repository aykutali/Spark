
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Services.Data
{
    public class PlatformService : BaseService, IPlatformService
    {
        private readonly IRepository<Platform, Guid> platformRepository;

        public PlatformService(IRepository<Platform, Guid> platformRepository)
        {
            this.platformRepository = platformRepository;
        }

        public async Task AddPlatformAsync(AddPlatformInputModel model)
        {
            Platform platformData = new Platform
            {
                Name = model.Name
            };

            await platformRepository.AddAsync(platformData);
        }
    }
}
