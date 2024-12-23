using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SparkApp.Data;
using SparkApp.Data.Models;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.Infrastructure.Extensions;

namespace SparkApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            string adminEmail = builder.Configuration.GetValue<string>("Admin:Email")!;
            string adminUsername = builder.Configuration.GetValue<string>("Admin:Username")!;
            string adminPassword = builder.Configuration.GetValue<string>("Admin:Password")!;

            string jsonPathGenres = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	            builder.Configuration.GetValue<string>("Seed:GenresJson")!);
            string jsonPathDevelopers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	            builder.Configuration.GetValue<string>("Seed:DevsJson")!);
            string jsonPathDirectors = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	            builder.Configuration.GetValue<string>("Seed:DirectorsJson")!);
            string jsonPathPlatforms = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	            builder.Configuration.GetValue<string>("Seed:PlatformsJson")!); 
            string jsonPathGames = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	            builder.Configuration.GetValue<string>("Seed:GamesJson")!);
            string jsonPathGamesGenres = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	            builder.Configuration.GetValue<string>("Seed:GamesGenresJson")!);
            string jsonPathGamesPlatforms = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
	            builder.Configuration.GetValue<string>("Seed:GamesPlatformsJson")!);

			builder.Services
                .AddDbContext<SparkDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<SparkDbContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/Identity/Account/Login";
            });

            builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);
            builder.Services.RegisterUserDefinedServices(typeof(IGenreService).Assembly);

            builder.Services.AddControllersWithViews(cfg =>
            {
	            cfg.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

			builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

            app.ApplyMigrations();
            if (app.Environment.IsDevelopment())
            {
				app.SeedAdministrator(adminEmail, adminUsername, adminPassword);
				app.SeedDatabase(jsonPathGenres, 
								jsonPathDevelopers, 
								jsonPathDirectors, 
								jsonPathPlatforms,
								jsonPathGames,
								jsonPathGamesGenres,
								jsonPathGamesPlatforms);
            }
            app.MapControllerRoute(
	            name: "Areas",
	            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

			app.MapControllerRoute(
				name: "Errors",
				pattern: "{controller=Home}/{action=Index}/{statusCode?}");

			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

			app.MapRazorPages();

            app.Run();
        }

        
    }
}