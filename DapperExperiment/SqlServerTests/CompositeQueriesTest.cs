using System.Linq;
using Dapper;
using NUnit.Framework;

namespace DapperExperiment.SqlServerTests
{
    public class CompositeQueriesTest : AbstractSqlServerTest
    {
        [Test]
        public void CanInsertAndRetrieveId()
        {
            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                connection.Execute(@"
                    create table Posts(
                        Id int identity(1, 1) primary key,
                        Content nvarchar(256) not null)");
            }

            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                var id = connection.Query<int>(@"
                    insert into Posts(Content) values(@content);
                    select cast(@@identity as int);",
                    new {content = "hello"}).Single();
                Assert.AreEqual(1, id);
            }
        }

        [Test]
        public void CanInsertAndRetrieveRecord()
        {
            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                connection.Execute(@"
                    create table Posts(
                        Id int identity(1, 1) primary key,
                        Content nvarchar(256) not null)");
            }

            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                var post = connection.Query(@"
                    insert into Posts(Content) values(@content);
                    select Id, Content from Posts where Id = @@identity;",
                    new { content = "hello" }).Single();
                Assert.AreEqual(1, post.Id);
                Assert.AreEqual("hello", post.Content);
            }
        }

        [Test]
        public void CanRetrieveMultipleResultSets()
        {
            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                connection.Execute(@"
                    create table Posts(
                        Id int primary key,
                        Content nvarchar(256) not null)");

                connection.Execute(@"
                    create table Comments(
                        Id int primary key,
                        Text nvarchar(256) not null,
                        PostId int not null foreign key references Posts(Id))");

                connection.Execute("insert into Posts(Id, Content) values(@id, @content)", new { id = 1, content = "First post" });
                connection.Execute("insert into Comments(Id, Text, PostId) values(@id, @text, @postId)", new { id = 1, text = "Comment #1", postId = 1 });
                connection.Execute("insert into Comments(Id, Text, PostId) values(@id, @text, @postId)", new { id = 2, text = "Comment #2", postId = 1 });
                connection.Execute("insert into Comments(Id, Text, PostId) values(@id, @text, @postId)", new { id = 3, text = "Comment #3", postId = 1 });
            }

            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                var reader = connection.QueryMultiple(@"
                    select Id, Content from Posts;
                    select Id, Text, PostId from Comments;", 
                                                      new { postId = 1 });

                var postRow = reader.Read().Single();
                Assert.AreEqual(1, postRow.Id);
                Assert.AreEqual("First post", postRow.Content);

                var commentRows = reader.Read().ToList();
                Assert.AreEqual(1, commentRows[0].Id);
                Assert.AreEqual("Comment #1", commentRows[0].Text);
                Assert.AreEqual(1, commentRows[0].PostId);

                Assert.AreEqual(2, commentRows[1].Id);
                Assert.AreEqual("Comment #2", commentRows[1].Text);
                Assert.AreEqual(1, commentRows[1].PostId);

                Assert.AreEqual(3, commentRows[2].Id);
                Assert.AreEqual("Comment #3", commentRows[2].Text);
                Assert.AreEqual(1, commentRows[2].PostId);
            }
        }
    }
}