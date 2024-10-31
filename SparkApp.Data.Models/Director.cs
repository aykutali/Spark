using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SparkApp.Common;

namespace SparkApp.Data.Models
{
    public class Director
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MinLength(EntityValidationConstants.Director.NameMinLength)]
        [MaxLength(EntityValidationConstants.Director.NameMaxLength)]
        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [MinLength(EntityValidationConstants.Director.AboutMinLength)]
        [MaxLength(EntityValidationConstants.Director.AboutMaxLength)]
        public string? About { get; set; }

        public virtual ICollection<Game> Games { get; set; }
            = new List<Game>();
    }
}