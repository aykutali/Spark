using System.Globalization;
using Microsoft.AspNetCore.Mvc;

using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class GenreController : BaseController
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
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
        public async Task<IActionResult> Add(AddGenreInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await genreService.AddGenreAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("{name}")]
		public async Task<IActionResult> Details(string name)
        {
	        GenreDetailsViewModel genreModel = await  genreService.GetGenreDetailsAsync(name);

	        if (genreModel != null)
	        {
				return View(genreModel);
			}

	        return RedirectToAction(nameof(Index));
        }
    }
}
