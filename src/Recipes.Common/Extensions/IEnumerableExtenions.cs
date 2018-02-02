using System;
using System.Collections.Generic;

namespace Recipes.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            Guard.ThrowIfNull(action, $"{nameof(action)} cannot be null");

            foreach (var item in list)
            {
                action(item);
            }
        }


        public static int ForEach<T>(this IEnumerable<T> list, Action<int, T> action)
        {
            Guard.ThrowIfNull(action, $"{nameof(action)} cannot be null");

            var index = 0;

            foreach (var item in list)
            {
                action(index, item);
                index++;
            }

            return index;
        }
    }
}
