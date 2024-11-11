
using static SparkApp.Common.EntityValidationConstants.Game;
using SparkApp.Data.Models;
using SparkApp.Data.Repository.Interfaces;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Director;
using SparkApp.Web.ViewModels.Game;
using SparkApp.Web.ViewModels.Genre;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SparkApp.Data;

namespace SparkApp.Services.Data
{
    public class GameService : BaseService, IGameService
    {
        private readonly SparkDbContext db;
        private readonly IRepository<Game, Guid> gameRepository;

        private readonly IRepository<Developer, Guid> devRepository;
        private readonly IRepository<Director, Guid> dirRepository;
        private readonly IRepository<Genre, Guid> genreRepository;

        public GameService(IRepository<Game, Guid> gameRepository,
                           IRepository<Director, Guid> dirRepository,
                           IRepository<Genre, Guid> genreRepository,
                           IRepository<Developer, Guid> devRepository,
                           SparkDbContext db)
        {
            this.gameRepository = gameRepository;
            this.dirRepository = dirRepository;
            this.genreRepository = genreRepository;
            this.devRepository = devRepository;
            this.db = db;
        }

        public async Task<List<GameAllViewModel>> GetAllGamesAsync()
        {
            List<GameAllViewModel> games = await gameRepository.GetAllAttached()
                .Select(g => new GameAllViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    ImageUrl = g.ImageUrl
                })
                .ToListAsync();

            return games;
        }

        public async Task<GameDetailsViewModel> GetGameDetailsAsync(string title)
        {
            Game game = await gameRepository.GetAllAttached()
                .Where(g => g.Title == title)
                .Include(g => g.LeadGameDirector)
                .Include(g => g.Developer)
                .Include(g => g.MainGenre)
                .FirstOrDefaultAsync();

            var genres = await db.GamesGenres
                .Where(gg => gg.Game.Title == title)
                .Select(gg=> gg.Genre)
                .ToListAsync();

            var platforms = await db.GamesPlatforms
                .Where(gp => gp.Game.Title == title)
                .Include(gp=>gp.Platform)
                .ToListAsync();

            GameDetailsViewModel gameDetailsModel = new GameDetailsViewModel
            {
                Id = game.Id.ToString(),
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Developer = game.Developer,
                LeadDirector = game.LeadGameDirector,
                MainGenre = game.MainGenre,
                ReleasedDate = game.ReleaseDate,
                SubGenres = genres,
                PlatformsList = platforms
            };

            return gameDetailsModel;
        }

        public async Task<AddGameInputModel> GetInputGameModelAsync()
        {
            IEnumerable<Genre> genresList = await genreRepository.GetAllAsync();
            IEnumerable<Developer> devList = await devRepository.GetAllAsync();
            IEnumerable<Director> directorList = await dirRepository.GetAllAsync();

            List<GenreViewModel> genreModelsList = genresList
                .Select(x => (new GenreViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }))
                .ToList();

            List<DeveloperViewModel> devModelsList = devList
                .Select(x => (new DeveloperViewModel
                {
                    Id = x.Id,
                    Name = x.Name,

                }))
                .ToList();

            List<DirectorViewModel> directorModelsList = directorList
                .Select(x => (new DirectorViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }))
                .ToList();

            AddGameInputModel gameModel = new AddGameInputModel
            {
                Genres = genreModelsList,
                Developers = devModelsList,
                Directors = directorModelsList
            };

            return gameModel;
        }

        public async Task<AddGameInputModel> GetInputGameModelAsync(AddGameInputModel gameModel)
        {
            AddGameInputModel gameLists = await GetInputGameModelAsync();

            gameModel.Genres = gameLists.Genres;
            gameModel.Developers = gameLists.Developers;
            gameModel.Directors = gameLists.Directors;

            return gameModel;
        }

        public async Task AddGameAsync(AddGameInputModel model)
        {
            var dateTimeString = $"{model.ReleasedDate}";

            DateTime.TryParseExact(dateTimeString, ReleasedDateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime parseDateTime);

            var gameData = new Game
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                ReleaseDate = parseDateTime,
                DeveloperId = new Guid(model.DeveloperId),
                MainGenreId = new Guid(model.MainGenreId),
                LeadGameDirectorId = new Guid(model.LeadGameDirectorId)
            };

            await gameRepository.AddAsync(gameData);
        }
    }
}
