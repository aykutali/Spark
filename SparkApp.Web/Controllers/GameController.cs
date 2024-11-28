﻿using Microsoft.AspNetCore.Mvc;

using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using SparkApp.Data.Models;
using static SparkApp.Common.EntityValidationConstants.Game;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class GameController : BaseController
	{
		private readonly IGameService gameService;

		public GameController(IGameService gameService)
		{
			this.gameService = gameService;
		}

		public IActionResult Index()
		{
			return RedirectToAction(nameof(All));
		}

		[HttpGet]
		public async Task<IActionResult> All(string title)
		{
			ViewData["CurrentFilter"] = title;

			var allGames = await gameService.GetAllGamesAsync();

			if (!String.IsNullOrEmpty(title))
			{
				allGames = allGames.Where(g => g.Title.ToLower().Contains(title.ToLower())).ToList();
			}

			return View(allGames);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var gameInputModel = await gameService.GetInputGameModelAsync();
			return View(gameInputModel);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddGameInputModel gameModel)
		{
			string dateTimeSting = $"{gameModel.ReleasedDate}";

			if (!DateTime.TryParseExact(dateTimeSting, ReleasedDateFormat, CultureInfo.InvariantCulture,
					DateTimeStyles.None, out DateTime parseDateTime))
			{
				ModelState.AddModelError("ReleasedDate", "Invalid Date Format!");
			}

			GameDetailsViewModel? isGameAlreadyAdded = await gameService.GetGameDetailsAsync(gameModel.Title);

			if (isGameAlreadyAdded != null)
			{
				ModelState.AddModelError("Title", "Game with that title already exist in our site...");
			}

			if (!ModelState.IsValid)
			{
				gameModel = await gameService.GetInputGameModelAsync(gameModel);
				return View(gameModel);
			}

			bool isUserMod = User.IsInRole("Moderator");

			await gameService.AddGameAsync(gameModel, isUserMod);

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		[Route("{title}")]
		public async Task<IActionResult> Details(string title)
		{
			var game = await gameService.GetGameDetailsAsync(title);

			if (game != null)
			{
				return View(game);
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		[Authorize(Roles = "Moderator")]
		public async Task<IActionResult> Edit(string id)
		{
			GameEditViewModel gameEditModel = await gameService.GetEditGameModelAsync(id);

			return View(gameEditModel);
		}

		[HttpPost]
		[Authorize(Roles = "Moderator")]
		public async Task<IActionResult> Edit(GameEditViewModel gameEditModel)
		{
			string dateTimeSting = $"{gameEditModel.ReleasedDate}";

			if (!DateTime.TryParseExact(dateTimeSting, ReleasedDateFormat, CultureInfo.InvariantCulture,
					DateTimeStyles.None, out DateTime parseDateTime))
			{
				ModelState.AddModelError("ReleasedDate", "Invalid Date Format!");
			}

			if (!ModelState.IsValid)
			{
				gameEditModel = await gameService.GetEditGameModelAsync(gameEditModel);
				return View(gameEditModel);
			}

			Game gameToEdit = await gameService.GetGameByIdAsync(gameEditModel.Id);

			try
			{
				await gameService.EditGameAsync(gameToEdit, gameEditModel);
				return Redirect($"{nameof(Details)}/{gameToEdit.Title}");
			}
			catch (Exception e)
			{
				gameEditModel = await gameService.GetEditGameModelAsync(gameEditModel);
				return View(gameEditModel);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Moderator")]
		public async Task<IActionResult> ManagePlatforms(string id)
		{
			AddPlatformsToGameInputModel? inputModel = await gameService.GetInputPlatformsToGameModelAsync(id);

			if (inputModel != null)
			{
				return View(inputModel);
			}
			else
			{
				return RedirectToAction(nameof(Index));
			}

		}

		[HttpPost]
		[Authorize(Roles = "Moderator")]
		public async Task<IActionResult> ManagePlatforms(AddPlatformsToGameInputModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return View(model);
			}

			Guid movieGuid = Guid.Empty;
			bool isGuidValid = this.IsGuidValid(model.Id, ref movieGuid);
			if (!isGuidValid)
			{
				return RedirectToAction(nameof(Index));
			}

			await gameService.AddPlatformsToGameAsync(model);

			return Redirect($"{nameof(Details)}/{model.Title}");
		}

		[HttpGet]
		[Authorize(Roles = "Moderator")]
		public async Task<IActionResult> ManageSubGenres(string id)
		{
			AddSubGenresToGameInputModel? inputModel = await gameService.GetInputGenresToGameModelAsync(id);

			if (inputModel != null)
			{
				return View(inputModel);
			}
			else
			{
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		[Authorize(Roles = "Moderator")]
		public async Task<IActionResult> ManageSubGenres(AddSubGenresToGameInputModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return View(model);
			}

			Guid movieGuid = Guid.Empty;
			bool isGuidValid = this.IsGuidValid(model.Id, ref movieGuid);
			if (!isGuidValid)
			{
				return RedirectToAction(nameof(Index));
			}

			await gameService.AddSubGenresToGameAsync(model);

			return Redirect($"{nameof(Details)}/{model.Title}");
		}

		[HttpGet]
		[Authorize(Roles = "Moderator")]
		public async Task<IActionResult> Delete(string id)
		{
			Game? game = await gameService.GetGameByIdAsync(id);

			GameAllViewModel model = new GameAllViewModel()
			{
				Id = game.Id,
				ImageUrl = game.ImageUrl,
				Title = game.Title
			};

			if (game != null)
			{
				return View(model);
			}

			return BadRequest();
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			await gameService.DeleteAGame(id);

			return RedirectToAction(nameof(Index));
		}
	}
}
