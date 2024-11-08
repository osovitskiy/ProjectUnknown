using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseMultipartRelatedExtension(this IApplicationBuilder app)
        {
            UseMultipartRelatedExtension(app, new MultipartOptions());
        }

        public static void UseMultipartRelatedExtension(this IApplicationBuilder app, MultipartOptions options)
        {
            Ensure.IsNotNull(app, nameof(app));
            Ensure.IsNotNull(options, nameof(options));

            app.UseMiddleware<MultipartRelatedMiddleware>(Options.Create(options));
        }
    }
}
