using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static SparkApp.Common.AppConstants;

namespace SparkApp.Web.Areas.Admin.Controllers
{
	[Area(AdminRoleName)]
	[Authorize(Roles = AdminRoleName)]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
