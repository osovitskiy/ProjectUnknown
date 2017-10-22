using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWhen(this IApplicationBuilder app, Func<HttpContext, bool> predicate, 
            Action<IApplicationBuilder> thenConfiguration, Action<IApplicationBuilder> elseConfiguration)
        {
            Ensure.IsNotNull(app, nameof(app));
            Ensure.IsNotNull(predicate, nameof(predicate));
            Ensure.IsNotNull(thenConfiguration, nameof(thenConfiguration));
            Ensure.IsNotNull(elseConfiguration, nameof(elseConfiguration));

            var thenBuilder = app.New();
            var elseBuilder = app.New();

            thenConfiguration(thenBuilder);
            elseConfiguration(elseBuilder);

            return app.Use(main =>
            {
                thenBuilder.Run(main);
                elseBuilder.Run(main);

                var thenBranch = thenBuilder.Build();
                var elseBranch = elseBuilder.Build();

                return context => predicate(context) ? thenBranch(context) : elseBranch(context);
            });
        }
    }
}
