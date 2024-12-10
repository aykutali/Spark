using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SparkApp.Data.Models;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

using static SparkApp.Common.EntityValidationConstants.Game;
using static SparkApp.Common.AppConstants;

namespace SparkApp.Web.Controllers
{
	[Authorize(Roles = $"{ModRoleName},{AdminRoleName}")]
	public class ModController : BaseController
	{
		private readonly IGameService gameService;
		private readonly IUserService userService;

		public ModController(IGameService gameService,
			IUserService userService)
		{
			this.gameService = gameService;
			this.userService = userService;
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

			Guid parsedGuid = Guid.Empty;
			if (IsGuidValid(gameModel.Id, ref parsedGuid))
			{
				Game? gameToConfirm = await gameService.GetGameByIdAsync(gameModel.Id);

				if (gameToConfirm != null)
				{
					gameToConfirm.IsConfirmed = true;

					await gameService.EditGameAsync(gameToConfirm, gameModel);

					return Redirect($"{nameof(GameController.Details)}/{gameToConfirm.Title}");
				}
			}

			return BadRequest();
		}

		[HttpGet]
		public async Task<IActionResult> Inspect(string id)
		{
			Guid parsedGuid = Guid.Empty;
			if (IsGuidValid(id, ref parsedGuid))
			{
				var game = await gameService.GetEditGameModelAsync(id);
				return View(game);
			}

			return View(nameof(ConfirmGames));
		}
	}
}