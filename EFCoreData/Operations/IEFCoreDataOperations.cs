namespace EFCoreData.Operations
{
    // ReSharper disable once InconsistentNaming
    public interface IEFCoreDataOperations
    {
        /// <summary>
        /// Add to SqLite Db Movie categories
        /// </summary>
        /// <param name="category"></param>
        public void AddCategory(List<string> category);
        /// <summary>
        /// Add to SqLite Db Movie genres
        /// </summary>
        public void AddGenres();

        /// <summary>
        /// Create in SqLite relation between category and genres
        /// </summary>
        public void AddCategoryToGenres();

        /// <summary>
        /// Check if in table Movies exist records from json file, to do not create duplicates
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Task CheckRecordNotExist(string category);

        /// <summary>
        /// Insert to Movies table data from json file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task InsertMoviesFromJson(string data);

        /// <summary>
        /// Add russian name for genres
        /// </summary>
        public void TranslateGenres();

        /// <summary>
        /// Read data from Genres table 
        /// </summary>
        public void ReadGenres();

        /// <summary>
        /// Read data from Categories table
        /// </summary>
        public void ReadCategories();

        /// <summary>
        /// Read data from relational table Categories To Genres
        /// </summary>
        public void ReadCategoriesToGenres();

        /// <summary>
        /// Read data from Movies table
        /// </summary>
        public void ReadMovies();

        /// <summary>
        /// Update record
        /// </summary>
        public void Update();

        /// <summary>
        /// Remove record
        /// </summary>
        public void Remove();
    }
}
