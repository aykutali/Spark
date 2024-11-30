
using SparkApp.Data.Models;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Services.Data.Interfaces
{
	public interface IGuessGameService : IBaseService
	{
		public Task<GuessTheGameViewModel> GuessGameAsync(string gameTitle, DateOnly date);

		public Task<Game> GetGameOfTheDayAsync(DateOnly date);

		public Task SetGameOfTheDayAsync(DateOnly date);

		public Task<string> GetGameTitleFromDayBefore();
	}
}
