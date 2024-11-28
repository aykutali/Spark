using Microsoft.AspNetCore.Mvc;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GuessGameController : ControllerBase
	{
		private readonly ILogger<GuessGameController> logger;
		private readonly IGuessGameService guessGameService;

		public GuessGameController(ILogger<GuessGameController> logger,
								   IGuessGameService guessGameService)
		{
			this.logger = logger; 
			this.guessGameService = guessGameService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Guess(string name)
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now);
			GuessTheGameViewModel viewModel = await guessGameService.GuessGameAsync(name, today);

			if (viewModel == null)
			{
				return NotFound();
			}

			return Ok(viewModel);
		}
	}
}
