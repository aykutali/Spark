using System.ComponentModel.DataAnnotations;

namespace SparkApp.Data.Models
{
    public class Platform
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<GamePlatform> GamesPlatform { get; set; }
            = new List<GamePlatform>();
    }
}