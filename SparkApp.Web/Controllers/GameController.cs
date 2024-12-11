using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

using SparkApp.Data.Models;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.Models;

using static SparkApp.Common.EntityValidationConstants.Game;
using static SparkApp.Common.AppConstants;

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
		public async Task<IActionResult> All(string title, string year, int? pageNumber)
		{
			if (String.IsNullOrWhiteSpace(title) && pageNumber == null&& String.IsNullOrWhiteSpace(year))
			{
				pageNumber = 1;
			}

			ViewData["CurrentFilter"] = title;
			ViewData["YearFilter"] = year;

			var allGames = await gameService.GetAllGamesAsync();

			if (!String.IsNullOrEmpty(title))
			{
				allGames = allGames.Where(g => g.Title.ToLower().Contains(title.ToLower()));
			}

			if (!String.IsNullOrWhiteSpace(year))
			{
				allGames = allGames.Where(g => g.ReleaseYear == year);
			}

			int pageSize = GameAllPagesSize;
			return View(await PaginatedList<GameAllViewModel>.CreateAsync(allGames.AsNoTracking(), pageNumber ?? 1, pageSize));
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Add()
		{
			var gameInputModel = await gameService.GetInputGameModelAsync();
			return View(gameInputModel);
		}

		[HttpPost]
		[Authorize]
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

			bool isUserMod = User.IsInRole(ModRoleName);

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
		[Authorize(Roles = ModRoleName)]
		public async Task<IActionResult> Edit(string id)
		{
			Guid parsedGuid = Guid.Empty;
			if (IsGuidValid(id, ref parsedGuid))
			{
				GameEditViewModel gameEditModel = await gameService.GetEditGameModelAsync(id);

				return View(gameEditModel);
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[Authorize(Roles = ModRoleName)]
		public async Task<IActionResult> Edit(GameEditViewModel gameEditModel)
		{
			string dateTimeString = $"{gameEditModel.ReleasedDate}";

			GameDetailsViewModel? isGameAlreadyAdded = await gameService.GetGameDetailsAsync(gameEditModel.Title);

			if (isGameAlreadyAdded != null && isGameAlreadyAdded.Id != gameEditModel.Id)
			{
				ModelState.AddModelError("Title", "Game with that title already exist in our site...");
			}

			if (!DateTime.TryParseExact(dateTimeString, ReleasedDateFormat, CultureInfo.InvariantCulture,
					DateTimeStyles.None, out DateTime parseDateTime))
			{
				ModelState.AddModelError("ReleasedDate", "Invalid Date Format!");
			}

			if (!ModelState.IsValid)
			{
				gameEditModel = await gameService.GetEditGameModelAsync(gameEditModel);
				return View(gameEditModel);
			}

			Game? gameToEdit = await gameService.GetGameByIdAsync(gameEditModel.Id);

			try
			{
				if (gameToEdit != null)
				{
					await gameService.EditGameAsync(gameToEdit, gameEditModel);
					return Redirect($"{nameof(Details)}/{gameToEdit.Title}");
				}
			}
			catch (Exception e)
			{
				gameEditModel = await gameService.GetEditGameModelAsync(gameEditModel);
				return View(gameEditModel);
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		[Authorize(Roles = ModRoleName)]
		public async Task<IActionResult> ManagePlatforms(string id)
		{
			Guid parsedGuid = Guid.Empty;
			if (IsGuidValid(id, ref parsedGuid))
			{
				AddPlatformsToGameInputModel? inputModel = await gameService.GetInputPlatformsToGameModelAsync(id);

				if (inputModel != null)
				{
					return View(inputModel);
				}
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[Authorize(Roles = ModRoleName)]
		public async Task<IActionResult> ManagePlatforms(AddPlatformsToGameInputModel model)
		{
			if (!ModelState.IsValid)
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
		[Authorize(Roles = ModRoleName)]
		public async Task<IActionResult> ManageSubGenres(string id)
		{
			Guid parsedGuid = Guid.Empty;
			if (IsGuidValid(id, ref parsedGuid))
			{
				AddSubGenresToGameInputModel? inputModel = await gameService.GetInputGenresToGameModelAsync(id);

				if (inputModel != null)
				{
					return View(inputModel);
				}
			}

			return RedirectToAction(nameof(Index));

		}

		[HttpPost]
		[Authorize(Roles = ModRoleName)]
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
		[Authorize(Roles = ModRoleName)]
		public async Task<IActionResult> Delete(string id)
		{
			Game? game = await gameService.GetGameByIdAsync(id);

			if (game == null)
			{
				return RedirectToAction(nameof(Index));
			}

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
		[Authorize(Roles = ModRoleName)]
		public async Task<IActionResult> Delete(Guid id)
		{
			Guid parsedGuid = Guid.Empty;
			if (IsGuidValid(id.ToString(), ref parsedGuid))
			{
				await gameService.DeleteAGame(id);
				return RedirectToAction(nameof(Index));
			}

			return BadRequest();
		}
	}
}
