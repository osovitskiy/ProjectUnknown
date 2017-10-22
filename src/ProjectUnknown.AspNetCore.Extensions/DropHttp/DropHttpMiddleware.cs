using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Extensions.DropHttp
{
    public class DropHttpMiddleware
    {
        private readonly RequestDelegate _next;

        public DropHttpMiddleware(RequestDelegate next)
        {
            Ensure.IsNotNull(next, nameof(next));

            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
            {
                return _next(context);
            }

            context.Abort();
            return Task.CompletedTask;
        }
    }
}
