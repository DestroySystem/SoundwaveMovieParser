using System.Collections.Generic;
using System.Reflection;
using CommonModels;
using EFCoreData.Context;
using EFCoreData.Models;

using Newtonsoft.Json;

namespace MovieCatalog.DbData
{
    public class DataRetrieval
    {
        private readonly MovieDbContext _context;
        private readonly ILogger<DataRetrieval> _logger;

        public DataRetrieval(MovieDbContext context, ILogger<DataRetrieval> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MovieModel> LoadMovieFromDb(string id)
        {
            MovieModel movieDetails = new();
            try
            {
                if (_context.Movies != null)
                {
                    Movies movie = _context.Movies.First(x => x.Id.ToString() == id.ToUpper());
                    List<int> genresToMovie = _context.GenresToMovies.Where(x => x.Movie == movie.Id)
                        .Select(x => x.Genre).ToList();
                    foreach (int genre in genresToMovie)
                    {
                        movieDetails.Id = movie.Id;
                        movieDetails.ReleaseDate = movie.ReleaseDate;
                        movieDetails.OriginalName = movie.OriginalName;
                        movieDetails.NameRu = movie.NameRu;
                        movieDetails.Genres = _context.Genres.Where(x => x.Id == genre).Select(x => x.Name).ToList();
                        movieDetails.Category = _context.Categories.Where(x => x.Id == movie.Category).Select(x => x.Name).ToString();
                        movieDetails.Country = movie.Country;
                        movieDetails.Duration = movie.Duration;
                        movieDetails.Age = movie.Age;
                        movieDetails.Quality = movie.Quality;
                        movieDetails.FromTheSeries = movie.FromTheSeries;
                        movieDetails.CoverImage.Path = _context.Images
                            .Where(x => x.Id.ToString() == movie.Image.ToString()).ToString();

                    }
                }
            }
            catch (Exception err)
            {
                _logger.LogError($"Error: {err} Message: {err.Message} From: {MethodBase.GetCurrentMethod()?.Name}");
            }

            return movieDetails;
        }

        public async Task<MovieModel> LoadMovieFromFile(string id, string category, string path)
        {
            MovieModel movieModels = new();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Length != 0)
                {
                    FileInfo fileInfo = new(file);
                    if (fileInfo.Exists && fileInfo.FullName.Contains(category) && fileInfo.Extension == ".json")
                    {
                        string? json = await System.IO.File.ReadAllTextAsync(file);
                        MovieDetailsModel detailsModel = JsonConvert.DeserializeObject<MovieDetailsModel>(json);
                        foreach (MovieModel item in from movie in detailsModel?.Models where movie.Id.ToString() == id select movie)
                        {
                            movieModels.Id = item.Id;
                            movieModels.Category = item.Category;
                            movieModels.Age = item.Age;
                            movieModels.Country = item.Country;
                            movieModels.Duration = item.Duration;
                            movieModels.FromTheSeries = item.FromTheSeries;
                            movieModels.Genres = item.Genres;
                            movieModels.Quality = item.Quality;
                            movieModels.NameRu = item.NameRu;
                            movieModels.OriginalName = item.OriginalName;
                            movieModels.Link = item.Link;
                            movieModels.CoverImage.Path = item.CoverImage.Path;
                            movieModels.ReleaseDate = item.ReleaseDate;
                        }
                    }
                }
            }

            return movieModels;
        }
    }
}
