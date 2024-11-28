﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Genre;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class PlatformController: BaseController
    {
        private readonly IPlatformService platformService;

        public PlatformController(IPlatformService platformService)
        {
            this.platformService = platformService;
        }
        public async Task<IActionResult> Index()
        {
	        List<PlatformViewModel>? platforms = await platformService.GetAllAsync();

            return View(platforms);
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
		public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
		public async Task<IActionResult> Add(AddPlatformInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await platformService.AddPlatformAsync(model);

            return View(nameof(Index));
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Details(string name)
        {
            PlatformDetailsViewModel? platformModel = await platformService.GetPlatformDetailsAsync(name);

            if (platformModel != null)
            {
                return View(platformModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
