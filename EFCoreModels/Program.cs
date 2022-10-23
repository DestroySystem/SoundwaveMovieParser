using System.Text;
using HtmlParser.Dictionary;
using HtmlParser.Parser;

namespace EFCoreModels;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        CategoriesDictionary dictionary = new CategoriesDictionary();
        EfDatabaseOperations operations = new EfDatabaseOperations();
        List<string> categories = dictionary.GetCategories();

        operations.AddCategory(categories);
        operations.AddGenres();
        operations.TranslateGenres();
        operations.AddCategoryToGenres();
        //operations.Update();
        operations.ReadCategories();
        operations.ReadGenres();
        //operations.Remove();

/*        List<string> countries = operations.GetAllCountrysNames();
        foreach(string country in countries)
        {
            Console.WriteLine(country);
        }*/

        Console.ReadLine();
    }
}