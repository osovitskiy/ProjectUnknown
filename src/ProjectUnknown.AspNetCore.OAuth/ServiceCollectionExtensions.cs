using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOAuthTokenHandler<T>(this IServiceCollection services)
            where T : class, IOAuthTokenHandler
        {
            Ensure.IsNotNull(services, nameof(services));

            services.TryAddScoped<IOAuthTokenHandler, T>();

            return services;
        }
    }
}
