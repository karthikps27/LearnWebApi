using DbUp;
using DbUp.Engine;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class SqlDBMigrator
    {
        public static void RunDBMigration()
        {
            UpgradeEngine upgradeEngine = DeployChanges.To
                .PostgresqlDatabase(GetConnectionString())
                .WithScriptsFromFileSystem("sqlScriptDirectory")
                .LogToConsole()
                .Build();
        }

        private static string GetConnectionString()
        {
            return new NpgsqlConnectionStringBuilder
            {
                Host = "serverAddress",
                Port = 5432,
                Database = "databasename",
                Username = "username",
                Password = "password"
            }.ConnectionString;
        }
    }
}
