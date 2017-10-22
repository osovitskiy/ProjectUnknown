using System;

namespace ProjectUnknown.AspNetCore.Extensions.RequestTime
{
    public class RequestTimeFeature : IRequestTimeFeature
    {
        public DateTimeOffset Timestamp { get; set; }
        public TimeSpan TimezoneOffset { get; set; }
    }
}
