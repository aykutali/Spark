using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkApp.Data;
using SparkApp.Data.Models;

namespace SparkApp.Web.Controllers
{
    public class GameController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
