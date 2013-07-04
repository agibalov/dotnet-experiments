using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors.SqlServer;
using NUnit.Framework;

namespace DapperExperiment.FluentMigratorTests
{
    public class FluentMigratorTests
    {
        [Test]
        public void CanCreateInitialSchemaWithFluentMigrator()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["migratorDatabase"].ConnectionString;

            var builder = new SqlCeConnectionStringBuilder(connectionString);
            var databaseFileName = builder.DataSource;

            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            using (var engine = new SqlCeEngine(connectionString))
            {
                engine.CreateDatabase();
            }

            var announcer = new NullAnnouncer();
            var runnerContext = new RunnerContext(announcer)
            {
                Namespace = "DapperExperiment.FluentMigratorTests"
            };

            var factory = new SqlServerCeProcessorFactory();
            var processor = factory.Create(
                connectionString, 
                announcer, 
                new MigrationOptions
                    {
                        PreviewOnly = false, 
                        Timeout = 60
                    });
            var migrationRunner = new MigrationRunner(
                Assembly.GetExecutingAssembly(), 
                runnerContext, 
                processor);
            migrationRunner.MigrateUp(true);

            using (var connection = new SqlCeConnection(connectionString))
            {
                connection.Execute("insert into Users(UserName) values(@userName)", new {userName = "loki2302"});
                var singleUser = connection.Query("select UserId, UserName from Users").Single();
                Assert.AreEqual(1, singleUser.UserId);
                Assert.AreEqual("loki2302", singleUser.UserName);
            }
        }
    }
}
