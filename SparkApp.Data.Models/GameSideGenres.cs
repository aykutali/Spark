using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkApp.Data.Models
{
    public class GameSideGenre
    {
        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;
        public Guid GameId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; } = null!;
        public Guid GenreId { get; set; }
    }
}