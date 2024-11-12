using Microsoft.AspNetCore.Mvc;

using System.Globalization;
using SparkApp.Data.Models;
using static SparkApp.Common.EntityValidationConstants.Game;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.Controllers
{
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

		public async Task<IActionResult> All()
		{
			var allGames = await gameService.GetAllGamesAsync();

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

			if (!ModelState.IsValid)
			{
				gameModel = await gameService.GetInputGameModelAsync(gameModel);
				return View(gameModel);
			}

			await gameService.AddGameAsync(gameModel);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Details(string id)
		{
			var game = await gameService.GetGameDetailsAsync(id);
			return View(game);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			GameEditViewModel gameEditModel = await gameService.GetEditGameModelAsync(id);

			return View(gameEditModel);
		}

		[HttpPost]
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
				return RedirectToAction(nameof(Index));
			}
			catch (Exception e)
			{
				gameEditModel = await gameService.GetEditGameModelAsync(gameEditModel);
				return View(gameEditModel);
			}
		}
	}
}
