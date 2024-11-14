

using SparkApp.Data.Models;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Services.Data.Interfaces
{
    public interface IGameService : IBaseService
    {
        public Task<List<GameAllViewModel>> GetAllGamesAsync();

        public Task<Game> GetGameByIdAsync(string id);

        public Task<GameEditViewModel> GetEditGameModelAsync(string id);

        public Task<GameEditViewModel> GetEditGameModelAsync(GameEditViewModel gameData);

        public Task<bool> EditGameAsync(Game gameToEdit, GameEditViewModel editModel);

		public Task<GameDetailsViewModel?> GetGameDetailsAsync(string title);

        public Task<AddGameInputModel> GetInputGameModelAsync();

        public Task<AddGameInputModel> GetInputGameModelAsync(AddGameInputModel model);

        public Task<AddPlatformsToGameInputModel?> GetInputPlatformsToGameModelAsync(string id);

        public Task AddGameAsync(AddGameInputModel model);

        public Task AddPlatformsToGameAsync(AddPlatformsToGameInputModel model);
        
    }
}
