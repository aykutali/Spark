
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Seeding.DataObjects
{
	public class ImportGamePlatformDto
	{
		[Required]
		public string GameId { get; set; } = null!;

		[Required]
		public string PlatformId { get; set; } = null!;

		public string? LinkToPlatform { get; set; }

		public bool IsDeleted { get; set; }
	}
}
