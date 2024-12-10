using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.Controllers;

using static SparkApp.Common.AppConstants;

namespace SparkApp.Web.Areas.Admin.Controllers
{
	[Area(AdminRoleName)]
	[Authorize(Roles = AdminRoleName)]
	public class UserManagementController : BaseController
	{
		private readonly IUserService userService;

		public UserManagementController(IUserService userService)
		{
			this.userService = userService;
		}


		public async Task<IActionResult> Index()
		{
			var users = await userService.GetAllUsersAsync();

			return View(users);
		}
		
		public async Task<IActionResult> MakeTheUserMod(string userId)
		{
			Guid userGuid = Guid.Empty;
			if (!IsGuidValid(userId, ref userGuid))
			{
				return RedirectToAction(nameof(Index));
			}

			if (!await userService.UserExistsByIdAsync(userGuid))
			{
				return RedirectToAction(nameof(Index));
			}

			string role = ModRoleName.ToUpper();
			await userService.AssignUserToRoleAsync(userGuid, role);

			return RedirectToAction(nameof(Index));
		}
		
		public async Task<IActionResult> UnModTheUser(string userId)
		{
			Guid userGuid = Guid.Empty;
			if (!IsGuidValid(userId, ref userGuid))
			{
				return RedirectToAction(nameof(Index));
			}

			if (!await userService.UserExistsByIdAsync(userGuid))
			{
				return RedirectToAction(nameof(Index));
			}

			string role = ModRoleName.ToUpper();
			await userService.RemoveUserRoleAsync(userGuid, role);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> DeleteUser(string userId)
		{
			Guid userGuid = Guid.Empty;
			if (!IsGuidValid(userId, ref userGuid))
			{
				return RedirectToAction(nameof(Index));
			}

			if (!await userService.UserExistsByIdAsync(userGuid))
			{
				return RedirectToAction(nameof(Index));
			}

			await userService.DeleteUserAsync(userGuid);

			return RedirectToAction(nameof(Index));
		}
	}
}

