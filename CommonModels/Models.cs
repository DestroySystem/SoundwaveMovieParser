using System.Text.Json.Serialization;

namespace CommonModels
{
    public class MovieModel
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("NameRu")]
        public string? NameRu { get; set; }

        [JsonPropertyName("OriginalName")]
        public string? OriginalName { get; set; }

        [JsonPropertyName("ReleaseDate")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("Country")]
        public string? Country { get; set; }

        [JsonPropertyName("Quality")]
        public string? Quality { get; set; }

        [JsonPropertyName("Age")]
        public string? Age { get; set; }

        [JsonPropertyName("Category")]
        public string? Category { get; set; }

        [JsonPropertyName("Genres")]
        public List<string>? Genres { get; set; }

        [JsonPropertyName("Duration")]
        public string? Duration { get; set; }

        [JsonPropertyName("FromTheSeries")]
        public string? FromTheSeries { get; set; }

        [JsonPropertyName("Link")]
        public string? Link { get; set; }

        [JsonPropertyName("CoverImage")]
        public Image? CoverImage { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("ContentType")]
        public string? ContentType { get; set; }

        [JsonPropertyName("Path")]
        public string? Path { get; set; }
    }

    public class MovieDetailsModel
    {
        public MovieDetailsModel() => Models = new List<MovieModel>();

        public List<MovieModel> Models { get; set; }
    }
}
