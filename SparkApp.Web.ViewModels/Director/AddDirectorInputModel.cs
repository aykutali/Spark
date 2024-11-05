
using SparkApp.Common;
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Web.ViewModels.Director
{
    public class AddDirectorInputModel
    {
        [Required]
        [MinLength(EntityValidationConstants.Director.NameMinLength)]
        [MaxLength(EntityValidationConstants.Director.NameMaxLength)]
        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [MinLength(EntityValidationConstants.Director.AboutMinLength)]
        [MaxLength(EntityValidationConstants.Director.AboutMaxLength)]
        public string? About { get; set; }
    }
}
