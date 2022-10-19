using System.Net;
using System.Reflection;

using CommonModels;

using HtmlAgilityPack;

using HtmlParser.Common;
using HtmlParser.Dictionary;
using HtmlParser.Helper;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
namespace HtmlParser.Parser
{
    public class HtmlParsing
    {
        #region localVariables
        private readonly string? ReleaseDate = "Дата выхода";
        private readonly string? Country = "Страна";
        private readonly string? Genre = "Жанр";
        private readonly string? Quality = "В качестве";
        private readonly string? Age = "Возраст";
        private readonly string? Duration = "Время";
        private readonly string? FromTheSeries = "Из серии";
        private readonly Utils _utils;
        private readonly HdrezkaTagHelpers _helper;
        private readonly GenresTranslate _translate;
        private readonly HttpClient _httpClient;
        private readonly ILogger<HtmlParsing> _logger;
        #endregion
        public HtmlParsing(Utils utils, HdrezkaTagHelpers helper, HttpClient httpClient, ILogger<HtmlParsing> logger, GenresTranslate translate)
        {
            _utils = utils;
            _helper = helper;
            _httpClient = httpClient;
            _logger = logger;
            _translate = translate;
        }


        /// <summary>
        /// Get json string with all movies from page
        /// </summary>
        /// <param name="category"></param>
        /// <param name="page"></param>
        /// <returns>json string</returns>
        public async Task<string> GetMovieAsync(string category = "films", string page = "1")
        {
            string json = string.Empty;
            CategoriesDictionary categories = new();
            List<string> genres = categories.GetGenresToCategory(category);
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.Indented
            };

            List<string> movieLinks = await GetMovieLinksAsync($@"http://hdrezka.tv/{category}/page/{page}");
            List<string> tempDelete = new();
            MovieDetailsModel movieDetails = new();
            try
            {
                foreach (string genre in genres)
                {
                    foreach (string link in movieLinks)
                    {
                        if (link.Contains(category) && link.Contains(genre))
                        {
                            MovieModel movie = await GetMovieDetailsAsync(link, category);
                            movieDetails.Models.Add(movie);
                            tempDelete.Add(link); //store for delete links that was added to model
                        }
                        else if (!link.Contains(category) && !link.Contains(genre))
                        {
                            tempDelete.Add(link); //store for delete links that dont contain category and any of genres
                        }
                    }
                    foreach (var item in tempDelete)
                    {
                        movieLinks.Remove(item);
                    }

                    if (tempDelete.Count != 0) //clear temp list only if it contain elements
                        tempDelete.Clear();
                }

                json = JsonConvert.SerializeObject(movieDetails, settings);
            }
            catch (Exception err)
            {
                _logger.LogError(message: $"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", args: err);
            }

            return json;
        }

        public async Task<Dictionary<string, string>> GetMovieCoverImageAsync(string url, string fileName)
        {
            string folder = Path.Combine(Environment.CurrentDirectory + "\\wwwroot", "images");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            Dictionary<string, string> image = new();

            try
            {
                HtmlDocument document = await _utils.GetHtmlDocument(url);
                HtmlNodeCollection imageNodes = document.DocumentNode.SelectNodes(_helper.ImageSourceTagHelper);
                foreach (HtmlNode item in imageNodes)
                {
                    string href = item.Attributes["href"].Value;
                    Uri baseUri = new(href);

                    using HttpClient client = new();
                    HttpResponseMessage responseMessage = await client.GetAsync(baseUri);
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        string uriWithoutQuery = baseUri.GetLeftPart(UriPartial.Path);
                        string fileExtension = Path.GetExtension(uriWithoutQuery);

                        string path = Path.Combine(folder, $@"{fileName}.jpeg");
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        byte[] imageBytes = await client.GetByteArrayAsync(baseUri);
                        await File.WriteAllBytesAsync(path, imageBytes);
                        image.Add("ContentType", responseMessage.Content.Headers.ContentType?.ToString() ?? string.Empty);
                        image.Add("Path", $"{fileName}.jpeg");
                    }
                }
            }
            catch (Exception err)
            {
                _logger.LogError(message: $"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", args: err);
            }

