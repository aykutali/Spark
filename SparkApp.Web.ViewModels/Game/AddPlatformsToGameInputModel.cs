
using System.ComponentModel.DataAnnotations;
using SparkApp.Web.ViewModels.Platform;

namespace SparkApp.Web.ViewModels.Game
{
	public class AddPlatformsToGameInputModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Title { get; set; } = null!;

		public List<PlatformCheckBoxInputModel> Platforms { get; set; }
			= new List<PlatformCheckBoxInputModel>();
	}
}
