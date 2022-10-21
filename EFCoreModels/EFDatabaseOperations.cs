using System.Collections.Immutable;

using EFCoreModels.Context;
using EFCoreModels.Models;

using HtmlParser.Dictionary;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFCoreModels
{
    public class EfDatabaseOperations
    {

        public void AddCategory(List<string> category)
        {
            MovieDbContext context = new();
            foreach (string item in category)
            {
                context.Add(new Categories()
                {
                    Name = item
                });
            }
            context.SaveChanges();
        }

        public void AddGenres()
        {
            MovieDbContext context = new();
            CategoriesDictionary dictionary = new CategoriesDictionary();
            Dictionary<string, List<string>> categories = dictionary.Categories();
            List<string> distGenres = new();
            foreach (var itemCategoriesKey in categories.Keys)
            {
                var genres = categories[itemCategoriesKey];
                foreach (var item in genres)
                {
                    if (!distGenres.Contains(item))
                        distGenres.Add(item);
                }
            }

            foreach (string item in distGenres)
            {
                context.Add(new Genres()
                {
                    Name = item
                });
            }

            context.SaveChanges();
        }

        public void AddCategoryToGenres()
        {
            MovieDbContext context = new();
            CategoriesDictionary dictionary = new CategoriesDictionary();
            Dictionary<int, int> dict = new Dictionary<int, int>();
            Dictionary<string, List<string>> catDictionary = dictionary.Categories();

            List<Categories> categories = context.Categories.OrderBy(b => b.Id).ToList();
            List<Genres> genres = context.Genres.OrderBy(b => b.Id).ToList();

            foreach (var category in categories)
            {
                foreach (var genre in genres)
                {
                    if (catDictionary.ContainsKey(category.Name) && catDictionary[category.Name].Contains(genre.Name))
                    {
                        context.Add(new CategoryToGenres()
                        {
                            Category = category.Id,
                            Genre = genre.Id
                        });
                    }
                }
            }

            context.SaveChanges();
        }


        public void TranslateGenres()
        {
            MovieDbContext context = new();
            CategoriesDictionary dictionary = new CategoriesDictionary();
            Dictionary<string, string> translate = dictionary.RussianNameToEnglish();
            List<Genres> genres = context.Genres.OrderBy(b => b.Id).ToList();
            foreach (var genre in genres)
            {
                foreach (var item in translate.Keys)
                {
                    foreach (var i in translate.Values)
                    {
                        if (i.Contains(genre.Name))
                        {
                            genre.NameRu = item;
                        }
                    }
                }
            }

            context.SaveChanges();
        }

        public void ReadGenres()
        {
            MovieDbContext context = new();
            if (context.Genres != null)
            {
                List<Genres> genres = context.Genres.OrderBy(b => b.Id).ToList();
                foreach (var item in genres)
                {
                    Console.WriteLine($"{item.Id} {item.Name} {item.NameRu}");
                }
            }
            context.Dispose();
        }

        public void ReadCategories()
        {
            MovieDbContext context = new();
            if (context.Categories != null)
            {
                List<Categories> genres = context.Categories.OrderBy(b => b.Id).ToList();
                foreach (var item in genres)
                {
                    Console.WriteLine($"{item.Id} {item.Name} {item.NameRu}");
                }
            }
            context.Dispose();
        }

        public void Update()
        {
            MovieDbContext context = new();
            if (context.Categories != null)
            {
                Categories category = context.Categories.First(x => x.Id == 1);
                category.Name = "series";
                category.NameRu = "сериалы";
                context.SaveChanges();
                Console.WriteLine($"Updated data for category id {category.Id}");
            }
            context.Dispose();
        }

        public void Remove()
        {
            MovieDbContext context = new();
            List<Genres> genres = context.Genres.OrderBy(b => b.Id).ToList();
            foreach (var item in genres)
            {
                context.Remove(item);
            }
            context.SaveChanges();
        }
    }
}
