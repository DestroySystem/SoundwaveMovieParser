using System.Reflection;

using CommonModels;

using EFCoreData.Context;
using EFCoreData.Models;

using HtmlParser.Dictionary;
using HtmlParser.Parser;

using Microsoft.AspNetCore.Mvc;

using MovieCatalog.Cache;

using Newtonsoft.Json;

namespace MovieCatalog.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly HtmlParsing _parser;
        private readonly DataCache _dataCache;
        private readonly MovieDbContext _context;
        private string RuntimePath { get; } = Path.Combine(Environment.CurrentDirectory, "Data");

        public MovieController(ILogger<MovieController> logger, HtmlParsing parser, DataCache dataCache, MovieDbContext context)
        {
            _logger = logger;
            _parser = parser;
            _dataCache = dataCache;
            _context = context;
        }

        [HttpGet]
        public IActionResult Movies()
        {
            Dictionary<string, List<string>> genresToCategory = new Dictionary<string, List<string>>();
            List<Categories> categoryList = _context.Categories.OrderBy(x => x.Id).ToList();
            foreach (var category in categoryList)
            {
                List<int> categoryToGenres =
                    _context.CategoryToGenres.Where(x => x.Category == category.Id).Select(x => x.Genre).ToList();
                List<string> genresList = new();
                foreach (var genres in categoryToGenres)
                {
                    foreach (var name in _context.Genres.Where(x => x.Id == genres))
                    {
                        genresList.Add(name.Name);
                    }
                }
                if (!genresToCategory.ContainsKey(category.Name))
                    genresToCategory.Add(category.Name, new List<string>(genresList));
            }


            return View(genresToCategory);
        }

        [HttpGet]
        public async Task<IActionResult> Category(string category, string genre)
        {
            string data = string.Empty;
            MovieDetailsModel? detailsModel = new();
            try
            {
                if (!Directory.Exists(RuntimePath))
                    Directory.CreateDirectory(RuntimePath);

                string[] files = Directory.GetFiles(RuntimePath);
                if (files.Length == 0)
                {
                    data = await _parser.GetMovieAsync(category: category);
                    await _dataCache.StoreDataToFileCache(data, RuntimePath, category, true);
                }
                else
                {
                    foreach (string file in files)
                    {
                        if (file.Length != 0)
                        {
                            FileInfo fileInfo = new(file);
                            if (fileInfo.Exists && fileInfo.FullName.Contains(category) && fileInfo.Extension == ".json")
                            {
                                data = await System.IO.File.ReadAllTextAsync(file);
                                await _dataCache.StoreDataToFileCache(data, RuntimePath, category, true);
                            }
                        }
                        else
                        {
                            data = await _parser.GetMovieAsync(category);
                            await _dataCache.StoreDataToFileCache(data, RuntimePath, category, true);
                        }
                    }
                }
                detailsModel = JsonConvert.DeserializeObject<MovieDetailsModel>(data);
            }
            catch (Exception err)
            {
                _logger.LogError($"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", err);
            }
            return View(detailsModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMovie(string id)
        {
            List<string> fromRequest = id.Split('&').ToList();
            MovieDetailsModel? detailsModel = new();
            try
            {
                if (!Directory.Exists(RuntimePath))
                {
                    _logger.LogError($"No data detected on path: {RuntimePath}");
                    //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                else
                {
                    string[] files = Directory.GetFiles(RuntimePath);
                    foreach (string file in files)
                    {
                        if (file.Length != 0)
                        {
                            FileInfo fileInfo = new(file);
                            if (fileInfo.Exists && fileInfo.FullName.Contains(fromRequest[1]) && fileInfo.Extension == ".json")
                            {
                                string? json = await System.IO.File.ReadAllTextAsync(file);
                                detailsModel = JsonConvert.DeserializeObject<MovieDetailsModel>(json);
                                IEnumerable<MovieModel> movieModels = from movie in detailsModel?.Models where movie.Id.ToString() == fromRequest[0] select movie;
                                return View(movieModels);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _logger.LogError($"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", err);
            }
            return null;
        }
    }
}
