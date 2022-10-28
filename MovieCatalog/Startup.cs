using EFCoreData.Context;
using EFCoreData.Operations;
using HtmlParser.Dictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using MovieCatalog.Cache;
using MovieCatalog.DbData;

namespace MovieCatalog
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddEndpointsApiExplorer();
            services.AddScoped<HtmlParser.Parser.HtmlParsing>();
            services.AddScoped<HtmlParser.Common.Utils>();
            services.AddScoped<CategoriesDictionary>();
            services.AddScoped<HtmlParser.Helper.GenresTranslate>();
            services.AddScoped<HtmlParser.Helper.HdrezkaTagHelpers>();
            services.AddScoped<EFDatabaseOperations>();
            services.AddScoped<MovieDbContext>();
            services.AddScoped<DataRetrieval>();
            services.AddScoped<DataCache>();
            services.AddScoped<HttpClient>();
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MovieDbContext context, EFDatabaseOperations operations)
        {
            app.UseForwardedHeaders();
            context.Database.EnsureCreated();

            int count = context.Database.ExecuteSqlRaw("SELECT COUNT(*) AS TableCount FROM sqlite_master WHERE type = 'table' AND name = 'Movies'");
            if (!context.Categories.Any() && !context.GenresToMovies.Any() && !context.CategoryToGenres.Any())
            {
                if (count == 0)
                {
                    context.Database.Migrate();
                }

                CategoriesDictionary dictionary = new();
                operations.AddCategory(dictionary.GetCategories());
                operations.AddGenres();
                operations.TranslateGenres();
                operations.AddCategoryToGenres();
            }
            

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpLogging();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
                endpoint.MapControllers();
            });
        }
    }
}
