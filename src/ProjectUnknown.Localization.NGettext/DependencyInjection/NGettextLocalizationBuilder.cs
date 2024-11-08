using Microsoft.Extensions.DependencyInjection;

namespace ProjectUnknown.Localization.NGettext.DependencyInjection
{
    public class NGettextLocalizationBuilder : INGettextLocalizationBuilder
    {
        public NGettextLocalizationBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
