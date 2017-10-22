using Microsoft.Extensions.DependencyInjection;

namespace ProjectUnknown.AspNetCore.JsonWriter.DependencyInjection
{
    internal class JsonObjectWriterBuilder : IJsonObjectWriterBuilder
    {
        public JsonObjectWriterBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
