using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOAuthToken(this IApplicationBuilder app, PathString endpoint, OAuthTokenOptions options)
        {
            Ensure.IsNotNull(app, nameof(app));
            Ensure.IsNotNull(options, nameof(options));

#if NET6_0
            app.UseEndpoints(endpoints => endpoints.Map(
                endpoint,
                endpoints.CreateApplicationBuilder().UseMiddleware<OAuthTokenMiddleware>(options).Build())
            );
#else
            app.UseWhen(
                context => context.Request.Path == endpoint,
                builder => builder.UseMiddleware<OAuthTokenMiddleware>(options).Build()
            );
#endif

            return app;
        }
    }
}
