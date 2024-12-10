using SparkApp.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using SparkApp.Data.Models;
using SparkApp.Data.Seeding;
using static SparkApp.Common.AppConstants;

namespace SparkApp.Web.Infrastructure.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
		{
			using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

			SparkDbContext dbContext = serviceScope
				.ServiceProvider
				.GetRequiredService<SparkDbContext>()!;
			dbContext.Database.Migrate();

			return app;
		}

		public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app, string jsonPathGenres,
			string jsonPathDevelopers,
			string jsonPathDirectors,
			string jsonPathPlatforms,
			string jsonPathGames,
			string jsonPathGamesGenres,
			string jsonPathGamesPlatforms)
		{
			using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
			IServiceProvider serviceProvider = serviceScope.ServiceProvider;


			Task.Run(async () =>
			{
				await DbSeeder.SeedDb(serviceProvider, jsonPathGenres,
														jsonPathDevelopers,
														jsonPathDirectors,
														jsonPathPlatforms,
														jsonPathGames,
														jsonPathGamesGenres,
														jsonPathGamesPlatforms);
			})
				.GetAwaiter()
				.GetResult();

			return app;
		}

		public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email, string username, string password)
		{
			using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
			IServiceProvider serviceProvider = serviceScope.ServiceProvider;

			RoleManager<IdentityRole<Guid>>? roleManager = serviceProvider
				.GetService<RoleManager<IdentityRole<Guid>>>();
			IUserStore<ApplicationUser>? userStore = serviceProvider
				.GetService<IUserStore<ApplicationUser>>();
			UserManager<ApplicationUser>? userManager = serviceProvider
				.GetService<UserManager<ApplicationUser>>();

			if (roleManager == null)
			{
				throw new ArgumentNullException(nameof(roleManager),
					$"Service for {typeof(RoleManager<IdentityRole<Guid>>)} cannot be obtained!");
			}

			if (userStore == null)
			{
				throw new ArgumentNullException(nameof(userStore),
					$"Service for {typeof(IUserStore<ApplicationUser>)} cannot be obtained!");
			}

			if (userManager == null)
			{
				throw new ArgumentNullException(nameof(userManager),
					$"Service for {typeof(UserManager<ApplicationUser>)} cannot be obtained!");
			}

			Task.Run(async () =>
			{
				bool roleExists = await roleManager.RoleExistsAsync(AdminRoleName);
				IdentityRole<Guid>? adminRole = null;
				if (!roleExists)
				{
					adminRole = new IdentityRole<Guid>(AdminRoleName);

					IdentityResult result = await roleManager.CreateAsync(adminRole);
					if (!result.Succeeded)
					{
						throw new InvalidOperationException($"Error occurred while creating the {AdminRoleName} role!");
					}
				}
				else
				{
					adminRole = await roleManager.FindByNameAsync(AdminRoleName);
				}

				bool modRoleExist = await roleManager.RoleExistsAsync(ModRoleName);
				IdentityRole<Guid>? modRole = null;
				if (!modRoleExist)
				{
					modRole = new IdentityRole<Guid>(ModRoleName);

					IdentityResult result = await roleManager.CreateAsync(modRole);
					if (!result.Succeeded)
					{
						throw new InvalidOperationException($"Error occurred while creating the {ModRoleName} role!");
					}
				}
				else
				{
					modRole = await roleManager.FindByNameAsync(ModRoleName);
				}

				ApplicationUser? adminUser = await userManager.FindByEmailAsync(email);
				if (adminUser == null)
				{
					adminUser = await
						CreateAdminUserAsync(email, username, password, userStore, userManager);
				}

				if (await userManager.IsInRoleAsync(adminUser, AdminRoleName) && await userManager.IsInRoleAsync(adminUser, ModRoleName))
				{
					return app;
				}

				if (!await userManager.IsInRoleAsync(adminUser,AdminRoleName))
				{
					IdentityResult userResult = await userManager.AddToRoleAsync(adminUser, AdminRoleName);
					if (!userResult.Succeeded)
					{
						throw new InvalidOperationException($"Error occurred while adding the user {username} to the {AdminRoleName} role!");
					}
				}

				if (!await userManager.IsInRoleAsync(adminUser, ModRoleName))
				{
					IdentityResult userResult = await userManager.AddToRoleAsync(adminUser, ModRoleName);
					if (!userResult.Succeeded)
					{
						throw new InvalidOperationException($"Error occurred while adding the user {username} to the {ModRoleName} role!");
					}
				}
				

				return app;
			})
				.GetAwaiter()
				.GetResult();

			return app;
		}

		private static async Task<ApplicationUser> CreateAdminUserAsync(string email, string username, string password,
			IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
		{
			ApplicationUser applicationUser = new ApplicationUser
			{
				Email = email
			};

			await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);
			IdentityResult result = await userManager.CreateAsync(applicationUser, password);
			if (!result.Succeeded)
			{
				throw new InvalidOperationException($"Error occurred while registering {AdminRoleName} user!");
			}

			return applicationUser;
		}
	}
}
