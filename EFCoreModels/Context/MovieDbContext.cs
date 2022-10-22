using EFCoreModels.Models;

using Microsoft.EntityFrameworkCore;

namespace EFCoreModels.Context
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Genres>? Genres { get; set; }
        public DbSet<Categories>? Categories { get; set; }
        public DbSet<Images>? Images { get; set; }
        public DbSet<Countries>? Countries { get; set; }
        public DbSet<GenresToMovie>? GenresToMovies { get; set; }
        public DbSet<Movies>? Movies { get; set; }
        public DbSet<CategoryToGenres>? CategoryToGenres { get; set; }

        public string? DbPath { get; }

        public MovieDbContext()
        {
            const string folder = @"C:\SoundwaveMovieParser\Resources\Data";
            DbPath = Path.Join(folder, "SoundwaveMovieDb.db");
        }

        public MovieDbContext(DbContextOptions<MovieDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Genres>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Categories>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Movies>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Images>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Countries>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<GenresToMovie>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<CategoryToGenres>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
