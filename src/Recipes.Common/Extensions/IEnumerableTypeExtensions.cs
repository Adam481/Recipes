using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Recipes.Common.Extensions
{
    public static class IEnumerableTypeExtensions
    {
        // TODO caching

        public static IEnumerable<Type> GetImplementationsOf<TInterface>(this IEnumerable<Type> types)
            => types.GetImplementationsOf(typeof(TInterface));


        public static IEnumerable<Type> GetImplementationsOf(this IEnumerable<Type> types, Type interfaceType)
        {
            Guard.ThrowIfNull(interfaceType, "Interface type cannot be null");
            Guard.ThrowIf(!interfaceType.IsInterface, $"Argument {nameof(interfaceType)} has to be generic type");

            var result = types.Where(type => interfaceType.IsAssignableFrom(type));
            return result;
        }


        public static IEnumerable<Type> GetGenericImplementationsOf(this IEnumerable<Type> types, Type interfaceType)
        {
            Guard.ThrowIf(!interfaceType.IsInterface, $"Argument {nameof(interfaceType)} has to be interface type");
            Guard.ThrowIf(!interfaceType.IsGenericType, $"Argument {nameof(interfaceType)} has to be generic type");

            var result = types
                 .Where(t => t.GetInterfaces()
                     .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType));

            return result;
        }


        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types) where TAttribute : Attribute
            => types.Where(t => t.GetCustomAttribute<TAttribute>() != null);


        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types, Func<TAttribute, bool> predicate)
             where TAttribute : Attribute
        {
            Guard.ThrowIfNull(predicate, "Predicate cannot be null");

            var result = types.Where(t =>
            {
                var attr = t.GetCustomAttribute<TAttribute>();
                return attr != null && predicate(attr);
            });

            return result;
        }
    }
}
