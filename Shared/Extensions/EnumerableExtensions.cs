using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool AnyOutLastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource result) where TSource : class
        {
            if (source.Any(predicate))
            {
                result = source.LastOrDefault(predicate);

                return true;
            }

            result = default;

            return false;
        }
    }
}