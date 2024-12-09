
using Microsoft.EntityFrameworkCore;
using SparkApp.Common;
using SparkApp.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Seeding.DataObjects
{
	public class ImportGameDto
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		[MinLength(EntityValidationConstants.Game.TitleMinLength)]
		[MaxLength(EntityValidationConstants.Game.TitleMaxLength)]
		public string Title { get; set; } = null!;

		[Required]
		[MinLength(EntityValidationConstants.Game.DescriptionMinLength)]
		[MaxLength(EntityValidationConstants.Game.DescriptionMaxLength)]
		public string Description { get; set; } = null!;

		public string? ImageUrl { get; set; }

		[Required]
		public string ReleaseDate { get; set; } = null!;

		public bool IsDeleted { get; set; } = false;

		public bool IsConfirmed { get; set; } = false;

		[Required]
		public string DeveloperId { get; set; } = null!;

		[Required]
		public string LeadGameDirectorId { get; set; } = null!;

		[Required]
		public string MainGenreId { get; set; } = null!;
	}
}
