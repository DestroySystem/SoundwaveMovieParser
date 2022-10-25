using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreData.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameRu { get; set; }
    }

    public class Genres
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameRu { get; set; }
    }

    public class Images
    {
        [Key]
        public Guid Id { get; set; }
        public string? ContentType { get; set; }
        public string? Base64ImageContent { get; set; }
        public string? Link { get; set; }
    }

    public class Movies
    {
        [Key]
        public Guid Id { get; set; }
        public string? OriginalName { get; set; }
        public string? NameRu { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Quality { get; set; }
        public string? Age { get; set; }
        public string? Duration { get; set; }
        public string? FromTheSeries { get; set; }
        public string? Link { get; set; }
        [ForeignKey("ImageID")]
        public Guid Image { get; set; }
        [ForeignKey("CategoryID")]
        public int Category { get; set; }
        public string? Country { get; set; }
    }

    public class GenresToMovie
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("MovieID")]
        public Guid Movie { get; set; }
        [ForeignKey("GenreID")]
        public int Genre { get; set; }
    }

    public class CategoryToGenres
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CategoryID")]
        public int Category { get; set; }
        [ForeignKey("GenreID")]
        public int Genre { get; set; }
    }
}
