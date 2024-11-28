
namespace SparkApp.Web.ViewModels.Game
{
	public class GuessTheGameViewModel
	{
		public Dictionary<string, char> Title { get; set; }

		public string ImageUrl { get; set; }

		public Dictionary<string, char> Developer { get; set; }

		public Dictionary<string, char> LeadGameDirector { get; set; }

		public Dictionary<string, char> ReleaseDate { get; set; }

		public Dictionary<string, char> Platforms { get; set; }

		public Dictionary<string, char> MainGenre { get; set; }

		public Dictionary<string, char> SubGenres { get; set; }

		public bool IsGuessIsCorrect { get; set; }
	}
}
