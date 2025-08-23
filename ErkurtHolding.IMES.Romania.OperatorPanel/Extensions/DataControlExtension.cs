using System;
using System.Collections;
using System.Collections.Generic;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    /// <summary>
    /// Extensions for safe, allocation‑free checks on collections and sequences.
    /// </summary>
    public static class DataControlExtension
    {
        /// <summary>
        /// Returns <c>true</c> if the list is non‑null and contains at least one element.
        /// (Backwards‑compatible with your existing signature.)
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="entites">The list to test.</param>
        /// <returns><c>true</c> if non‑null and non‑empty; otherwise <c>false</c>.</returns>
        public static bool HasEntries<T>(this List<T> source) where T : class
        {
            return source != null && source.Count > 0;
        }

        /// <summary>
        /// Returns <c>true</c> if the sequence is non‑null and contains at least one element.
        /// Optimized for <see cref="ICollection{T}"/>, <see cref="IReadOnlyCollection{T}"/>, and <see cref="ICollection"/>.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Sequence to test.</param>
        /// <returns><c>true</c> if non‑null and non‑empty; otherwise <c>false</c>.</returns>
        public static bool HasEntries<T>(this IEnumerable<T> source)
        {
            if (source == null) return false;

            // O(1) fast paths
            var c1 = source as ICollection<T>;
            if (c1 != null) return c1.Count > 0;

            var c2 = source as IReadOnlyCollection<T>;
            if (c2 != null) return c2.Count > 0;

            var c3 = source as ICollection;
            if (c3 != null) return c3.Count > 0;

            // Fallback: check first item without allocating LINQ iterators
            using (var e = source.GetEnumerator())
                return e.MoveNext();
        }

        /// <summary>
        /// Returns <c>true</c> if the sequence is non‑null and has at least one element
        /// that satisfies the given <paramref name="predicate"/>.
        /// Optimized for <see cref="ICollection{T}"/> and <see cref="IReadOnlyCollection{T}"/> by early exit.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Sequence to test.</param>
        /// <param name="predicate">Condition to test each element against.</param>
        /// <returns><c>true</c> if any element matches; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate"/> is <c>null</c>.</exception>
        public static bool HasEntries<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) return false;
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            // Iterate until we find a match; no allocations
            foreach (var item in source)
            {
                if (predicate(item)) return true;
            }
            return false;
        }
    }
}
