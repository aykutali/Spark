using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SparkApp.Data.Models;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.ViewModels.User;

namespace SparkApp.Services.Data
{
	public class UserService : BaseService, IUserService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole<Guid>> roleManager;

		public UserService(UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole<Guid>> roleManager)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
		}

		public async Task<IEnumerable<UserAllViewModel>> GetAllUsersAsync()
		{
			IEnumerable<ApplicationUser> allUsers = await this.userManager.Users
				.ToArrayAsync();
			ICollection<UserAllViewModel> allUsersViewModel = new List<UserAllViewModel>();

			foreach (ApplicationUser user in allUsers)
			{
				IList<string> roles = await this.userManager.GetRolesAsync(user);

				allUsersViewModel.Add(new UserAllViewModel()
				{
					Id = user.Id.ToString(),
					UserName = user.UserName,
					Roles = roles
				});
			}

			return allUsersViewModel;
		}

		public async Task<bool> UserExistsByIdAsync(Guid userId)
		{
			ApplicationUser? user = await this.userManager
				.FindByIdAsync(userId.ToString());

			return user != null;
		}

		public async Task<bool> AssignUserToRoleAsync(Guid userId, string roleName)
		{
			ApplicationUser? user = await userManager
				.FindByIdAsync(userId.ToString());
			bool roleExists = await this.roleManager.RoleExistsAsync(roleName);

			if (user == null || !roleExists)
			{
				return false;
			}

			bool alreadyInRole = await this.userManager.IsInRoleAsync(user, roleName);
			if (!alreadyInRole)
			{
				IdentityResult? result = await this.userManager
					.AddToRoleAsync(user, roleName);

				if (!result.Succeeded)
				{
					return false;
				}
			}

			return true;
		}

		public async Task<bool> RemoveUserRoleAsync(Guid userId, string roleName)
		{
			ApplicationUser? user = await userManager
				.FindByIdAsync(userId.ToString());
			bool roleExists = await this.roleManager.RoleExistsAsync(roleName);

			if (user == null || !roleExists)
			{
				return false;
			}

			bool alreadyInRole = await this.userManager.IsInRoleAsync(user, roleName);
			if (alreadyInRole)
			{
				IdentityResult? result = await this.userManager
					.RemoveFromRoleAsync(user, roleName);

				if (!result.Succeeded)
				{
					return false;
				}
			}

			return true;
		}

		public async Task<bool> DeleteUserAsync(Guid userId)
		{
			ApplicationUser? user = await userManager
				.FindByIdAsync(userId.ToString());

			if (user == null)
			{
				return false;
			}

			IdentityResult? result = await this.userManager
				.DeleteAsync(user);
			if (!result.Succeeded)
			{
				return false;
			}

			return true;
		}
	}
}

