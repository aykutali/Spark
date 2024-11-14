

namespace SparkApp.Web.ViewModels.Game
{
    using SparkApp.Data.Models;

    public class GameDetailsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime ReleasedDate { get; set; }

        public Genre MainGenre { get; set; }

        public Director LeadDirector { get; set; }

        public Developer Developer { get; set; }

        public IEnumerable<GamePlatform> PlatformsList { get; set; } = new List<GamePlatform>();

        public IEnumerable<GameGenre> SubGenres { get; set; } = new List<GameGenre>();
    }
}
