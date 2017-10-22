using Microsoft.AspNetCore.Builder;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Extensions.DropHttp
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDropHttp(this IApplicationBuilder app)
        {
            Ensure.IsNotNull(app, nameof(app));

            return app.UseMiddleware<DropHttpMiddleware>();
        }
    }
}
