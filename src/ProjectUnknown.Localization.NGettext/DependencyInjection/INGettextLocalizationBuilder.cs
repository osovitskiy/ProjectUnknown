using Microsoft.Extensions.DependencyInjection;

namespace ProjectUnknown.Localization.NGettext.DependencyInjection
{
    public interface INGettextLocalizationBuilder
    {
        IServiceCollection Services { get; }
    }
}
