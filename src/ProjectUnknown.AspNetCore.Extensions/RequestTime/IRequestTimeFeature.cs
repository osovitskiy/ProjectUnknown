using System;

namespace ProjectUnknown.AspNetCore.Extensions.RequestTime
{
    public interface IRequestTimeFeature
    {
        DateTimeOffset Timestamp { get; set; }
        TimeSpan TimezoneOffset { get; set; }
    }
}
