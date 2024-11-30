using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkApp.Web.Models;
using System.Diagnostics;

namespace SparkApp.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        

        public IActionResult Error(int? statusCode = null)
		{
			// TODO: Add other pages
			if (!statusCode.HasValue)
			{
				return this.View();
			}

			if (statusCode == 404)
			{
				return this.View("Error404");
			}
			else if (statusCode == 401 || statusCode == 403)
			{
				return this.View("Error403");
			}

			return this.View("Error500");
		}
    }
}
