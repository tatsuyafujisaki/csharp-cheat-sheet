using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace CheatSheet.Database1
{
    static class Database
    {
        static readonly SqlConnectionStringBuilder Scsb;

        static Database()
        {
            switch (ConnectionStringFactory.GetDeploymentEnvironment())
            {
                case DeploymentEnvironment.Production:
                    Scsb = ConnectionStringFactory.Create(DatabaseServer.Production, "Database1");
                    break;
                case DeploymentEnvironment.Development1:
                    Scsb = ConnectionStringFactory.Create(DatabaseServer.Development, "Database1");
                    break;
                case DeploymentEnvironment.Development2:
                    Scsb = ConnectionStringFactory.Create(DatabaseServer.Development, "Database2");
                    break;
            }
        }

        static string GetConnectionStringWithoutPassword()
        {
            var scsb = new SqlConnectionStringBuilder(Scsb.ToString());
            scsb.Remove("Password");
            return scsb.ToString();
        }

        static T GetValue<T>(int id)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = "SELECT Column1 FROM Table1 WHERE Id = @Id";
                    c.Parameters.AddWithValue("@Id", id);

                    return (T)c.ExecuteScalar();
                }
            }
        }

        enum TableColumns1
        {
            Column1,
            Column2,
            Column3,
            Column4,
            Column5,
            Column6,
            Column7,
            Column8
        }

        static (bool, char, DateTime, decimal, double, int, string, byte[])? GetRecord(int id)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = $"SELECT {string.Join(",", Enum.GetNames(typeof(TableColumns1)))} FROM Table1 WHERE Id = @Id";
                    c.Parameters.AddWithValue("@Id", id);
                    using (var r = c.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return (r.GetBoolean((int)TableColumns1.Column1),
                                    r.GetChar((int)TableColumns1.Column2),
                                    r.GetDateTime((int)TableColumns1.Column3),
                                    r.GetDecimal((int)TableColumns1.Column4),
                                    r.GetDouble((int)TableColumns1.Column5),
                                    r.GetInt32((int)TableColumns1.Column6),
                                    r.GetString((int)TableColumns1.Column7),
                                    (byte[])r[(int)TableColumns1.Column8]);
                        }

                        return null;
                    }
                }
            }
        }

        static HashSet<string> GetRecordsOfOneColumn(int id)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = "SELECT Column1 FROM Table1 WHERE Id = @Id";
                    c.Parameters.AddWithValue("@Id", id);
                    using (var r = c.ExecuteReader())
                    {
                        // Use List<T> if the result is ordered or contains duplicate records.
                        var hs = new HashSet<string>();

                        while (r.Read())
                        {
                            hs.Add(r.GetString(0));
                        }

                        return hs;
                    }
                }
            }
        }

        enum TableColumns2
        {
            Column1,
            Column2
        }

        static Dictionary<string, double> SelectRecordsOfMultipleColumns(int id)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = $"SELECT {string.Format(",", Enum.GetNames(typeof(TableColumns2)))} FROM Table1 WHERE Id = @Id";
                    c.Parameters.AddWithValue("@Id", id);
                    using (var r = c.ExecuteReader())
                    {
                        // Use List<T, T> if the result is ordered or contains duplicate records.
                        var d = new Dictionary<string, double>();

                        while (r.Read())
                        {
                            d.Add(r.GetString((int)TableColumns2.Column1), r.GetDouble((int)TableColumns2.Column2));
                        }

                        return d;
                    }
                }
            }
        }

        static void Merge(string name, byte[] bs)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = "MERGE INTO Table1 T USING (SELECT @Name, @BinaryFile, @LastEditedBy) S(Name, BinaryFile, LastEditedBy) ON T.Name = S.Name WHEN MATCHED THEN UPDATE SET BinaryFile = S.BinaryFile, LastEditedBy = S.LastEditedBy, LastEditedAt = CURRENT_TIMESTAMP WHEN NOT MATCHED THEN INSERT (Name, BinaryFile, LastEditedBy) VALUES (S.Name, S.BinaryFile, S.LastEditedBy);";

                    c.Parameters.AddWithValue("@Name", name);
                    c.Parameters.AddWithValue("@BinaryFile", bs);
                    c.Parameters.AddWithValue("@LastEditedBy", Environment.UserName);

                    c.ExecuteNonQuery();
                }
            }
        }

        static void UpdateIfExists(string name, byte[] bs)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = "MERGE INTO Table1 T USING (SELECT @Name, @BinaryFile, @LastEditedBy) S(Name, BinaryFile, LastEditedBy) ON T.Name = S.Name WHEN MATCHED THEN UPDATE SET BinaryFile = S.BinaryFile, LastEditedBy = S.LastEditedBy, LastEditedWhen = CURRENT_TIMESTAMP;";

                    c.Parameters.AddWithValue("@Name", name);
                    c.Parameters.AddWithValue("@BinaryFile", bs);
                    c.Parameters.AddWithValue("@LastEditedBy", Environment.UserName);

                    c.ExecuteNonQuery();
                }
            }
        }

        static void InsertIfNotExists(string name, byte[] bs)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = "MERGE INTO Table1 T USING (SELECT @Name, @BinaryFile, @LastEditedBy) S(Name, BinaryFile, LastEditedBy) ON T.Name = S.Name WHEN NOT MATCHED THEN INSERT (Name, BinaryFile, LastEditedBy) VALUES (S.Name, S.BinaryFile, S.LastEditedBy);";

                    c.Parameters.AddWithValue("@Name", name);
                    c.Parameters.AddWithValue("@BinaryFile", bs);
                    c.Parameters.AddWithValue("@LastEditedBy", Environment.UserName);

                    c.ExecuteNonQuery();
                }
            }
        }

        static void DeleteInsert(string name)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var sc = new SqlConnection(Scsb.ConnectionString))
                    {
                        sc.Open();
                        using (var c = sc.CreateCommand())
                        {
                            c.CommandText = "DELETE FROM Table1 WHERE Name = @Name";
                            c.Parameters.AddWithValue("@Name", name);
                            c.ExecuteNonQuery();

                            c.CommandText = "INSERT INTO Table1 (Name, LastEditedBy) VALUES (@Name, @LastEditedBy)";
                            c.Parameters.AddWithValue("@LastEditedBy", Environment.UserName);
                            c.ExecuteNonQuery();
                        }
                    }

                    ts.Complete();
                }
            }
            catch
            {
                throw;
            }
        }

        static bool Exists(string name)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = @"SELECT TOP 1 NULL FROM Table WHERE Name = @Name";
                    c.Parameters.AddWithValue("@Name", name);

                    // "SELECT NULL" returns an instance of DbNull, which is not null.
                    return c.ExecuteScalar() != null;
                }
            }
        }

        // Usage
        //
        // var columns = new Dictionary<string, Type>
        // {
        //     { "Name", typeof(string) },
        //     { "BinaryFile", typeof(byte[]) },
        //     { "LastEditedBy", typeof(string) },
        // };
        // 
        // var rows = new List<object[]>
        // {
        //     new object[] { "Name1", new byte[] { 0x01 }, "Foo" },
        //     new object[] { "Name2", new byte[] { 0x10 }, "Bar" },
        //     new object[] { "Name3", new byte[] { 0x11 }, "Baz" }
        // };
        // 
        // Db.BulkInsert("Table1", columns, rows);
        [SuppressMessage("Microsoft.Security", "CA2100")]
        static void BulkInsert(string table, Dictionary<string, Type> columns, IEnumerable<object[]> rows)
        {
            using (var dt = new DataTable())
            {
                foreach (var column in columns)
                {
                    dt.Columns.Add(column.Key, column.Value);
                }

                foreach (var row in rows)
                {
                    dt.Rows.Add(row);
                }

                try
                {
                    using (var ts = new TransactionScope())
                    {
                        using (var sc = new SqlConnection(Scsb.ConnectionString))
                        {
                            sc.Open();
                            using (var c = sc.CreateCommand())
                            {
                                c.CommandText = string.Concat("TRUNCATE TABLE ", table);
                                c.ExecuteNonQuery();
                            }

                            using (var sbc = new SqlBulkCopy(sc) { DestinationTableName = table })
                            {
                                foreach (var columnName in columns.Keys)
                                {
                                    sbc.ColumnMappings.Add(columnName, columnName);
                                }

                                sbc.WriteToServer(dt);
                            }
                        }

                        ts.Complete();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        // CREATE PROCEDURE FallibleProcedure
        // @Error nvarchar(max) OUTPUT
        // AS
        // BEGIN
        //     BEGIN TRY
        //         SELECT 1 / 0
        //     END TRY
        //     BEGIN CATCH
        //         -- Use CONCAT in SQL Server 2012 or higher.
        //         SELECT @Error = 'ErrorNumber='
        //                 + CAST(ERROR_NUMBER() AS nvarchar)
        //                 + ', ErrorMessage=' + ERROR_MESSAGE()
        //                 + ', ErrorProcedure=' + ERROR_PROCEDURE()
        //                 + ', ErrorLineOfProcedure=' + CAST(ERROR_LINE() AS nvarchar)
        //         -- ERROR_STATE() is not useful as this function is for SQL Server dev team.
        //         RETURN -1
        //     END CATCH
        // END
        // GO
        [SuppressMessage("Microsoft.Security", "CA2100")]
        static (int, string) ExecuteStoredProcedure(string storedProcedure)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandType = CommandType.StoredProcedure;
                    c.CommandText = storedProcedure;

                    // The name and the type of ParameterDirection.Output must match those in the stored procedure.
                    var errorMessage = c.Parameters.Add("Error", SqlDbType.NVarChar, -1);
                    errorMessage.Direction = ParameterDirection.Output;

                    // The name of ParameterDirection.ReturnValue can be anything.
                    var returnValue = c.Parameters.Add("_", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;

                    c.ExecuteNonQuery();

                    return ValueTuple.Create((int)returnValue.Value, (string)errorMessage.Value);
                }
            }
        }

        static bool IsDuplicate(string column1)
        {
            using (var sc = new SqlConnection(Scsb.ConnectionString))
            {
                sc.Open();
                using (var c = sc.CreateCommand())
                {
                    c.CommandText = "SELECT TOP 1 NULL FROM Table1 WHERE Column1 = @Column1 GROUP BY Column1 HAVING 1 < COUNT(*)";
                    c.Parameters.AddWithValue("@Column1", column1);

                    // "SELECT NULL" returns an instance of DbNull, which is not null.
                    return c.ExecuteScalar() != null;
                }
            }
        }
    }
}
