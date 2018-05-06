using CheatSheet.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CheatSheet
{
    static class DataTableView
    {
        static class DataSet1
        {
            static void SortRows(DataSet ds, string tableName, string columnName)
            {
                var dv = ds.Tables[tableName].DefaultView;
                dv.Sort = columnName;
                ds.Tables.Remove(tableName);
                ds.Tables.Add(dv.ToTable());
            }
        }

        static class DataTable1
        {
            static DataColumn[] GetColumns(DataTable dt, IEnumerable<string> columnNames) => columnNames.Select(columnName => dt.Columns[columnName]).ToArray();

            internal static DataRow GetRow(DataTable dt, IEnumerable<string> columnNames, object key)
            {
                dt.PrimaryKey = GetColumns(dt, columnNames);
                return dt.Rows.Find(key);
            }

            internal static DataRow GetRow(DataTable dt, IEnumerable<string> columnNames, object[] keys)
            {
                dt.PrimaryKey = GetColumns(dt, columnNames);
                return dt.Rows.Find(keys);
            }

            internal static IEnumerable<DataRow> GetRows(DataTable dt) => dt.Select();
            internal static IEnumerable<DataRow> GetRows(DataTable dt, string filter) => dt.Select(filter);
            internal static IEnumerable<DataRow> GetRows(DataTable dt, string filter, string sort) => dt.Select(filter, sort);

            internal static void SetValue(DataTable dt, int columnIndex, string value) => SetColumnValue(dt.Columns[columnIndex], value);
            internal static void SetValue(DataTable dt, string columnName, string value) => SetColumnValue(dt.Columns[columnName], value);
        }

        static class DataView1
        {
            internal static DataRowView GetRow(DataView dv, string columnName, object key)
            {
                dv.Sort = columnName;
                var i = dv.Find(key);
                return i == -1 ? null : dv[i];
            }

            internal static DataRowView GetRow(DataView dv, string columnName, object[] keys)
            {
                dv.Sort = columnName;
                var i = dv.Find(keys);
                return i == -1 ? null : dv[i];
            }

            internal static IEnumerable<DataRowView> GetRows(DataView dv, string columnName, object key)
            {
                dv.Sort = columnName;
                return dv.FindRows(key);
            }

            internal static IEnumerable<DataRowView> GetRows(DataView dv, string columnName, object[] keys)
            {
                dv.Sort = columnName;
                return dv.FindRows(keys);
            }
        }

        static void SetColumnValue(DataColumn dc, string value)
        {
            dc.Expression = Bool.String1.IsNumeric(value) ? value : String1.SingleQuote(value);
        }

        static void Demo()
        {
            using (var dt = new DataTable())
            {
                // DataTable.AddRange(DataColumn[]) is faster than DataTable.Add(string, Type) even though the former calls "new DataColumn" for each column.
                dt.Columns.AddRange(new[] { new DataColumn("FirstName", typeof(string)),
                                            new DataColumn("LastName", typeof(string)),
                                            new DataColumn("Age", typeof(int)) });

                //dt.Columns.Add("FirstName", typeof(string));
                //dt.Columns.Add("LastName", typeof(string));
                //dt.Columns.Add("Age", typeof(int));

                dt.Rows.Add("Dmitri", "Karamazov", 28);
                dt.Rows.Add("Ivan", "Karamazov", 24);
                dt.Rows.Add("Alexei", "Karamazov", 20);

                var columns = new[] { "FirstName" };
                const string findMe = "Ivan";
                var dr1 = DataTable1.GetRow(dt, columns, findMe);
                Console.WriteLine("DataTable.Rows.Find() using ({0}) = ({1})", string.Join(", ", columns), string.Join(", ", findMe));
                Console.WriteLine("=> {0} {1} is found: Age = {2}\n", dr1["FirstName"], dr1["LastName"], dr1["Age"]);

                columns = new[] { "FirstName", "LastName" };
                var findObjects = new object[] { "Ivan", "Karamazov" };
                dr1 = DataTable1.GetRow(dt, columns, findObjects);
                Console.WriteLine("DataTable.Rows.Find() using ({0}) = ({1})", string.Join(", ", columns), string.Join(", ", findObjects));
                Console.WriteLine("=> {0} {1} is found: Age = {2}\n", dr1["FirstName"], dr1["LastName"], dr1["Age"]);

                Console.WriteLine("DataTable.Rows.Contains() using ({0}) = ({1})", string.Join(", ", columns), string.Join(", ", findObjects));
                Console.WriteLine("=> {0} {1} exists?: {2}\n", dr1["FirstName"], dr1["LastName"], dt.Rows.Contains(findObjects));

                Console.WriteLine("DataTable.Select() without filter");
                foreach (var dr in DataTable1.GetRows(dt))
                {
                    Console.WriteLine("=> {0} {1} is found: Age = {2}", dr["FirstName"], dr["LastName"], dr["Age"]);
                }
                Console.WriteLine();

                const string filter = "20 < Age";

                Console.WriteLine("DataTable.Select() using filter ({0})", filter);
                foreach (var dr in DataTable1.GetRows(dt, filter))
                {
                    Console.WriteLine("=> {0} {1} is found: Age = {2}", dr["FirstName"], dr["LastName"], dr["Age"]);
                }
                Console.WriteLine();

                const string sort = "Age DESC";
                Console.WriteLine("DataTable.Select() using filter ({0}) then sort ({1})", filter, sort);
                foreach (var dr in DataTable1.GetRows(dt, filter, sort))
                {
                    Console.WriteLine("=> {0} {1} is found: Age = {2}", dr["FirstName"], dr["LastName"], dr["Age"]);
                }
                Console.WriteLine();

                var dv = dt.DefaultView;

                var column = "Age";
                var findInt = 20;
                var drv1 = DataView1.GetRow(dv, column, findInt);
                Console.WriteLine("DataView.Find() using {0} = {1}", column, findInt);
                Console.WriteLine("=> {0} {1} is found: Age = {2}\n", drv1["FirstName"], drv1["LastName"], drv1["Age"]);

                column = "Age, LastName";
                findObjects = new object[] { "20", "Karamazov" };
                drv1 = DataView1.GetRow(dv, column, findObjects);
                Console.WriteLine("DataView.Find() using ({0}) = ({1})", column, string.Join(", ", findObjects));
                Console.WriteLine("=> {0} {1} is found: Age = {2}\n", drv1["FirstName"], drv1["LastName"], drv1["Age"]);

                column = "Age";
                findInt = 20;
                Console.WriteLine("DataView.FindRows() using {0} = {1}", column, findInt);
                foreach (var drv in DataView1.GetRows(dv, column, findInt))
                {
                    Console.WriteLine("=> {0} {1} is found: Age = {2}", drv["FirstName"], drv["LastName"], drv["Age"]);
                }
                Console.WriteLine();

                column = "Age, LastName";
                findObjects = new object[] { "20", "Karamazov" };
                Console.WriteLine("DataView.FindRows() using ({0}) = ({1})", column, string.Join(", ", findObjects));
                foreach (var drv in DataView1.GetRows(dv, column, findObjects))
                {
                    Console.WriteLine("=> {0} {1} is found: Age = {2}", drv["FirstName"], drv["LastName"], drv["Age"]);
                }
                Console.WriteLine();

                dv.RowFilter = filter;

                Console.WriteLine("DataView.RowFilter() using filter ({0})", filter);
                foreach (DataRowView drv in dv)
                {
                    Console.WriteLine("=> {0} {1} is found: Age = {2}", drv["FirstName"], drv["LastName"], drv["Age"]);
                }
                Console.WriteLine();

                dv.RowFilter = "";

                Console.WriteLine("DataView.RowFilter() without filter");
                foreach (DataRowView drv in dv)
                {
                    Console.WriteLine("=> {0} {1} is found: Age = {2}", drv["FirstName"], drv["LastName"], drv["Age"]);
                }
                Console.WriteLine();

                DataTable1.SetValue(dt, 2, "50");
                Console.WriteLine("DataTable.Columns[ColumnIndex].Expression = Value");
                foreach (var dr in DataTable1.GetRows(dt))
                {
                    Console.WriteLine("{0} {1} is Age {2}", dr["FirstName"], dr["LastName"], dr["Age"]);
                }
                Console.WriteLine();

                DataTable1.SetValue(dt, "Age", "100");
                Console.WriteLine("DataTable.Columns[ColumnName].Expression = Value");
                foreach (var dr in DataTable1.GetRows(dt))
                {
                    Console.WriteLine("{0}, {1} is Age {2}", dr["FirstName"], dr["LastName"], dr["Age"]);
                }
            }
        }
    }
}
