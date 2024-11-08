using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NGettext;

namespace ProjectUnknown.Localization.NGettext
{
    public class FileProviderCatalogCollection : ICatalogCollection
    {
        private readonly IFileProvider _provider;
        private readonly FileProviderCatalogCollectionOptions _options;

        private readonly ConcurrentDictionary<string, Lazy<Catalog>> _catalogs = new ConcurrentDictionary<string, Lazy<Catalog>>();

        public FileProviderCatalogCollection(IFileProvider provider, IOptions<FileProviderCatalogCollectionOptions> options)
        {
            _provider = provider;
            _options = options.Value;
        }

        public Catalog GetCatalog(CultureInfo culture)
        {
            return _catalogs.GetOrAdd(culture.Name, _ => new Lazy<Catalog>(() => LoadCatalog(culture))).Value;
        }

        private Catalog LoadCatalog(CultureInfo culture)
        {
            var file = _provider.GetFileInfo(Path.Combine(_options.LocalizationDirectory, $"{culture.Name}.mo"));

            if (file.Exists)
            {
                using (var stream = file.CreateReadStream())
                {
                    return new Catalog(stream, culture);
                }
            }

            return new Catalog();
        }
    }
}
