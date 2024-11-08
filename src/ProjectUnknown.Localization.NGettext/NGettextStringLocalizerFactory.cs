using System;
using Microsoft.Extensions.Localization;

namespace ProjectUnknown.Localization.NGettext
{
    public class NGettextStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ICatalogCollection _collection;

        public NGettextStringLocalizerFactory(ICatalogCollection collection)
        {
            _collection = collection;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new NGettextStringLocalizer(_collection);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new NGettextStringLocalizer(_collection);
        }
    }
}
