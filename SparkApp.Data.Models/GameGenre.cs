
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace SparkApp.Data.Models
{
    public class GameGenre
    {
        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;
        public Guid GameId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; } = null!;
        public Guid GenreId { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}