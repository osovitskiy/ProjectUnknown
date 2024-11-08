using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public class MultipartRelatedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MultipartOptions _options;

        public MultipartRelatedMiddleware(RequestDelegate next, IOptions<MultipartOptions> options)
        {
            Ensure.IsNotNull(next, nameof(next));
            Ensure.IsNotNull(options, nameof(options));

            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.HasMultipartRelatedContentType())
            {
                context.Features[typeof(IMultipartRelatedFeature)] = new MultipartRelatedFeature(context.Request, _options);
            }

            return _next(context);
        }
    }
}
