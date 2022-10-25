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
            CategoriesDictionary dictionary = new();
            List<Categories> categories = _context.Categories.OrderBy(x => x.Id).ToList();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Category(string category)
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
                    await _dataCache.StoreDataToFileCache(data, RuntimePath, category);
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
                            }
                            else
                            {
                                data = await _parser.GetMovieAsync(category);
                                await _dataCache.StoreDataToFileCache(data, RuntimePath, category);
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
