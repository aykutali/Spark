
using SparkApp.Common;
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Seeding.DataObjects
{
	public class ImportDeveloperDto
	{
		[Required] 
		public string Id { get; set; } = null!;

		[Required]
		[MinLength(EntityValidationConstants.Developer.NameMinLength)]
		[MaxLength(EntityValidationConstants.Developer.NameMaxLength)]
		public string Name { get; set; } = null!;

		public string? LogoUrl { get; set; }
	}
}
