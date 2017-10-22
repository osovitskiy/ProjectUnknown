using Microsoft.Extensions.DependencyInjection;

namespace ProjectUnknown.AspNetCore.OAuth.DependencyInjection
{
    internal class OAuthBuilder : IOAuthBuilder
    {
        public OAuthBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
