﻿using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Genre;

using static SparkApp.Common.AppConstants;

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

        public async Task<IActionResult> Index()
        {
	        List<GenreViewModel>? genres = await genreService.GetAllAsync();

            return View(genres);
        }

        [HttpGet]
        [Authorize(Roles = ModRoleName)]
		public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ModRoleName)]
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
	        GenreDetailsViewModel? genreModel = await  genreService.GetGenreDetailsAsync(name);

	        if (genreModel != null)
	        {
				return View(genreModel);
			}

	        return RedirectToAction(nameof(Index));
        }
    }
}
