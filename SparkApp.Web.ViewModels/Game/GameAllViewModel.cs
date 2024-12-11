
namespace SparkApp.Web.ViewModels.Game
{
    public class GameAllViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string? ReleaseYear { get; set; }
    }
}
