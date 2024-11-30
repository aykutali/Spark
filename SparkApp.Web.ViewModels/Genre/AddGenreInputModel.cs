using System.ComponentModel.DataAnnotations;

using SparkApp.Common;

namespace SparkApp.Web.ViewModels.Genre
{
    public class AddGenreInputModel
    {
        [Required(ErrorMessage = "Genre name is required")]
        [MinLength(EntityValidationConstants.Genre.NameMinLength)]
        [MaxLength(EntityValidationConstants.Genre.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = $"Genre description is required to be between 10-255")]
        [MinLength(EntityValidationConstants.Genre.DescriptionMinLength)]
        [MaxLength(EntityValidationConstants.Genre.DescriptionMaxLength)]
        public string Description { get; set; } = null!;
    }
}
