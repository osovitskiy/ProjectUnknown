using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.OAuth.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOAuth(this IApplicationBuilder app, OAuthOptions options)
        {
            Ensure.IsNotNull(app, nameof(app));
            Ensure.IsNotNull(options, nameof(options));

            app.UseMiddleware<OAuthMiddleware>(Options.Create(options));

            return app;
        }
    }
}
