using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using NGettext;

namespace ProjectUnknown.Localization.NGettext
{
    public class NGettextStringLocalizer : IStringLocalizer, ICatalog
    {
        public NGettextStringLocalizer(ICatalogCollection collection)
        {
            Collection = collection;
        }

        public NGettextStringLocalizer(ICatalogCollection collection, CultureInfo culture)
        {
            Collection = collection;
            Culture = culture;
        }

        public ICatalogCollection Collection { get; }
        public CultureInfo Culture { get; }

        public Catalog Catalog
        {
            get
            {
                var culture = Culture ?? CultureInfo.CurrentUICulture;
                var catalog = Collection.GetCatalog(culture);

                if (catalog == null)
                {
                    throw new UnsupportedLocaleException($"Locale '{culture.Name}' is not supported.");
                }

                return catalog;
            }
        }

        #region IStringLocalizer

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var catalog = Catalog;

            foreach (var translation in catalog.Translations)
            {
                yield return new LocalizedString(translation.Key, translation.Value[0]);
            }

            if (includeParentCultures)
            {
                while (true)
                {
                    catalog = Collection.GetCatalog(catalog.CultureInfo.Parent);

                    if (catalog == null)
                    {
                        break;
                    }

                    foreach (var translation in catalog.Translations)
                    {
                        yield return new LocalizedString(translation.Key, translation.Value[0]);
                    }
                }
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            return new NGettextStringLocalizer(Collection, culture);
        }

        LocalizedString IStringLocalizer.this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                return new LocalizedString(name, Catalog.GetString(name));
            }
        }

        LocalizedString IStringLocalizer.this[string name, params object[] arguments]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                return new LocalizedString(name, Catalog.GetString(name, arguments));
            }
        }

        #endregion

        #region ICatalog

        string ICatalog.GetString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Catalog.GetString(text);
        }

        string ICatalog.GetString(string text, params object[] args)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Catalog.GetString(text, args);
        }

        string ICatalog.GetPluralString(string text, string pluralText, long n)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrEmpty(pluralText))
            {
                throw new ArgumentNullException(nameof(pluralText));
            }

            return Catalog.GetPluralString(text, pluralText, n);
        }

        string ICatalog.GetPluralString(string text, string pluralText, long n, params object[] args)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrEmpty(pluralText))
            {
                throw new ArgumentNullException(nameof(pluralText));
            }

            return Catalog.GetPluralString(text, pluralText, n, args);
        }

        string ICatalog.GetParticularString(string context, string text)
        {
            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Catalog.GetString(context, text);
        }

        string ICatalog.GetParticularString(string context, string text, params object[] args)
        {
            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Catalog.GetString(context, text, args);
        }

        string ICatalog.GetParticularPluralString(string context, string text, string pluralText, long n)
        {
            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrEmpty(pluralText))
            {
                throw new ArgumentNullException(nameof(pluralText));
            }

            return Catalog.GetParticularPluralString(context, text, pluralText, n);
        }

        string ICatalog.GetParticularPluralString(string context, string text, string pluralText, long n, params object[] args)
        {
            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrEmpty(pluralText))
            {
                throw new ArgumentNullException(nameof(pluralText));
            }

            return Catalog.GetParticularPluralString(context, text, pluralText, n, args);
        }

        #endregion
    }
}
