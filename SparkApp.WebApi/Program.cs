using Microsoft.EntityFrameworkCore;

using SparkApp.Data;
using SparkApp.Data.Models;
using SparkApp.Services.Data.Interfaces;
using SparkApp.Web.Infrastructure.Extensions;

namespace SparkApp.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			builder.Services
				.AddDbContext<SparkDbContext>(options =>
				{
					options.UseSqlServer(connectionString);
				});


			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddCors(cfg =>
			{
				cfg.AddPolicy("AllowAll", policyBld =>
				{
					policyBld
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowAnyOrigin();
				});
			});

			builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);
			builder.Services.RegisterUserDefinedServicesWebApi(typeof(IGuessGameService).Assembly);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseCors("AllowAll");

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
