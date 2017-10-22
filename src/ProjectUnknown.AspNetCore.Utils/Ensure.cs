using System;
using System.Runtime.CompilerServices;

namespace ProjectUnknown.AspNetCore.Utils
{
    public static class Ensure
    {
        private const string ValueCannotBeNull = "Value cannot be null.";
        private const string ValueCannotBeEmpty = "Value cannot be empty.";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, ValueCannotBeNull);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, ValueCannotBeNull);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException(ValueCannotBeEmpty, paramName);
            }
        }
    }
}
