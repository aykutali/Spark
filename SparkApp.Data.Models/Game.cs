using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparkApp.Data.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsConfirmed { get; set; } = false;

        [Required]
        public Guid DeveloperId { get; set; }

        [Required]
        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; } = null!;

        public Guid LeadGameDirectorId { get; set; }

        [ForeignKey(nameof(LeadGameDirectorId))]
        public Director? LeadGameDirector { get; set; }

        [Required]
        public Guid MainGenreId { get; set; }

        [Required]
        [ForeignKey(nameof(MainGenreId))]
        public virtual Genre MainGenre { get; set; } = null!;

        public virtual ICollection<Genre> SideGenres { get; set; }
            = new List<Genre>();

        public virtual ICollection<Review> Reviews { get; set; }
            = new List<Review>();
    }
}