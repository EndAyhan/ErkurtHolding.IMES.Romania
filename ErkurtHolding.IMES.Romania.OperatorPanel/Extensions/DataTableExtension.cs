using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    public static class DataTableExtension
    {
        public static DataTable CreateDataTable<T>(this T tentity, string TableName = null) where T : class
        {
            //public DataTable ToDataTable<T>(List<T> items)
            DataTable dataTable;
            if (TableName == null)
                dataTable = new DataTable(typeof(T).Name);
            else
                dataTable = new DataTable(TableName);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                values[i] = Props[i].GetValue(tentity, null);
            }
            dataTable.Rows.Add(values);

            return dataTable;
        }

        public static DataTable CreateDataTable<T>(this List<T> tentities, string TableName = null) where T : class
        {
            //public DataTable ToDataTable<T>(List<T> items)
            DataTable dataTable;
            if (TableName == null)
                dataTable = new DataTable(typeof(T).Name);
            else
                dataTable = new DataTable(TableName);
            //Get all the properties
            bool flag = true;
            foreach (var tentity in tentities)
            {
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                if (flag)
                {
                    foreach (PropertyInfo prop in Props)
                    {
                        dataTable.Columns.Add(prop.Name);
                    }
                    flag = false;
                }

                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(tentity, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
