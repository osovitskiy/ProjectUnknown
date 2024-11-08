using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using ProjectUnknown.Common;

namespace ProjectUnknown.Localization.NGettext.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static INGettextLocalizationBuilder AddNGettextLocalization(this IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));

            var builder = new NGettextLocalizationBuilder(services);

            builder.Services.TryAddSingleton<IStringLocalizerFactory, NGettextStringLocalizerFactory>();
            builder.Services.TryAddTransient<IStringLocalizer, NGettextStringLocalizer>();
            builder.Services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            return builder;
        }
    }
}
