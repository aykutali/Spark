
using System.ComponentModel.DataAnnotations;
using SparkApp.Web.ViewModels.Game;

namespace SparkApp.Web.ViewModels.Platform
{
	public class PlatformDetailsViewModel
	{
		[Required] 
		public string Id { get; set; } = null!;

		[Required] 
		public string Name { get; set; } = null!;

		public HashSet<GameAllViewModel> Games { get; set; } = new HashSet<GameAllViewModel>();
	}
}
