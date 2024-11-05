using SparkApp.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
