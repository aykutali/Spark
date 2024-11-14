
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.ViewModels.Genre
{

	public class GenreDetailsViewModel
	{
		public string Id { get; set; } = null!;

		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public HashSet<GameAllViewModel> Games { get; set; } = new HashSet<GameAllViewModel>();
	}
}
