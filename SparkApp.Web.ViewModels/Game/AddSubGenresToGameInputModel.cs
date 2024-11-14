
using System.ComponentModel.DataAnnotations;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Web.ViewModels.Game
{
	public class AddSubGenresToGameInputModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Title { get; set; } = null!;

		public List<GenreCheckBoxInputModel> SubGenres { get; set; }
			= new List<GenreCheckBoxInputModel>();
	}
}
