using Microsoft.Extensions.DependencyInjection;

namespace ProjectUnknown.AspNetCore.OAuth.DependencyInjection
{
    public interface IOAuthBuilder
    {
        IServiceCollection Services { get; }
    }
}
