
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparkApp.Data.Models
{
	public class GameOfTheDay
	{
		[Key]
		public DateOnly Day { get; set; }

		public Guid GameId { get; set; }

		[ForeignKey(nameof(GameId))] 
		public virtual Game TheGame { get; set; } = null!;
	}
}
