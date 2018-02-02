using System;

namespace Recipes.Common
{
    public static class Guard
    {
        public static void ThrowIf(bool value, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            if (value) throw new Exception(message);
        }

        public static void ThrowIf(Func<bool> condition, string message)
        {
            ThrowIf(condition(), message);
        }

        public static void ThrowIf(Func<bool> condition, Func<string> messageProvider)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (messageProvider == null) throw new ArgumentNullException(nameof(messageProvider));

            ThrowIf(condition(), messageProvider());
        }

        public static void ThrowIf(Func<bool> condition, Func<Exception> exceptionFactory)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));

            ThrowIf(condition(), exceptionFactory);
        }

        public static void ThrowIf(bool condition, Func<Exception> exceptionFactory)
        {
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            if (condition) throw exceptionFactory();
        }

        public static void ThrowIfNull<T>(T obj, string message) where T : class
        {
            if (obj == null) throw new NullReferenceException(message);
        }

        public static void ThrowIfNullOrEmpty(string text, string message)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new Exception(message);
        }

        public static void ThrowIfNullOrEmpty(Guid id, string message)
        {
            if (id == Guid.Empty) throw new Exception(message);
        }
    }
}
