using System;
using Microsoft.Extensions.DependencyInjection;
using ProjectUnknown.Common;

namespace ProjectUnknown.Localization.NGettext.DependencyInjection
{
    public static class NGettextLocalizationBuilderExtensions
    {
        public static INGettextLocalizationBuilder UseFileProviderCatalogCollection(this INGettextLocalizationBuilder builder)
        {
            Ensure.IsNotNull(builder, nameof(builder));

            return UseFileProviderCatalogCollectionInternal(builder, null);
        }

        public static INGettextLocalizationBuilder UseFileProviderCatalogCollection(this INGettextLocalizationBuilder builder, Action<FileProviderCatalogCollectionOptions> configure)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNull(configure, nameof(configure));

            return UseFileProviderCatalogCollectionInternal(builder, configure);
        }

        private static INGettextLocalizationBuilder UseFileProviderCatalogCollectionInternal(INGettextLocalizationBuilder builder, Action<FileProviderCatalogCollectionOptions> configure)
        {
            builder.Services.AddSingleton<ICatalogCollection, FileProviderCatalogCollection>();

            if (configure != null)
            {
                builder.Services.Configure(configure);
            }

            return builder;
        }
    }
}
