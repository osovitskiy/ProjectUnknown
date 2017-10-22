using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.JsonWriter.DependencyInjection
{
    public static class JsonObjectWriterBuilderExtensions
    {
        public static IJsonObjectWriterBuilder WithAspNetJsonWriterFactory(this IJsonObjectWriterBuilder builder)
        {
            Ensure.IsNotNull(builder, nameof(builder));

            builder.Services.Replace(ServiceDescriptor.Singleton<IJsonWriterFactory, AspNetJsonWriterFactory>());

            return builder;
        }
    }
}
