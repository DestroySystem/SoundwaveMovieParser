using EFCoreData;
using EFCoreData.Context;

using MovieCatalog.Cache;

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
            services.AddScoped<DataCache>();
            services.AddScoped<HttpClient>();
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
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
