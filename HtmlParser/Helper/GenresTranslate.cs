using HtmlParser.Dictionary;

namespace HtmlParser.Helper
{
    public class GenresTranslate
    {
        public List<string> TranslateGenres(Dictionary<string, string> data)
        {
            CategoriesDictionary dictionary = new CategoriesDictionary();
            Dictionary<string, string> ruGenres = dictionary.RussianNameToEnglish();
            List<string> newGenres = new List<string>();
            try
            {
                if (data.ContainsKey("Genres"))
                {
                    List<string> genres = data["Genres"].Split(",").ToList();
                    foreach (string genre in genres)
                    {
                        if (!newGenres.Contains(genre))
                        {
                            newGenres.Add(ruGenres[genre]);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return newGenres;
        }
    }
}
