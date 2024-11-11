using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SparkApp.Data.Models
{
    [Table("GamesGenres")]
    public class GameGenre
    {
        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;
        public Guid GameId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; } = null!;
        public Guid GenreId { get; set; }

        [Comment("Shows is the genre is a main or sub(secondary) of the game")]
        public bool IsSubGenre { get; set; } = false;
    }
}