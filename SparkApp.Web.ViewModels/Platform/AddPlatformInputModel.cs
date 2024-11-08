
using System.ComponentModel.DataAnnotations;

namespace SparkApp.Web.ViewModels.Platform
{
    public class AddPlatformInputModel
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
