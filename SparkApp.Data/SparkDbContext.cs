using System.Reflection;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SparkApp.Data.Models;

namespace SparkApp.Data
{



    public class SparkDbContext : IdentityDbContext
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

        public virtual DbSet<GameSideGenre> GameSideGenres { get; set; }

        public virtual DbSet<GameMainGenre> GameMainGenre { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameSideGenre>()
                .HasKey(gsg => new { gsg.GameId, gsg.GenreId });

            modelBuilder.Entity<GameMainGenre>()
                .HasKey(gsg => new { gsg.GameId, gsg.GenreId });

            modelBuilder.Entity<Game>()
                .HasOne(ga => ga.MainGenre)
                .WithMany()
                .HasForeignKey(ga => ga.MainGenreId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}