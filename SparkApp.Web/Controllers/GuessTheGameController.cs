using Microsoft.AspNetCore.Mvc;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.Controllers
{
	public class GuessTheGameController : Controller
	{
		private readonly IGameService gameService;

		public GuessTheGameController(IGameService gameService)
		{
			this.gameService = gameService;
		}
		public async Task<IActionResult> Index()
		{
			var allGames = await gameService.GetAllGamesAsync();
			return View(allGames);
		}
	}
}
