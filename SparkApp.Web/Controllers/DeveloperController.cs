using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Director;

namespace SparkApp.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class DeveloperController : BaseController
    {
        private readonly IDeveloperService developerService;

        public DeveloperController(IDeveloperService developerService)
        {
            this.developerService = developerService;
        }

        public async Task<IActionResult> Index()
        {
	        List<DeveloperViewModel>? developers = await developerService.GetAllAsync();

            return View(developers);
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
		public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
		public async Task<IActionResult> Add(AddDeveloperInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await developerService.AddDeveloperAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("{name}")]
		public async Task<IActionResult> Details(string name)
		{
			DeveloperDetailsViewModel? developerModel = await developerService.GetDeveloperDetailsAsync(name);

			if (developerModel!= null)
			{
				return View(developerModel);
			}

			return RedirectToAction(nameof(Index));
		}
    }
}
