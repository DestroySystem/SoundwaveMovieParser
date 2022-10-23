namespace HtmlParser.Dictionary
{
    public class CategoriesDictionary : ICategoriesDictionary
    {
        public Dictionary<string, List<string>> Categories()
        {
            Dictionary<string, List<string>> categories = new()
            {
                { "films", new List<string> { "crime", "comedy", "fiction", "family", "action", "adventures", "fantasy", "kids", "detective", "thriller", "foreign" } },
                { "series", new List<string> { "military", "fantasy", "comedy", "action", "adventures", "detective", "family", "crime", "foreign", "thriller", "fiction", "documentary", "melodrama", "horror", "drama", "realtv"} },
                { "cartoons", new List<string> { "military", "fantasy", "comedy", "action", "adventures", "detective", "family", "crime", "foreign", "thriller", "fiction" } },
                { "animation", new List<string> { "military", "comedy", "romance", "musical", "samurai", "parody", "kodomo", "shounenai", "drama", "fiction", "historical", "erotic", "sport", "school", "shoujoai", "ecchi", "detective", "fantasy", "horror", "action", "educational", "kids", "shoujo", "mahoushoujo", "thriller", "adventures", "mystery", "fighting", "everyday", "fairytale", "shounen", "mecha" } }
            };
            return categories;
        }

        public Dictionary<string, string> RussianNameToEnglish()
        {
            Dictionary<string, string> ruGenres = new()
            {
                {"Криминал","crime" },
                {"Комедии", "comedy" },
                {"Фантастика", "fiction" },
                {"Семейные", "family" },
                {"Боевики", "action" },
                {"Приключения", "adventures" },
                {"Фэнтези", "fantasy" },
                {"Детские","kids" },
                {"Детектив","detective" },
                {"Триллеры", "thriller" },
                {"Зарубежные", "foreign" },
                {"Военные", "military" },
                {"Документальные","documentary" },
                {"Мелодрамы", "melodrama" },
                {"Ужасы","horror" },
                {"Драмы","drama" },
                {"Реальное ТВ", "realtv" },
                {"Романтические", "romance" },
                {"Музыкальные", "musical" },
                {"Самурайский боевик", "samurai" },
                {"Пародия", "parody" },
                {"Кодомо", "kodomo" },
                {"Сёнэн-ай","shounenai" },
                {"Исторические","history" },
                {"Эротика", "erotic" },
                {"Спортивные", "sport" },
                {"Школа", "school" },
                {"Сёдзё-ай","shoujoai" },
                {"Этти", "ecchi" },
                {"Образовательные", "educational" },
                {"Сёдзё","shoujo" },
                {"Махо-сёдзё","mahoushoujo" },
                {"Мистические","mystery" },
                {"Меха","mecha" },
                {"Сёнэн","shounen" },
                {"Сказки","fairytale" },
                {"Повседневность","everyday" },
                {"Боевые искуства","fighting" },
                {"Исторические", "historical" }
            };
            return ruGenres;
        }


        public Dictionary<string, string> EnglishNameToRussian()
        {
            Dictionary<string, string> enGenres = new()
            {
                {"crime" , "Криминал" },
                {"comedy" , "Комедии" },
                {"fiction" , "Фантастика" },
                {"family" , "Семейные" },
                {"action" , "Боевики" },
                {"adventures" , "Приключения" },
                {"fantasy" , "Фэнтези" },
                {"kids" , "Детские" },
                {"detective" , "Детектив" },
                {"thriller" , "Триллеры" },
                {"foreign" , "Зарубежные" },
                {"military" , "Военные" },
                {"documentary" , "Документальные" },
                {"melodrama" , "Мелодрамы" },
                {"horror" , "Ужасы" },
                {"drama" , "Драмы" },
                {"realtv" , "Реальное ТВ" },
                {"romance" , "Романтические" },
                {"musical" , "Музыкальные" },
                {"samurai" , "Самурайский боевик" },
                {"parody" , "Пародия" },
                {"kodomo" , "Кодомо" },
                {"shounenai" , "Сёнэн-ай" },
                {"history" , "Исторические" },
                {"erotic" , "Эротика" },
                {"sport" , "Спортивные" },
                {"school" , "Школа" },
                {"shoujoai" , "Сёдзё-ай" },
                {"ecchi" , "Этти" },
                {"educational" , "Образовательные" },
                {"shoujo" , "Сёдзё" },
                {"mahoushoujo" , "Махо-сёдзё" },
                {"mystery" , "Мистические" },
                {"mecha" , "Меха" },
                {"shounen" , "Сёнэн" },
                {"fairytale" , "Сказки" },
                {"everyday","Повседневность" },
                {"fighting","Боевые искуства" },
                {"historical", "Исторические" }
            };
            return enGenres;
        }

        public List<string> GetCategories()
        {
            Dictionary<string, List<string>> dictionary = Categories();
            List<string> categories = new();
            foreach (string category in dictionary.Keys)
            {
                categories.Add(category);
            }
            return categories;
        }

        public Dictionary<string, List<string>> GetAllGenresToCategories()
        {
            Dictionary<string, List<string>> dictionary = Categories();
            return dictionary;
        }

        public List<string> GetGenresToCategory(string category)
        {
            Dictionary<string, List<string>> dictionary = Categories();
            List<string> genres = dictionary[category];
            return genres;
        }
    }
}
