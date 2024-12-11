using SparkApp.Common;
using System.ComponentModel.DataAnnotations;
using SparkApp.Web.ViewModels.Developer;
using SparkApp.Web.ViewModels.Director;
using SparkApp.Web.ViewModels.Genre;
namespace SparkApp.Web.ViewModels.Game
{
    public class AddGameInputModel 
	{
        [Required]
        [MinLength(EntityValidationConstants.Game.TitleMinLength)]
        [MaxLength(EntityValidationConstants.Game.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(EntityValidationConstants.Game.DescriptionMinLength)]
        [MaxLength(EntityValidationConstants.Game.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required] 
        public string ReleasedDate { get; set; } = null!;

        
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "The Developer field is required.")] 
        public string DeveloperId { get; set; } = null!;

        public virtual List<DeveloperViewModel>? Developers { get; set; }
            = new List<DeveloperViewModel>();

        [Required(ErrorMessage = "The Lead Director field is required.")]
        public string LeadGameDirectorId { get; set; } = null!;

        public virtual List<DirectorViewModel>? Directors { get; set; }
            = new List<DirectorViewModel>();

        [Required(ErrorMessage = "The Main Genre field is required.")]
        public string MainGenreId { get; set; } = null!;

        public virtual List<GenreViewModel>? Genres { get; set; }
            = new List<GenreViewModel>();

        public bool IsConfirmed { get; set; }

    }
}
