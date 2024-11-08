using Microsoft.AspNetCore.Mvc;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Genre;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Web.Controllers
{
    public class PlatformController: BaseController
    {
        private readonly IPlatformService platformService;

        public PlatformController(IPlatformService platformService)
        {
            this.platformService = platformService;
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
        public async Task<IActionResult> Add(AddPlatformInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await platformService.AddPlatformAsync(model);

            return View(nameof(Index));
        }
    }
}
