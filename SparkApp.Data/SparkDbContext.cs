using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SparkApp.Data.Models;

namespace SparkApp.Data
{



    public class SparkDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {

        public SparkDbContext()
        {

        }

        public SparkDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<Platform> Platforms { get; set; }

        public virtual DbSet<Developer> Developer { get; set; }

        public virtual DbSet<Director> Directors { get; set; }

        public virtual DbSet<GameGenre> GamesGenres { get; set; }

        
        public virtual DbSet<GamePlatform> GamesPlatforms { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameGenre>()
                .HasKey(gsg => new { gsg.GameId, gsg.GenreId });

            modelBuilder.Entity<GameGenre>()
                .HasKey(gsg => new { gsg.GameId, gsg.GenreId });

            modelBuilder.Entity<Game>()
                .HasOne(ga => ga.MainGenre)
                .WithMany()
                .HasForeignKey(ga => ga.MainGenreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GamePlatform>()
                .HasKey(gp => new { gp.GameId, gp.PlatformId });

        
        }
    }
}