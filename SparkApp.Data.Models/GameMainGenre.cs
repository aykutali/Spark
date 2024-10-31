using System.ComponentModel.DataAnnotations.Schema;

namespace SparkApp.Data.Models
{
    public class GameMainGenre
    {
        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;
        public Guid GameId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; } = null!;
        public Guid GenreId { get; set; }
    }
}