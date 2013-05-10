using System.Data.Entity.Migrations;
using System.Linq;
using NUnit.Framework;

namespace EFCodeFirstMigrationsExperiment
{
    public static class DbMigratorAssertions
    {
        public static void AssertThereAreAppliedMigrations(this DbMigrator migrator, params string[] migrationNames)
        {
            var databaseMigrations = migrator
                .GetDatabaseMigrations()
                .OrderBy(x => x)
                .ToList();
            Assert.AreEqual(migrationNames.Length, databaseMigrations.Count);
            for (var i = 0; i < migrationNames.Length; ++i)
            {
                Assert.AreEqual(migrationNames[i], databaseMigrations[i]);
            }
        }

        public static void AssertThereAreKnownMigrations(this DbMigrator migrator, params string[] migrationNames)
        {
            var localMigrations = migrator
                .GetLocalMigrations()
                .OrderBy(x => x)
                .ToList();
            Assert.AreEqual(migrationNames.Length, localMigrations.Count);
            for (var i = 0; i < migrationNames.Length; ++i)
            {
                Assert.AreEqual(migrationNames[i], localMigrations[i]);
            }
        }

        public static void AssertThereArePendingMigrations(this DbMigrator migrator, params string[] migrationNames)
        {
            var pendingMigrations = migrator
                .GetPendingMigrations()
                .OrderBy(x => x)
                .ToList();
            Assert.AreEqual(migrationNames.Length, pendingMigrations.Count);
            for (var i = 0; i < migrationNames.Length; ++i)
            {
                Assert.AreEqual(migrationNames[i], pendingMigrations[i]);
            }
        }
    }
}