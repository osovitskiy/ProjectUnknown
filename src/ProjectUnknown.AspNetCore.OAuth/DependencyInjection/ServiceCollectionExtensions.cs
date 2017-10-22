using Microsoft.Extensions.DependencyInjection;
using ProjectUnknown.AspNetCore.JsonWriter.DependencyInjection;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.OAuth.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IOAuthBuilder AddOAuth(this IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));

            services.AddJsonObjectWriter()
                .WithAspNetJsonWriterFactory();

            return new OAuthBuilder(services);
        }
    }
}
