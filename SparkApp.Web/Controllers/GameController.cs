using Microsoft.AspNetCore.Mvc;

using System.Globalization;

using  static SparkApp.Common.EntityValidationConstants.Game;
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
            return View();
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

            return View(nameof(Index));
        }
    }
}
