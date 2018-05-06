using System;
using System.ComponentModel;
using System.Data.SqlClient;

namespace CheatSheet.Db1
{
    public enum DeploymentEnvironment
    {
        Production,
        Development1,
        Development2
    }

    public enum DatabaseServer
    {
        Production,
        Development
    }

    static class ConnectionStringFactory
    {
        internal static DeploymentEnvironment GetDeploymentEnvironment()
        {
            var directories = Io.Directory1.SplitIntoDirectories(AppDomain.CurrentDomain.BaseDirectory);

            if (Bool.String1.ContainsIgnoreCase(directories, "production"))
            {
                return DeploymentEnvironment.Production;
            }

            return Bool.String1.ContainsIgnoreCase(directories, "uat1") ? DeploymentEnvironment.Development1 : DeploymentEnvironment.Development2;
        }

        internal static SqlConnectionStringBuilder Create(DatabaseServer ds, string database)
        {
            var scsb = new SqlConnectionStringBuilder { InitialCatalog = database };

            switch (ds)
            {
                case DatabaseServer.Production:
                    scsb.DataSource = "production.example.com";
                    switch (database)
                    {
                        case "Database1":
                            scsb.UserID = "user1";
                            scsb.Password = "password1";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(database), database, "Must be Database1.");
                    }
                    break;
                case DatabaseServer.Development:
                    scsb.DataSource = "development.example.com";
                    switch (database)
                    {
                        case "Database1":
                            scsb.UserID = "user1";
                            scsb.Password = "password1";
                            break;
                        case "Database2":
                            scsb.UserID = "user1";
                            scsb.Password = "password1";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(database), database, "Must be Database1 or Database2.");
                    }
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(ds), (int)ds, typeof(DatabaseServer));
            }

            return scsb;
        }
    }
}