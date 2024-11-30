using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.Controllers
{
	public class GuessTheGameController : Controller
	{
		private readonly IGameService gameService;
		private readonly IGuessGameService guessGameService;

		public GuessTheGameController(IGameService gameService,
									  IGuessGameService guessGameService)
		{
			this.gameService = gameService;
			this.guessGameService = guessGameService;
		}
		public async Task<IActionResult> Index()
		{
			var allGames = await gameService.GetAllGamesAsync();

			var allGamesList = await allGames.ToListAsync();

			string gameTitleFromYesterday = await guessGameService.GetGameTitleFromDayBefore();

			if (!gameTitleFromYesterday.IsNullOrEmpty())
			{
				ViewData["YesterdaysGame"] = gameTitleFromYesterday;
			}
			
			return View(allGamesList);
		}
	}
}
