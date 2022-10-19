namespace HtmlParser.Dictionary
{
    public interface ICategoriesDictionary
    {
        /// <summary>
        /// Dictionary populated with categories of movies, serials, cartoon or anime
        /// </summary>
        /// <returns>Dictionary<string, List<string>> with categories</returns>
        public Dictionary<string, List<string>> Categories();

        /// <summary>
        /// Get all categories from dictionary
        /// </summary>
        /// <returns>List<string></returns>
        public List<string> GetCategories();

        /// <summary>
        /// Get all genres of all categories in dictionary
        /// </summary>
        /// <returns>Dictionary<string, List<string>></returns>
        public Dictionary<string, List<string>> GetAllGenresToCategories();

        /// <summary>
        /// Get all genres for selected category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>List<string></returns>
        public List<string> GetGenresToCategory(string category);
    }
}
