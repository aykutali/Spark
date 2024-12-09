
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Seeding.DataObjects
{
	public class ImportGameGenreDto
	{
		[Required]
		public string GenreId { get; set; } = null!;

		[Required]
		public string GameId { get; set; } = null!;

		public bool IsDeleted { get; set; }
	}
}
