
using System.ComponentModel.DataAnnotations;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.ViewModels.Director
{
	public class DirectorDetailsViewModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;


		public string? ImageUrl { get; set; }

		public string? About { get; set; }

		public HashSet<GameAllViewModel> Games { get; set; }
			= new HashSet<GameAllViewModel>();
	}
}
