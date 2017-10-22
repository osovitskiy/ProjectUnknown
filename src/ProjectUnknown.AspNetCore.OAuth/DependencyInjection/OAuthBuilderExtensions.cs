using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectUnknown.AspNetCore.JsonWriter;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.OAuth.DependencyInjection
{
    public static class OAuthBuilderExtensions
    {
        public static IOAuthBuilder WithHandler<T>(this IOAuthBuilder builder)
            where T : class, IOAuthHandler
        {
            Ensure.IsNotNull(builder, nameof(builder));

            builder.Services.TryAddScoped<IOAuthHandler, T>();

            return builder;
        }

        public static IOAuthBuilder WithJsonWriterOptions(this IOAuthBuilder builder, Action<JsonObjectWriterOptions> configure)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNull(configure, nameof(configure));

            builder.Services.Configure(configure);

            return builder;
        }
    }
}
