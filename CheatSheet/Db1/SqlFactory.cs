using System.Linq;

namespace CheatSheet.Db1
{
    static class SqlFactory
    {
        static string Update(string table, string where, string[] columns) => $"UPDATE {table} SET {string.Join(", ", columns.Zip(columns.Select(s => "@" + s), (s, v) => s + " = " + v))} WHERE {where}";

        // Column names can be omitted in an INSERT statement when values of all columns are given,
        // but the case is rare because most tables have the CreatedAt column that has a DEFAULT constraint
        // so a value of the column is not given for simplicity.
        static string Insert(string table, string[] columns) => $"INSERT INTO {table} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", columns.Select(s => "@" + s))})";

        // This is useful when there are two target tables. For example, "update on table A or update on table B".
        static string ThenElse(string table, string where, string thenSql, string elseSql) => $"IF EXISTS(SELECT NULL FROM {table} WHERE {where}) {thenSql} ELSE {elseSql}";
    }
}