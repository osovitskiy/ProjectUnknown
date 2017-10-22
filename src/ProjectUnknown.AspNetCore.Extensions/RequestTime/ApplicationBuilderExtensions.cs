using Microsoft.AspNetCore.Builder;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Extensions.RequestTime
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestTime(this IApplicationBuilder app)
        {
            return UseRequestTime(app, new RequestTimeOptions());
        }

        public static IApplicationBuilder UseRequestTime(this IApplicationBuilder app, RequestTimeOptions options)
        {
            Ensure.IsNotNull(app, nameof(app));
            Ensure.IsNotNull(options, nameof(options));

            return app.UseMiddleware<RequestTimeMiddleware>(options);
        }
    }
}
