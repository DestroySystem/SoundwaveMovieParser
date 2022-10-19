using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace EFCoreModels
{
    public class MovieContext : DbContext
    {
        public DbSet<Genres>? Genres { get; set; }
        public DbSet<Categories>? Categories { get; set; }
        public DbSet<Images>? Images { get; set; }
        public DbSet<Countries>? Countries { get; set; }
        public DbSet<GenresToMovie>? GenresToMovies { get; set; }
        public DbSet<Movies>? Movies { get; set; }

        public string? DbPath { get; }

        public MovieContext()
        {
            string folder = @"C:\SoundwaveMovieParser\SoundwaveMovieParser\Resources\Data";
            DbPath = Path.Join(folder, "SoundwaveMovieDb.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Categories
    {
        public byte Id { get; set; }
        public string? Name { get; set; }
        public string? Name_ru { get; set; }
    }

    public class Genres
    {
        public byte Id { get; set; }
        public string? Name { get; set; }
        public string? Name_ru { get; set; }
        public Categories? Category { get; set; }
    }

    public class Images
    {
        public Guid Id { get; set; }
        public string? ContentType { get; set; }
        public string? Base64ImageContent { get; set; }
        public string? Link { get; set; }
    }

    public class Countries
    {
        public byte Id { get; set; }
        public string? Name { get; set; }
        public string? Name_ru { get; set; }
    }

    public class Movies
    {
        public Guid Id { get; set; }
        public string? OriginalName { get; set; }
        public string? Name_ru { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Quality { get; set; }
        public string? Age { get; set; }
        public string? Duration { get; set; }
        public string? FromTheSeries { get; set; }
        public string? Link { get; set; }
        public Images? Image { get; set; }
        public Categories? Category { get; set; }
        public Countries? Country { get; set; }
    }

    public class GenresToMovie
    {
        public Guid Id { get; set; }
        public Movies? Movie { get; set; }
        public Genres? Genre { get; set; }
    }
}
