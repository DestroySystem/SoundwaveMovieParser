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

      // operations.AddCategory(categories);
      // operations.AddGenres();
      operations.TranslateGenres();
        //operations.Update();
       //operations.ReadCategories();
      operations.ReadGenres();
        //operations.Remove();
        Console.ReadLine();
    }
}