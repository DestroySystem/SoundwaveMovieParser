using CommonModels;
using EFCoreData.Models;

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
        /// <param name="data"></param>
        /// <returns></returns>
        public Task CheckRecordNotExist(string data);

        /// <summary>
        /// Insert to Movies table data from json file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task InsertMoviesFromJson(string data);

        /// <summary>
        /// Insert to Movie table movie that don`t exist in table records from MovieModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task InsertMovieFromModel(MovieModel model);

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

        /// <summary>
        /// Get from Db, List of Movies by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Task<List<Movies>> GetMoviesByCategory(string category);

        /// <summary>
        /// Get from Db, List of Movies by genre, using as filter also category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        public Task<List<Movies>> GetMoviesByGenre(string category, string genre);
    }
}
