using System.ComponentModel.DataAnnotations;

using SparkApp.Common;

namespace SparkApp.Data.Models
{
    public class Developer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(EntityValidationConstants.Developer.NameMinLength)]
        [MaxLength(EntityValidationConstants.Developer.NameMaxLength)]
        public string Name { get; set; } = null!;

        public string? LogoUrl { get; set; }

        public virtual ICollection<Game> Games { get; set; }
            = new List<Game>();
    }
}