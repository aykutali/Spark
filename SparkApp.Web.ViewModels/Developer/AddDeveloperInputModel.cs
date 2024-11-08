using System.ComponentModel.DataAnnotations;

using SparkApp.Common;

namespace SparkApp.Web.ViewModels.Developer
{
    public class AddDeveloperInputModel
    {
        [Required]
        [MinLength(EntityValidationConstants.Developer.NameMinLength)]
        [MaxLength(EntityValidationConstants.Developer.NameMaxLength)]
        public string Name { get; set; } = null!;

        public string? LogoUrl { get; set; }

    }
}
