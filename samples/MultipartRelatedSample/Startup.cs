using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated;

namespace MultipartRelatedSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.AddMultipartRelatedInputFormatter();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMultipartRelatedExtension();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
