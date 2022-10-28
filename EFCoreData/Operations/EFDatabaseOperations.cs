using System.Reflection;

using CommonModels;

using EFCoreData.Context;
using EFCoreData.Models;

using HtmlParser.Common;
using HtmlParser.Dictionary;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace EFCoreData.Operations
{
    // ReSharper disable once InconsistentNaming
    public class EFDatabaseOperations : IEFCoreDataOperations
    {
        private readonly Utils _utils;
        private readonly ILogger<EFDatabaseOperations> _logger;
        private readonly MovieDbContext _context;


        public EFDatabaseOperations(Utils utils, MovieDbContext context, ILogger<EFDatabaseOperations> logger)
        {
            _utils = utils;
            _context = context;
            _logger = logger;
        }

        public void AddCategory(List<string> category)
        {
            foreach (string item in category)
            {
                _context.Add(new Categories()
                {
                    Name = item
                });
            }
            _context.SaveChanges();
        }

        public void AddGenres()
        {
            CategoriesDictionary dictionary = new CategoriesDictionary();
            Dictionary<string, List<string>> categories = dictionary.Categories();
            List<string> distGenres = new();
            foreach (var itemCategoriesKey in categories.Keys)
            {
                var genres = categories[itemCategoriesKey];
                foreach (var item in genres)
                {
                    if (!distGenres.Contains(item))
                        distGenres.Add(item);
                }
            }

            foreach (string item in distGenres)
            {
                _context.Add(new Genres()
                {
                    Name = item
                });
            }

            _context.SaveChanges();
        }

        public void AddCategoryToGenres()
        {
            CategoriesDictionary dictionary = new CategoriesDictionary();
            Dictionary<string, List<string>> catDictionary = dictionary.Categories();

            try
            {
                List<Categories> categories = _context.Categories.OrderBy(b => b.Id).ToList();
                List<Genres> genres = _context.Genres.OrderBy(b => b.Id).ToList();

                foreach (var category in categories)
                {
                    foreach (var genre in genres)
                    {
                        if (catDictionary.ContainsKey(category.Name) && catDictionary[category.Name].Contains(genre.Name))
                        {
                            _context.Add(new CategoryToGenres()
                            {
                                Category = category.Id,
                                Genre = genre.Id
                            });
                        }
                    }
                }

                _context.SaveChanges();
            }
            catch (ObjectDisposedException err)
            {
                _logger.LogError($"Error: {err}, Message: {err.Message} From: {MethodBase.GetCurrentMethod()?.Name}", err);
            }
            catch (Exception err)
            {
                _logger.LogError($"Error: {err}, Message: {err.Message} From: {MethodBase.GetCurrentMethod()?.Name}", err);
            }

        }

        public async Task CheckRecordNotExist(string data)
        {
            bool isDataEmpty = false;
            MovieDetailsModel detailsModel = JsonConvert.DeserializeObject<MovieDetailsModel>(data);
            foreach (MovieModel movie in detailsModel.Models)
            {
                isDataEmpty = _context.Movies.FirstOrDefault(x => x.Id == movie.Id) == null;


                int moviesCount = _context.Movies.Count();
                if (moviesCount == 0)
                {
                    await InsertMoviesFromJson(data);
                }
                else
                {
                    if (isDataEmpty)
                    {
                        await InsertMovieFromModel(movie);
                    }
                }
            }
        }


        public async Task InsertMovieFromModel(MovieModel model)
        {
            IQueryable<int> category = _context.Categories.Where(c => c.Name == model.Category).Select(c => c.Id);

            _context.Add(new Images
            {
                ContentType = model?.CoverImage?.ContentType,
                Link = model?.CoverImage?.Path,
                Base64ImageContent = await _utils.FromImageToBase64(model?.CoverImage?.Path)

            });
            await _context.SaveChangesAsync();

            IQueryable<Guid> image = _context.Images.Where(x => x.Link == model.CoverImage.Path).Select(x => x.Id);

            _context.Add(new Movies()
            {
                Id = model.Id,
                OriginalName = model.OriginalName,
                NameRu = model.NameRu,
                Age = model.Age,
                Country = model.Country,
                ReleaseDate = model.ReleaseDate,
                Category = category.FirstOrDefault(),
                Quality = model.Quality,
                Duration = model.Duration,
                FromTheSeries = model.FromTheSeries,
                Link = model.Link,
                Image = image.FirstOrDefault()
            });
            await _context.SaveChangesAsync();

            IQueryable<Guid> movieId = _context.Movies.Where(x => x.OriginalName == model.OriginalName).Select(y => y.Id);

            foreach (var genreId in model.Genres.Select(genre => _context.Genres.Where(x => x.Name == genre).Select(y => y.Id)))
            {
                _context.Add(new GenresToMovie()
                {
                    Movie = movieId.FirstOrDefault(),
                    Genre = genreId.FirstOrDefault(),
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task InsertMoviesFromJson(string data)
        {
            int count = 0;
            MovieDetailsModel movieDetails = JsonConvert.DeserializeObject<MovieDetailsModel>(data);
            foreach (MovieModel movie in movieDetails.Models)
            {
                IQueryable<int> cat = _context.Categories.Where(category => category.Name == movie.Category)
                    .Select(category => category.Id);

                _context.Add(new Images
                {
                    ContentType = movie?.CoverImage?.ContentType,
                    Link = movie?.CoverImage?.Path,
                    Base64ImageContent = await _utils.FromImageToBase64(movie?.CoverImage?.Path)

                });
                await _context.SaveChangesAsync();

                IQueryable<Guid> image = _context.Images.Where(x => x.Link == movie.CoverImage.Path).Select(x => x.Id);

                _context.Add(new Movies()
                {
                    Id = movie.Id,
                    OriginalName = movie.OriginalName,
                    NameRu = movie.NameRu,
                    Age = movie.Age,
                    Country = movie.Country,
                    ReleaseDate = movie.ReleaseDate,
                    Category = cat.FirstOrDefault(),
                    Quality = movie.Quality,
                    Duration = movie.Duration,
                    FromTheSeries = movie.FromTheSeries,
                    Link = movie.Link,
                    Image = image.FirstOrDefault()
                });
                await _context.SaveChangesAsync();

                IQueryable<Guid> movieId = _context.Movies.Where(x => x.OriginalName == movie.OriginalName).Select(y => y.Id);

                foreach (var genreId in movie.Genres.Select(genre => _context.Genres.Where(x => x.Name == genre).Select(y => y.Id)))
                {
                    _context.Add(new GenresToMovie()
                    {
                        Movie = movieId.FirstOrDefault(),
                        Genre = genreId.FirstOrDefault(),
                    });
                    await _context.SaveChangesAsync();
                }

                Console.Write($"\r{++count}/{movieDetails.Models.Count} {count * 100 / movieDetails.Models.Count:0.0}%");
            }
        }

        public void ReadGenres()
        {
            if (_context.Genres != null)
            {
                List<Genres> genres = _context.Genres.OrderBy(b => b.Id).ToList();
                foreach (var item in genres)
                {
                    Console.WriteLine($"{item.Id} {item.Name} {item.NameRu}");
                }
            }
        }

        public void ReadCategories()
        {
            if (_context.Categories != null)
            {
                List<Categories> genres = _context.Categories.OrderBy(b => b.Id).ToList();
                foreach (var item in genres)
                {
                    Console.WriteLine($"{item.Id} {item.Name} {item.NameRu}");
                }
            }
        }

        public void ReadCategoriesToGenres()
        {
            if (_context.CategoryToGenres != null)
            {
                List<CategoryToGenres> categoryToGenres = _context.CategoryToGenres.OrderBy(b => b.Id).ToList();
                foreach (var item in categoryToGenres)
                {
                    Categories categories = _context.Categories.First(x => x.Id == item.Category);
                    Genres genres = _context.Genres.First(x => x.Id == item.Genre);
                    Console.WriteLine($"Category: {categories.Name} Genre: En({genres.Name})  Ru({genres.NameRu})");
                }
            }
        }

        public void ReadMovies()
        {
            if (_context.Categories != null)
            {
                List<Movies> genres = _context.Movies.OrderBy(b => b.Id).ToList();
                foreach (var item in genres)
                {
                    Console.WriteLine($"{item.OriginalName} / {item.NameRu} / {item.Country} / {item.ReleaseDate} / {item.Category} / {item.Duration}");
                }
            }
        }

        public void TranslateGenres()
        {
            CategoriesDictionary dictionary = new CategoriesDictionary();
            Dictionary<string, string> translate = dictionary.EnglishNameToRussian();
            List<Genres> genres = _context.Genres.OrderBy(b => b.Id).ToList();

            foreach (var genre in genres)
            {
                Genres gen = _context.Genres.First(x => x.Name == genre.Name);
                foreach (var item in translate.Keys)
                {
                    if (item == genre.Name)
                    {
                        gen.NameRu = translate[item];
                    }
                }
                _context.SaveChanges();
            }
        }

        public void Update()
        {
            if (_context.Categories != null)
            {
                Categories category = _context.Categories.First(x => x.Id == 1);
                category.Name = "series";
                category.NameRu = "сериалы";
                _context.SaveChanges();
                Console.WriteLine($"Updated data for category id {category.Id}");
            }
        }

        public void Remove()
        {
            List<Genres> genres = _context.Genres.OrderBy(b => b.Id).ToList();
            foreach (var item in genres)
            {
                _context.Remove(item);
            }
            _context.SaveChanges();
        }

        public async Task<List<Movies>> GetMoviesByCategory(string category)
        {
            List<Movies> movie = null;
            if (_context.Movies.Any())
            {
                IQueryable<int> dbCategory = _context.Categories.Where(x => x.Name == category).Select(x => x.Id);
                movie = _context.Movies.Where(x => x.Category == dbCategory.FirstOrDefault()).ToList();
            }

            return movie;
        }

        public Task<List<Movies>> GetMoviesByGenre(string category, string genre)
        {
            throw new NotImplementedException();
        }
    }
}
