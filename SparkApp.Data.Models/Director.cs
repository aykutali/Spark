using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Models
{
    public class Director
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string? About { get; set; }

        public virtual ICollection<Game> Games { get; set; }
            = new List<Game>();
    }
}