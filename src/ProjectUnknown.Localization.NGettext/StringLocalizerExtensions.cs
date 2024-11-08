using System;
using Microsoft.Extensions.Localization;
using NGettext;
using ProjectUnknown.Common;

namespace ProjectUnknown.Localization.NGettext
{
    public static class StringLocalizerExtensions
    {
        public static LocalizedString GetPluralString(this IStringLocalizer localizer, string text, string plural, long n)
        {
            Ensure.IsNotNull(localizer, nameof(localizer));
        
            if (localizer is ICatalog catalog)
            {
                return new LocalizedString(text, catalog.GetPluralString(text, plural, n));
            }
        
            throw new InvalidOperationException();
        }

        public static LocalizedString GetPluralString(this IStringLocalizer localizer, string text, string plural, long n, params object[] args)
        {
            Ensure.IsNotNull(localizer, nameof(localizer));

            if (localizer is ICatalog catalog)
            {
                return new LocalizedString(text, catalog.GetPluralString(text, plural, n, args));
            }

            throw new InvalidOperationException();
        }

        public static LocalizedString GetParticularString(this IStringLocalizer localizer, string context, string text)
        {
            Ensure.IsNotNull(localizer, nameof(localizer));

            if (localizer is ICatalog catalog)
            {
                return new LocalizedString(text, catalog.GetParticularString(context, text));
            }

            throw new InvalidOperationException();
        }

        public static LocalizedString GetParticularString(this IStringLocalizer localizer, string context, string text, params object[] args)
        {
            Ensure.IsNotNull(localizer, nameof(localizer));

            if (localizer is ICatalog catalog)
            {
                return new LocalizedString(text, catalog.GetParticularString(context, text, args));
            }

            throw new InvalidOperationException();
        }

        public static LocalizedString GetParticularPluralString(this IStringLocalizer localizer, string context, string text, string plural, long n)
        {
            Ensure.IsNotNull(localizer, nameof(localizer));

            if (localizer is ICatalog catalog)
            {
                return new LocalizedString(text, catalog.GetParticularPluralString(context, text, plural, n));
            }

            throw new InvalidOperationException();
        }

        public static LocalizedString GetParticularPluralString(this IStringLocalizer localizer, string context, string text, string plural, long n, params object[] args)
        {
            Ensure.IsNotNull(localizer, nameof(localizer));

            if (localizer is ICatalog catalog)
            {
                return new LocalizedString(text, catalog.GetParticularPluralString(context, text, plural, n, args));
            }

            throw new InvalidOperationException();
        }
    }
}
