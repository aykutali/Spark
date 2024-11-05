using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SparkApp.Common;

namespace SparkApp.Data.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(EntityValidationConstants.Genre.NameMinLength)]
        [MaxLength(EntityValidationConstants.Genre.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(EntityValidationConstants.Genre.DescriptionMinLength)]
        [MaxLength(EntityValidationConstants.Genre.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
            = new List<Game>();
    }
}