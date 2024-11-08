using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectUnknown.Localization.NGettext.DependencyInjection;

namespace NGettextLocalizationSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Environment.ContentRootFileProvider);
            
            services.AddNGettextLocalization()
                .UseFileProviderCatalogCollection(options => options.LocalizationDirectory = "Translations");
            
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseRequestLocalization(options =>
            {
                var supportedCultures = new[] {new CultureInfo("en-US"), new CultureInfo("ru-RU")};
                var queryLangMap = new Dictionary<string, string> {{"en", "en-US"}, {"ru", "ru-RU"}};

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
                options.RequestCultureProviders = new[]
                {
                    new CustomRequestCultureProvider(context =>
                    {
                        string culture = null;
                        if (context.Request.Query.TryGetValue("lang", out var values))
                        {
                            queryLangMap.TryGetValue(values, out culture);
                        }

                        if (culture == null)
                        {
                            culture = "en-US";
                        }
                        
                        return Task.FromResult(new ProviderCultureResult(culture));
                    })
                };

            });

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
