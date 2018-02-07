using System.Data.Entity.Migrations;
using System.Linq;
using EFCodeFirstMigrationsExperiment.V1;
using EFCodeFirstMigrationsExperiment.V2;
using NUnit.Framework;

namespace EFCodeFirstMigrationsExperiment
{
    [TestFixture]
    public class CodeFirstMigrationTests
    {
        private const string Migration1 = "201305100145199_InitializeMigration";
        private const string Migration2 = "201305100152178_AddPostTitle";

        [SetUp]
        public void DropDatabaseIfExists()
        {
            using (var context = new MyContextV1())
            {
                var database = context.Database;
                if (database.Exists())
                {
                    database.Delete();
                }

                // NOTE: there's no database.Create();
            }
        }

        [Test]
        public void CanCreateV1DatabaseAndPlayWithIt()
        {
            var migrator = new DbMigrator(new MyContextV1Configuration());
            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations();
            migrator.AssertThereArePendingMigrations(Migration1, Migration2);

            migrator.Update(Migration1);

            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations(Migration1);
            migrator.AssertThereArePendingMigrations(Migration2);

            using (var context = new MyContextV1())
            {
                var post = new PostV1
                    {
                        PostText = "my post #1"
                    };
                context.Posts.Add(post);
                context.SaveChanges();
            }

            using (var context = new MyContextV1())
            {
                var post = context.Posts.Single();
                Assert.AreEqual(1, post.PostId);
                Assert.AreEqual("my post #1", post.PostText);
            }
        }

        [Test]
        public void CanCreateV2DatabaseAndPlayWithIt()
        {
            var migrator = new DbMigrator(new MyContextV2Configuration());
            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations();
            migrator.AssertThereArePendingMigrations(Migration1, Migration2);

            migrator.Update(Migration2);

            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations(Migration1, Migration2);
            migrator.AssertThereArePendingMigrations();

            using (var context = new MyContextV2())
            {
                var post = new PostV2
                    {
                        PostText = "my post #1",
                        PostTitle = "my post #1 title"
                    };
                context.Posts.Add(post);
                context.SaveChanges();
            }

            using (var context = new MyContextV2())
            {
                var post = context.Posts.Single();
                Assert.AreEqual(1, post.PostId);
                Assert.AreEqual("my post #1", post.PostText);
                Assert.AreEqual("my post #1 title", post.PostTitle);
            }
        }

        [Test]
        public void CanCreateV1DatabaseThenPlayWithItThenMigrateToV2ThenPlayWithIt()
        {
            var migrator = new DbMigrator(new MyContextV1Configuration());
            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations();
            migrator.AssertThereArePendingMigrations(Migration1, Migration2);

            migrator.Update(Migration1);

            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations(Migration1);
            migrator.AssertThereArePendingMigrations(Migration2);

            using (var context = new MyContextV1())
            {
                var post = new PostV1
                    {
                        PostText = "my post #1"
                    };
                context.Posts.Add(post);
                context.SaveChanges();
            }

            migrator.Update(Migration2);

            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations(Migration1, Migration2);
            migrator.AssertThereArePendingMigrations();

            using (var context = new MyContextV2())
            {
                var post = context.Posts.Single();
                Assert.AreEqual(1, post.PostId);
                Assert.AreEqual("my post #1", post.PostText);
                Assert.AreEqual("Default Post Title", post.PostTitle);
            }
        }

        [Test]
        public void CanCreateV2DatabaseThenPlayWithItThenMigrateToV1ThenPlayWithIt()
        {
            var migrator = new DbMigrator(new MyContextV2Configuration());
            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations();
            migrator.AssertThereArePendingMigrations(Migration1, Migration2);

            migrator.Update(Migration2);

            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations(Migration1, Migration2);
            migrator.AssertThereArePendingMigrations();

            using (var context = new MyContextV2())
            {
                var post = new PostV2
                    {
                        PostText = "my post #1",
                        PostTitle = "my post #1 title"
                    };
                context.Posts.Add(post);
                context.SaveChanges();
            }

            migrator.Update(Migration1);

            migrator.AssertThereAreKnownMigrations(Migration1, Migration2);
            migrator.AssertThereAreAppliedMigrations(Migration1);
            migrator.AssertThereArePendingMigrations(Migration2);

            using (var context = new MyContextV1())
            {
                var post = context.Posts.Single();
                Assert.AreEqual(1, post.PostId);
                Assert.AreEqual("my post #1", post.PostText);
            }
        }
    }
}