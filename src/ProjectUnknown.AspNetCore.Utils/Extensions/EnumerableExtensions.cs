using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectUnknown.AspNetCore.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<TResult> SelectList<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (source is ICollection<T> collection)
            {
                var result = new List<TResult>(collection.Count);

                foreach (var item in collection)
                {
                    result.Add(selector(item));
                }

                return result;
            }
            else
            {
                return source.Select(selector).ToList();
            }
        }
    }
}
