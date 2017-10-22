using System;

namespace ProjectUnknown.AspNetCore.Extensions.Uptime
{
    public interface IApplicationUptime
    {
        bool HasStarted { get; }
        bool HasStopped { get; }
        DateTimeOffset StartedAt { get; }
        DateTimeOffset StoppedAt { get; }
        TimeSpan Uptime { get; }

        void Start();
        void Stop();
    }
}
