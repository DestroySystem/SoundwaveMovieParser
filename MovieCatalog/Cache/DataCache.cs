using System.Reflection;

using CommonModels;
using EFCoreData.Operations;
using HtmlParser.Common;

using Newtonsoft.Json;

namespace MovieCatalog.Cache
{
    public class DataCache
    {
        private readonly Utils _utils;
        private readonly ILogger<DataCache> _logger;
        private readonly EFDatabaseOperations _operations;

        public DataCache(Utils utils, ILogger<DataCache> logger, EFDatabaseOperations operations)
        {
            _utils = utils;
            _logger = logger;
            _operations = operations;
        }

        private async Task StoreDataToSqLiteDb(string data)
        {
            try
            {
                await _operations.InsertMoviesFromJson(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> StoreDataToFileCache(string data, string path, string category, bool writeToDb = false)
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

                if (writeToDb)
                {
                    await StoreDataToSqLiteDb(data);
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
