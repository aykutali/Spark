using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Models
{
    public class Developer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        public string? LogoUrl { get; set; }

        public virtual ICollection<Game> Games { get; set; }
            = new List<Game>();
    }
}