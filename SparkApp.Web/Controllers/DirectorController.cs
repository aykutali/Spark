using System.Globalization;
using Microsoft.AspNetCore.Mvc;

using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class DirectorController : BaseController
    {
        private readonly IDirectorService directorService;

        public DirectorController(IDirectorService directorService)
        {
            this.directorService = directorService;
        }

        public async Task<IActionResult> Index()
        {
	        List<DirectorViewModel>? directors = await directorService.GetAllAsync();

	        return View(directors);
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

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("{name}")]
		public async Task<IActionResult> Details(string name)
		{
			DirectorDetailsViewModel? directorModel = await directorService.GetDirectorDetailsAsync(name);

			if (directorModel != null)
			{
                return View(directorModel);
			}

			return RedirectToAction(nameof(Index));
        }
    }
}
