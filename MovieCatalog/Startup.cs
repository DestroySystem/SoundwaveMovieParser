using EFCoreData.Context;
using EFCoreData.Operations;

using Microsoft.EntityFrameworkCore;

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
            services.AddScoped<HtmlParser.Dictionary.CategoriesDictionary>();
            services.AddScoped<HtmlParser.Helper.GenresTranslate>();
            services.AddScoped<HtmlParser.Helper.HdrezkaTagHelpers>();
            services.AddScoped<EFDatabaseOperations>();
            services.AddScoped<MovieDbContext>();
            services.AddScoped<DataRetrieval>();
            services.AddScoped<DataCache>();
            services.AddScoped<HttpClient>();
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MovieDbContext context)
        {
            app.UseForwardedHeaders();

            context.Database.Migrate();

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
