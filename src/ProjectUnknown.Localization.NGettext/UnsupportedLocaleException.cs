using System;

namespace ProjectUnknown.Localization.NGettext
{
    public class UnsupportedLocaleException : Exception
    {
        public UnsupportedLocaleException()
        {
        }

        public UnsupportedLocaleException(string message)
            : base(message)
        {
        }

        public UnsupportedLocaleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
