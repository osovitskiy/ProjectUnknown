using System;
using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.Extensions.RequestTime
{
    public static class HttpContextExtensions
    {
        public static DateTimeOffset GetTimestamp(this HttpContext context)
        {
            var feature = context.Features.Get<IRequestTimeFeature>()  ?? throw new InvalidOperationException();
            return feature.Timestamp;
        }

        public static TimeSpan GetTimezoneOffset(this HttpContext context)
        {
            var feature = context.Features.Get<IRequestTimeFeature>() ?? throw new InvalidOperationException();
            return feature.TimezoneOffset;
        }
    }
}
