
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Web.ViewModels.Platform
{
	public class PlatformViewModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;
	}
}
