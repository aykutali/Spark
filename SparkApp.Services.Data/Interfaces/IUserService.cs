
using SparkApp.Web.ViewModels.User;

namespace SparkApp.Services.Data.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserAllViewModel>> GetAllUsersAsync();

		Task<bool> UserExistsByIdAsync(Guid userId);

		Task<bool> AssignUserToRoleAsync(Guid userId, string roleName);

		Task<bool> RemoveUserRoleAsync(Guid userId, string roleName);

		Task<bool> DeleteUserAsync(Guid userId);
	}
}
