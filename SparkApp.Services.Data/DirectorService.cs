
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Services.Data
{
    public class DirectorService : BaseService, IDirectorService
    {
        private readonly IRepository<Director, Guid> directorRepository;

        public DirectorService(IRepository<Director, Guid> directorRepository)
        {
            this.directorRepository = directorRepository;
        }

        public async Task AddDirectorAsync(AddDirectorInputModel model)
        {
            Director directorData = new Director
            {
                Name = model.Name,
                About = model.About,
                ImageUrl = model.ImageUrl
            };

             await directorRepository.AddAsync(directorData);
        }
    }
}
