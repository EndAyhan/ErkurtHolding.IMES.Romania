using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    /// <summary>
    /// Provides extension methods for casting one object type to another
    /// by mapping public properties with matching names.
    /// </summary>
    public static class CastingExtension
    {
        // Cache for property mappings: (sourceType, targetType) -> pairs of PropertyInfos
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, PropertyPair[]> _mapCache
            = new ConcurrentDictionary<Tuple<Type, Type>, PropertyPair[]>();

        /// <summary>
        /// Creates a new instance of <typeparamref name="T"/> and copies property values
        /// from the source object where property names match.
        /// </summary>
        /// <typeparam name="T">The target type to cast to.</typeparam>
        /// <param name="source">The source object. Must not be null.</param>
        /// <returns>
        /// A new instance of <typeparamref name="T"/> with matching properties populated.
        /// Properties that cannot be converted are skipped.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
        public static T Casting<T>(this object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var targetType = typeof(T);
            var sourceType = source.GetType();

            var key = Tuple.Create(sourceType, targetType);
            var map = _mapCache.GetOrAdd(key, _ => BuildMap(sourceType, targetType));

            // Create instance (works for reference and value types)
            var target = Activator.CreateInstance(targetType);

            // Copy values
            for (int i = 0; i < map.Length; i++)
            {
                var srcProp = map[i].Source;
                var dstProp = map[i].Target;

                var raw = srcProp.GetValue(source, null);
                if (raw == null)
                {
                    // If target is nullable type, set null, else leave default
                    if (IsNullable(dstProp.PropertyType))
                        dstProp.SetValue(target, null, null);

                    continue;
                }

                object valueToSet = raw;
                var dstType = dstProp.PropertyType;
                var nonNullableDst = Nullable.GetUnderlyingType(dstType) ?? dstType;

                // Convert if necessary
                if (!nonNullableDst.IsAssignableFrom(raw.GetType()))
                {
                    try
                    {
                        if (nonNullableDst.IsEnum)
                        {
                            // Support string or numeric to enum conversion
                            if (raw is string s)
                                valueToSet = Enum.Parse(nonNullableDst, s, ignoreCase: true);
                            else
                                valueToSet = Enum.ToObject(nonNullableDst, raw);
                        }
                        else
                        {
                            valueToSet = Convert.ChangeType(raw, nonNullableDst);
                        }
                    }
                    catch
                    {
                        // Incompatible conversion: skip assignment
                        continue;
                    }
                }

                dstProp.SetValue(target, valueToSet, null);
            }

            return (T)target;
        }

        /// <summary>
        /// Builds a property map between source and target types by matching property names.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>An array of property pairs to be copied.</returns>
        private static PropertyPair[] BuildMap(Type sourceType, Type targetType)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            // Target props: writable, non-indexer
            var targetProps = targetType.GetProperties(flags)
                .Where(p => p.CanWrite && p.GetIndexParameters().Length == 0);

            // Source props: readable, non-indexer
            var sourceProps = sourceType.GetProperties(flags)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                .ToDictionary(p => p.Name, p => p, StringComparer.Ordinal);

            // Join by name
            var pairs = targetProps
                .Where(tp => sourceProps.ContainsKey(tp.Name))
                .Select(tp => new PropertyPair(sourceProps[tp.Name], tp))
                .ToArray();

            return pairs;
        }

        /// <summary>
        /// Checks if a type is nullable (reference type or Nullable&lt;T&gt;).
        /// </summary>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || Nullable.GetUnderlyingType(t) != null;
        }

        /// <summary>
        /// Holds a pair of matching properties (source → target).
        /// </summary>
        private sealed class PropertyPair
        {
            public PropertyInfo Source { get; }
            public PropertyInfo Target { get; }

            public PropertyPair(PropertyInfo src, PropertyInfo dst)
            {
                Source = src;
                Target = dst;
            }
        }
    }
}
