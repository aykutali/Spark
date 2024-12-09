
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Seeding.DataObjects
{
	public class ImportPlatformDto
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;
	}
}
