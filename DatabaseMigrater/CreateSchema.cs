using ComicsLibrary.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;

namespace DatabaseMigrater
{
    public static class CreateSchema
    {
        public static void Run(string connectionString)
        {
            using (var context = new DbContext(connectionString))
            {
                var config = new Configuration();
                config.TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient");
                var migrator = new MigratorLoggingDecorator(new DbMigrator(config), new ComicsLibraryMigrationsLogger());
                if (migrator.GetPendingMigrations().Any())
                {
                    migrator.Update();
                }
            }
        }

        private static void RunMigration(DbContext context, DbMigration migration)
        {
            var prop = migration.GetType().GetProperty("Operations", BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null)
            {
                IEnumerable<MigrationOperation> operations = prop.GetValue(migration) as IEnumerable<MigrationOperation>;
                var generator = new SqlServerMigrationSqlGenerator();
                var statements = generator.Generate(operations, "2008");
                foreach (MigrationStatement item in statements)
                    context.Database.ExecuteSqlCommand(item.Sql);
            }
        }

        public class ComicsLibraryMigrationsLogger : MigrationsLogger
        {
            public override void Info(string message)
            {
                Console.WriteLine("INFO: " + message);
            }

            public override void Verbose(string message)
            {
                Console.WriteLine("VERBOSE: " + message);
            }

            public override void Warning(string message)
            {
                Console.WriteLine("WARNING: " + message);
            }
        }
    }
}
