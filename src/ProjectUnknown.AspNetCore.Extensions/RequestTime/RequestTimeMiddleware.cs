using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Extensions.RequestTime
{
    public class RequestTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestTimeOptions _options;

        public RequestTimeMiddleware(RequestDelegate next, RequestTimeOptions options)
        {
            Ensure.IsNotNull(next, nameof(next));
            Ensure.IsNotNull(options, nameof(options));

            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext context)
        {
            var feature = new RequestTimeFeature
            {
                Timestamp = DateTimeOffset.UtcNow
            };

            var header = _options.TimezoneOffsetHeaderName;
            if (header != null && context.Request.Headers.TryGetValue(header, out var values) && 
                int.TryParse(values, NumberStyles.Integer, CultureInfo.InvariantCulture, out var offset))
            {
                feature.TimezoneOffset = TimeSpan.FromMinutes(offset);
            }
            
            context.Features[typeof(IRequestTimeFeature)] = feature;

            return _next(context);
        }
    }
}
