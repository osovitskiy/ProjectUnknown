using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.JsonWriter.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IJsonObjectWriterBuilder AddJsonObjectWriter(this IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));

            return AddJsonObjectWriterInternal(services, null);
        }

        public static IJsonObjectWriterBuilder AddJsonObjectWriter(this IServiceCollection services, Action<JsonObjectWriterOptions> configure)
        {
            Ensure.IsNotNull(services, nameof(services));
            Ensure.IsNotNull(configure, nameof(configure));

            return AddJsonObjectWriterInternal(services, configure);
        }

        private static IJsonObjectWriterBuilder AddJsonObjectWriterInternal(IServiceCollection services, Action<JsonObjectWriterOptions> configure)
        {
            services.TryAddSingleton<IJsonObjectWriter, JsonObjectWriter>();
            services.TryAddScoped<IJsonWriterFactory, DefaultJsonWriterFactory>();

            if (configure != null)
            {
                services.Configure(configure);
            }

            return new JsonObjectWriterBuilder(services);
        }
    }
}
