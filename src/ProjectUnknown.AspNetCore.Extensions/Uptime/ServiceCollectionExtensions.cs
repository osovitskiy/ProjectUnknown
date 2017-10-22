using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Extensions.Uptime
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationUptime(this IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));

            services.TryAddSingleton<IApplicationUptime, ApplicationUptime>();
            services.TryAddSingleton<IStartupFilter, ApplicationUptimeStartupFilter>();

            return services;
        }
    }
}
