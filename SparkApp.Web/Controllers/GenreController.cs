using Microsoft.AspNetCore.Mvc;
using SparkApp.Data;
using SparkApp.Data.Models;
using SparkApp.Web.ViewModels.Genre;

namespace SparkApp.Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly SparkDbContext db;

        public GenreController(SparkDbContext _db)
        {
            db= _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddGenreInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var genreData = new Genre
            {
                Name = model.Name,
                Description = model.Description
            };

            db.Genres.Add(genreData);
            db.SaveChanges();

            return View(nameof(Index));
        }
    }
}
