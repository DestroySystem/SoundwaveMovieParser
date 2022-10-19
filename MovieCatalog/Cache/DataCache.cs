using System.Reflection;

using CommonModels;
using HtmlParser.Common;
using Newtonsoft.Json;

namespace MovieCatalog.Cache
{
    public class DataCache
    {
        private readonly Utils _utils;
        private readonly ILogger<DataCache> _logger;

        public DataCache(Utils utils, ILogger<DataCache> logger)
        {
            _utils = utils;
            _logger = logger;
        }

        public Task StoreDataToInMemoryCache(string data, string key)
        {
            Dictionary<string, string> cache = new();
            try
            {
                if (!cache.ContainsKey(key))
                {
                    cache.Add(key, data);
                }
                else
                {
                    cache[key] = data;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error: {err}");
            }

            return Task.CompletedTask;
        }

        public async Task<string> StoreDataToFileCache(string data, string path, string category)
        {
            string fileName = string.Empty;
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                fileName = $@"{path}\{category}ListData.json";
                if (!File.Exists(fileName))
                {
                    await File.WriteAllTextAsync(fileName, data);
                }
            }
            catch (Exception err)
            {
                _logger.LogError($"Error: {err} from {MethodBase.GetCurrentMethod()?.Name}", err);
            }
/*            finally
            {
                await Task.Run(() => { InitImageResize(data); return Task.CompletedTask; });
            }*/

            return fileName;
        }

        public void InitImageResize(string json)
        {
            string folder = Path.Combine(Environment.CurrentDirectory + "\\wwwroot", "images");
            try
            {
                if (!Directory.Exists(folder))
                    throw new FileNotFoundException();

                MovieDetailsModel? data = JsonConvert.DeserializeObject<MovieDetailsModel>(json);
                foreach (MovieModel item in data.Models)
                {
                    _utils.ResizeImage(folder, $"{item.CoverImage?.Path}");
                }
            }
            catch (Exception err)
            {
                _logger.LogError($"Error {err} from {MethodBase.GetCurrentMethod()?.Name}");
            }
        }
    }
}
