using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using SparkApp.Common;

namespace SparkApp.Data.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(EntityValidationConstants.Game.TitleMinLength)]
        [MaxLength(EntityValidationConstants.Game.TitleMaxLength)]
        [Comment("The title of the game")]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(EntityValidationConstants.Game.DescriptionMinLength)]
        [MaxLength(EntityValidationConstants.Game.DescriptionMaxLength)]
        [Comment("Description of the game")]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        [Comment("Release date of the game")]
        public DateTime ReleaseDate { get; set; }

        [Comment("Soft delete option for a game")]
        public bool IsDeleted { get; set; } = false;

        [Comment("IsConfirmed is showing if the game is confirmed by admin for a valid game or not")]
        public bool IsConfirmed { get; set; } = false;

        [Required]
        [Comment("Developer team/studio of the game")]
        public Guid DeveloperId { get; set; }

        [Required]
        [ForeignKey(nameof(DeveloperId))]
        public virtual Developer Developer { get; set; } = null!;

        [Required]
        [Comment("Lead game director of the game")]
        public Guid LeadGameDirectorId { get; set; }

        [Required]
        [ForeignKey(nameof(LeadGameDirectorId))]
        public virtual Director LeadGameDirector { get; set; } = null!;

        public virtual ICollection<GamePlatform> GamePlatforms { get; set; }
            = new List<GamePlatform>();

        [Required]
        public Guid MainGenreId { get; set; }

        [Required]
        [ForeignKey(nameof(MainGenreId))]
        public virtual Genre MainGenre { get; set; } = null!;

        public virtual ICollection<GameGenre> SideGenres { get; set; }
            = new List<GameGenre>();
    }
}