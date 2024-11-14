
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Web.ViewModels.Genre
{
	public class GenreCheckBoxInputModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;

		public bool IsSelected { get; set; } = false;
	}
}
