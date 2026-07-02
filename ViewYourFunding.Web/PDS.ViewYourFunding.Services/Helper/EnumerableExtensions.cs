using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Contains reusable Extension methods for any Generic Enumerable type.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// To select recrusively all child of parent objects.
        /// </summary>
        /// <typeparam name="T">Class of Parent Objects.</typeparam>
        /// <param name="source">Parent Object.</param>
        /// <param name="selector">Selector is a Lamba function to projects the child objects.</param>
        /// <returns>Enumerated value of parents its all child (recrusive).</returns>
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);

                if (children != null)
                {
                    foreach (var child in SelectRecursive(children, selector))
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}