            return image;
        }

        /// <summary>
        /// Get movie image from page
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Dictionary with ContentType and Url to image</returns>
        public async Task<Dictionary<string, string>> GetMovieCoverImageAsync(string url)
        {
            Dictionary<string, string> image = new();
            try
            {
                HtmlDocument document = await _utils.GetHtmlDocument(url);
                string imageHref = document.DocumentNode.SelectSingleNode(_helper.ImageSourceTagHelper).Attributes["href"].Value;
                Uri imageUri = new(imageHref);
                HttpResponseMessage responseMessage = await _httpClient.GetAsync(imageUri);
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    image.Add("ContentType", responseMessage.Content.Headers.ContentType?.ToString() ?? string.Empty);
                    image.Add("Path", imageUri.ToString());
                }

            }
            catch (Exception err)
            {

                _logger.LogError(message: $"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", args: err);
            }

            return image;
        }

        /// <summary>
        /// Get details for each movie from page
        /// </summary>
        /// <param name="url"></param>
        /// <returns>MovieModel</returns>
        public async Task<MovieModel> GetMovieDetailsAsync(string url, string category)
        {
            HtmlDocument document = await _utils.GetHtmlDocument(url);
            Dictionary<string, string> data = await GetTableContentAsync(url);
            MovieModel movie = new();
            bool loadImageAsResource = false;
            string fileName = $"{Guid.NewGuid()}";
            try
            {
                Dictionary<string, string> image = loadImageAsResource ? await GetMovieCoverImageAsync(url, fileName) : await GetMovieCoverImageAsync(url);

                Image img = new()
                {
                    ContentType = image.ContainsKey("ContentType") ? image["ContentType"] : string.Empty,
                    Path = image.ContainsKey("Path") ? image["Path"] : string.Empty
                };

                List<string> genres = _translate.TranslateGenres(data);
                HtmlNodeCollection detailsNode = document.DocumentNode.SelectNodes(_helper.OriginalNameTagHelper);
                foreach (HtmlNode? item in detailsNode)
                {
                    movie.Id = Guid.NewGuid();
                    movie.NameRu = document.DocumentNode.SelectSingleNode(_helper.NameRuTagHelper).InnerText;
                    movie.CoverImage = img;
                    movie.OriginalName = item.InnerText;
                    movie.Link = url;
                    movie.ReleaseDate = data.ContainsKey("ReleaseDate") ? data["ReleaseDate"] : string.Empty;
                    movie.Quality = data.ContainsKey("Quality") ? data["Quality"] : string.Empty;
                    movie.Age = data.ContainsKey("Age") ? data["Age"] : string.Empty;
                    movie.Country = data.ContainsKey("Country") ? data["Country"] : string.Empty;
                    movie.Category = category;
                    movie.Genres = data.ContainsKey("Genres") ? genres : data["Genres"].Split(",").ToList();
                    movie.FromTheSeries = data.ContainsKey("FromSeries") ? data["FromSeries"] : string.Empty;
                    movie.Duration = data.ContainsKey("Duration") ? data["Duration"] : string.Empty;
                }
            }
            catch (Exception err)
            {
                _logger.LogError(message: $"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", args: err);
            }

            return movie;
        }

        /// <summary>
        /// Get all links from page
        /// </summary>
        /// <param name="url"></param>
        /// <returns>List<string></returns>
        public async Task<List<string>> GetMovieLinksAsync(string url)
        {
            List<string> movieLinks = new();
            try
            {
                HtmlDocument document = await _utils.GetHtmlDocument(url);
                HtmlNodeCollection linkNodes = document.DocumentNode.SelectNodes(_helper.LinksTagHelper);
                Uri baseUri = new(url);
                movieLinks.AddRange(from HtmlNode link in linkNodes
                                    where !movieLinks.Contains(link.Attributes["href"].Value)
                                    let href = link.Attributes["href"].Value
                                    select new Uri(baseUri, href).AbsoluteUri);
            }
            catch (Exception err)
            {
                _logger.LogError(message: $"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", args: err);
            }

            return movieLinks;
        }

        /// <summary>
        /// Get details about movie from table tag
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Dictionary with details</returns>
        public async Task<Dictionary<string, string>> GetTableContentAsync(string url)
        {
            Dictionary<string, string> dictionary = new();

            try
            {
                HtmlDocument document = await _utils.GetHtmlDocument(url);
                HtmlNode tableNode = document.DocumentNode.SelectSingleNode(_helper.TableTagHelper);
                var orderedCellTexts = tableNode.Descendants("tr")
                    .Select(row => row.SelectNodes("th|td").Take(2).ToArray())
                    .Where(cellArray => cellArray.Length == 2)
                    .Select(cellArray => new { KeyCell = cellArray[0].InnerText, ValueCell = cellArray[1].InnerText })
                    .OrderBy(x => x.KeyCell)
                    .ToList();

                foreach (var item in orderedCellTexts)
                {
                    if (item.KeyCell != null && item.KeyCell.Contains(ReleaseDate))
                    {
                        dictionary.Add("ReleaseDate", item.ValueCell.ToString());
                    }

                    if (item.KeyCell != null && item.KeyCell.Contains(Country))
                    {
                        dictionary.Add("Country", item.ValueCell.ToString());
                    }

                    if (item.KeyCell != null && item.KeyCell.Contains(Genre))
                    {
                        dictionary.Add("Genres", item.ValueCell.Replace(" ", "").ToString());
                    }

                    if (item.KeyCell != null && item.KeyCell.Contains(Quality))
                    {
                        dictionary.Add("Quality", item.ValueCell.ToString());
                    }

                    if (item.KeyCell != null && item.KeyCell.Contains(Age))
                    {
                        dictionary.Add("Age", item.ValueCell.ToString());
                    }

                    if (item.KeyCell != null && item.KeyCell.Contains(Duration))
                    {
                        dictionary.Add("Duration", item.ValueCell.ToString());
                    }
                    if (item.KeyCell != null && item.KeyCell.Contains(FromTheSeries))
                    {
                        dictionary.Add("FromSeries", item.ValueCell.ToString());
                    }
                }
            }
            catch (Exception err)
            {
                _logger.LogError(message: $"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", args: err);
            }

            return dictionary;
        }
    }
}
