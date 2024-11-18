
using System.ComponentModel.DataAnnotations;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.ViewModels.Developer
{
	public class DeveloperDetailsViewModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;

		public string? LogoUrl { get; set; }

		public HashSet<GameAllViewModel> Games { get; set; }
		= new HashSet<GameAllViewModel>();
	}
}
