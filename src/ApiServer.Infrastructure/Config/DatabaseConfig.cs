using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ApiServer.Infrastructure.Config
{
    public class DatabaseConfig
    {
        public static string ConnectionString { get; private set; }
        public static DatabaseType DbType { get; private set; } = DatabaseType.SqlServer;
        public static string Version { get; private set; }

        public static void InitConfiguration(IConfiguration configuration)
        {
            var dbTypeStr = configuration["DataBase:Dialect"];
            var connectString = configuration.GetConnectionString("Default");
            switch (dbTypeStr.ToLower())
            {
                case "mysql":
                    DbType = DatabaseType.MySql;
                    Version = configuration["Database:Version"];
                    break;

                case "sqlserver":
                    DbType = DatabaseType.SqlServer;
                    break;

                case "oracle":
                    DbType = DatabaseType.Oracle;
                    break;

                case "sqlite":
                    DbType = DatabaseType.Sqlite;
                    connectString = $"DataSource=" + Path.Combine(Environment.CurrentDirectory, connectString);
                    break;

                default:
                    DbType = DatabaseType.Unknown;
                    break;
            }

            ConnectionString = connectString;
        }
    }

    public enum DatabaseType
    {
        MySql,
        SqlServer,
        Sqlite,
        Oracle,
        PostgreSQL,
        Unknown
    }
}
