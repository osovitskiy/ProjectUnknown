using System.Globalization;
using NGettext;

namespace ProjectUnknown.Localization.NGettext
{
    public interface ICatalogCollection
    {
        Catalog GetCatalog(CultureInfo culture);
    }
}
