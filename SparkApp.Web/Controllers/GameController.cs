using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkApp.Data;
using SparkApp.Data.Models;

namespace SparkApp.Web.Controllers
{
    public class GameController : BaseController
    {
        private readonly SparkDbContext db;
        public GameController(SparkDbContext db)
        {
            this.db = db;
        }

        public IActionResult All()
        {
            List<Game> games = db.Games
                .Include(g=> g.LeadGameDirector)
                .Include(g=> g.Developer)
                .Include(g => g.MainGenre)
                .ToList();

            return View();
        }
    }
}
