using System.Reflection;

using CommonModels;

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
        private string RuntimePath { get; } = Path.Combine(Environment.CurrentDirectory, "Data");

        public MovieController(ILogger<MovieController> logger, HtmlParsing parser, DataCache dataCache)
        {
            _logger = logger;
            _parser = parser;
            _dataCache = dataCache;
        }

        [HttpGet]
        public IActionResult Movies()
        {
            CategoriesDictionary dictionary = new();
            List<string> categories = dictionary.GetCategories();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Category(string id)
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
                    data = await _parser.GetMovieAsync(category: id);
                    await _dataCache.StoreDataToFileCache(data, RuntimePath, id);
                }
                else
                {
                    foreach (string file in files)
                    {
                        if (file.Length != 0)
                        {
                            FileInfo fileInfo = new(file);
                            if (fileInfo.Exists && fileInfo.FullName.Contains(id) && fileInfo.Extension == ".json")
                            {
                                data = await System.IO.File.ReadAllTextAsync(file);
                            }
                            else
                            {
                                data = await _parser.GetMovieAsync(id);
                                await _dataCache.StoreDataToFileCache(data, RuntimePath, id);
                            }
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
