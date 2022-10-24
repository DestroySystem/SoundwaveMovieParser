using System.Text;

using EFCoreData.Context;

using HtmlParser.Common;
using HtmlParser.Dictionary;

using Microsoft.Extensions.Logging;

namespace EFCoreData
{
    public class Program
    {
        private static readonly ILogger<Utils> _logger;
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CategoriesDictionary dictionary = new CategoriesDictionary();
            MovieDbContext context = new();
            ILogger<EFDatabaseOperations> logger = null;
            Utils utils = new Utils(_logger);
            EFDatabaseOperations operations = new EFDatabaseOperations(utils, context, logger);
            List<string> categories = dictionary.GetCategories();

            string json =
                File.ReadAllText(@"C:\SoundwaveMovieParser\SoundwaveMovieParser\MovieCatalog\Data\animationListData.json");

            // operations.AddCategory(categories);
            // operations.AddGenres();
            // operations.TranslateGenres();
            //operations.AddCategoryToGenres();
            // operations.Update();
            operations.ReadCategories();
            operations.ReadGenres();
            // await operations.InsertMoviesFromJson(json);
            operations.ReadMovies();
            //operations.ReadCategoriesToGenres();
            //operations.Remove();

            Console.ReadLine();
        }
    }
}
