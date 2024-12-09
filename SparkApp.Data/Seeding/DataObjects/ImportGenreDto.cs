
using SparkApp.Common;
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Seeding.DataObjects
{
	public class ImportGenreDto
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		[MinLength(EntityValidationConstants.Genre.NameMinLength)]
		[MaxLength(EntityValidationConstants.Genre.NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[MinLength(EntityValidationConstants.Genre.DescriptionMinLength)]
		[MaxLength(EntityValidationConstants.Genre.DescriptionMaxLength)]
		public string Description { get; set; } = null!;
	}
}
