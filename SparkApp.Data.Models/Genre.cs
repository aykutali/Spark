using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
            = new List<Game>();
    }
}