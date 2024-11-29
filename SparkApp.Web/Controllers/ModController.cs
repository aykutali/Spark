using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkApp.Common;
using SparkApp.Data.Models;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

using static SparkApp.Common.EntityValidationConstants.Game;

namespace SparkApp.Web.Controllers
{
	[Authorize(Roles = "Moderator")]
	public class ModController : BaseController
	{
		private readonly IGameService gameService;
		public ModController(IGameService gameService)
		{
			this.gameService = gameService;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmGames()
		{
			var allGamesToConfirm = await gameService.GetAllGamesToConfirmAsync();
			return View(allGamesToConfirm);
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmGame(GameEditViewModel gameModel)
		{
			string dateTimeSting = $"{gameModel.ReleasedDate}";

			GameDetailsViewModel? isGameAlreadyAdded = await gameService.GetGameDetailsAsync(gameModel.Title);

			if (isGameAlreadyAdded != null && isGameAlreadyAdded.Id != gameModel.Id)
			{
				ModelState.AddModelError("Title", "Game with that title already exist in our site...");
			}

			if (!DateTime.TryParseExact(dateTimeSting, ReleasedDateFormat, CultureInfo.InvariantCulture,
				    DateTimeStyles.None, out DateTime parseDateTime))
			{
				ModelState.AddModelError("ReleasedDate", "Invalid Date Format!");
			}

			if (!ModelState.IsValid)
			{
				gameModel = await gameService.GetEditGameModelAsync(gameModel);
				return Redirect($"{Inspect(gameModel.Id)}");
			}

			Game gameToConfirm = await gameService.GetGameByIdAsync(gameModel.Id);
			gameToConfirm.IsConfirmed = true;

			await gameService.EditGameAsync(gameToConfirm, gameModel);

			return Redirect($"{nameof(GameController.Details)}/{gameToConfirm.Title}");
		}

		[HttpGet]
		public async Task<IActionResult> Inspect(string id)
		{
			var game = await gameService.GetEditGameModelAsync(id);
			return View(game);
		}
	}
}
