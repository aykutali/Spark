using Microsoft.AspNetCore.Mvc;

using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Web.Controllers
{
    public class DirectorController : BaseController
    {
        private readonly IDirectorService directorService;

        public DirectorController(IDirectorService directorService)
        {
            this.directorService = directorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDirectorInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await directorService.AddDirectorAsync(model);

            return View(nameof(Index));
        }
    }
}
