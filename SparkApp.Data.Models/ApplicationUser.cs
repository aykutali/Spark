using Microsoft.AspNetCore.Identity;

namespace SparkApp.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }

        public int GameOfTheDayStreak { get; set; }
    }
}
