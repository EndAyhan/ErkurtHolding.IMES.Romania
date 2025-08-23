using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    /// <summary>
    /// Extensions to convert objects and collections into <see cref="DataTable"/> instances.
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// Creates a <see cref="DataTable"/> with a single row populated from <paramref name="tentity"/>.
        /// Column names are public instance property names of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the source entity.</typeparam>
        /// <param name="tentity">The source object. Must not be null.</param>
        /// <param name="TableName">Optional table name. Defaults to <c>typeof(T).Name</c>.</param>
        /// <param name="propertyFilter">
        /// Optional filter to select which properties to include. If null, all readable, non-indexer public instance properties are included.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tentity"/> is null.</exception>
        public static DataTable CreateDataTable<T>(this T tentity, string TableName = null, Func<PropertyInfo, bool> propertyFilter = null) where T : class
        {
            if (tentity == null) throw new ArgumentNullException(nameof(tentity));
            var props = GetDataProperties(typeof(T), propertyFilter);

            var table = CreateEmptyTable(typeof(T), props, TableName);
            var row = table.NewRow();

            for (int i = 0; i < props.Length; i++)
                row[i] = GetValueOrDbNull(props[i], tentity);

            table.Rows.Add(row);
            return table;
        }

        /// <summary>
        /// Creates a <see cref="DataTable"/> populated from a list of entities.
        /// Column names are public instance property names of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the source entities.</typeparam>
        /// <param name="tentities">The source list. If null or empty, an empty table is returned with the proper schema.</param>
        /// <param name="TableName">Optional table name. Defaults to <c>typeof(T).Name</c>.</param>
        /// <param name="propertyFilter">
        /// Optional filter to select which properties to include. If null, all readable, non-indexer public instance properties are included.
        /// </param>
        public static DataTable CreateDataTable<T>(this List<T> tentities, string TableName = null, Func<PropertyInfo, bool> propertyFilter = null) where T : class
        {
            // Delegate to IEnumerable<T> overload for shared logic
            return CreateDataTable((IEnumerable<T>)tentities, TableName, propertyFilter);
        }

        /// <summary>
        /// Creates a <see cref="DataTable"/> populated from a sequence of entities.
        /// Column names are public instance property names of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the source entities.</typeparam>
        /// <param name="source">The source sequence. If null or empty, an empty table is returned with the proper schema.</param>
        /// <param name="TableName">Optional table name. Defaults to <c>typeof(T).Name</c>.</param>
        /// <param name="propertyFilter">
        /// Optional filter to select which properties to include. If null, all readable, non-indexer public instance properties are included.
        /// </param>
        public static DataTable CreateDataTable<T>(this IEnumerable<T> source, string TableName = null, Func<PropertyInfo, bool> propertyFilter = null) where T : class
        {
            var props = GetDataProperties(typeof(T), propertyFilter);
            var table = CreateEmptyTable(typeof(T), props, TableName);

            if (source == null)
                return table;

            foreach (var item in source)
            {
                if (item == null) continue;
                var row = table.NewRow();

                for (int i = 0; i < props.Length; i++)
                    row[i] = GetValueOrDbNull(props[i], item);

                table.Rows.Add(row);
            }
            return table;
        }

        // ---------- helpers ----------

        private static PropertyInfo[] GetDataProperties(Type t, Func<PropertyInfo, bool> filter)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            var props = t.GetProperties(flags)
                         .Where(p => p.CanRead && p.GetIndexParameters().Length == 0); // skip indexers

            if (filter != null)
                props = props.Where(filter);

            // Keep stable order by metadata name
            return props.ToArray();
        }

        private static DataTable CreateEmptyTable(Type t, PropertyInfo[] props, string tableName)
        {
            var table = new DataTable(string.IsNullOrEmpty(tableName) ? t.Name : tableName);

            foreach (var p in props)
            {
                var colType = GetColumnType(p.PropertyType);
                table.Columns.Add(p.Name, colType);
            }

            return table;
        }

        /// <summary>
        /// Determines the <see cref="Type"/> to use for the <see cref="DataColumn"/> based on a property type.
        /// - <c>Nullable&lt;T&gt;</c> becomes <c>T</c>.
        /// - <c>enum</c> types are stored as <see cref="string"/> (enum name).
        /// - Other types are used as-is.
        /// </summary>
        private static Type GetColumnType(Type propertyType)
        {
            var underlying = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (underlying.IsEnum)
                return typeof(string); // store enum names (more portable to consumers)

            return underlying;
        }

        /// <summary>
        /// Gets the value of a property on an object, converting it for the DataTable:
        /// - <c>null</c> → <see cref="DBNull.Value"/>
        /// - <c>enum</c> → <see cref="string"/> name
        /// - <c>Nullable&lt;T&gt;</c> unwraps underlying value
        /// </summary>
        private static object GetValueOrDbNull(PropertyInfo p, object instance)
        {
            var value = p.GetValue(instance, null);
            if (value == null)
                return DBNull.Value;

            var type = value.GetType();
            var underlying = Nullable.GetUnderlyingType(type) ?? type;

            if (underlying.IsEnum)
                return Enum.GetName(underlying, value) ?? value.ToString();

            return value;
        }
    }
}
