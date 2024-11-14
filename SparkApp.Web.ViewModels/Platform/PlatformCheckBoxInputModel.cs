
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Web.ViewModels.Platform
{
	public class PlatformCheckBoxInputModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;

		public string? LinkToPlatform { get; set; } = string.Empty;

		public bool IsSelected { get; set; }
	}
}
