using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectUnknown.AspNetCore.Extensions.Uptime
{
    public class ApplicationUptimeStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                var uptime = app.ApplicationServices.GetRequiredService<IApplicationUptime>();

                lifetime.ApplicationStarted.Register(uptime.Start);
                lifetime.ApplicationStopped.Register(uptime.Stop);
            };
        }
    }
}

