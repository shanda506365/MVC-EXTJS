
namespace USO.Domain.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this ICollection<T> instance)
        {
            return (instance == null) || (instance.Count == 0);
        }

        [DebuggerStepThrough]
        public static bool IsEmpty<T>(this ICollection<T> instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            return instance.Count == 0;
        }

        [DebuggerStepThrough]
        public static void AddRange<T>(this ICollection<T> instance, IEnumerable<T> collection)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (T item in collection)
            {
                instance.Add(item);
            }
        }
    }
}
