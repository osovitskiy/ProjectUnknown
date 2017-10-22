using Microsoft.Extensions.DependencyInjection;

namespace ProjectUnknown.AspNetCore.JsonWriter.DependencyInjection
{
    public interface IJsonObjectWriterBuilder
    {
        IServiceCollection Services { get; }
    }
}
