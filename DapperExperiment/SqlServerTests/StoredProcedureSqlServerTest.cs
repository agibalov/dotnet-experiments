using System.Data;
using System.Linq;
using Dapper;
using NUnit.Framework;

namespace DapperExperiment.SqlServerTests
{
    public class StoredProcedureSqlServerTest : AbstractSqlServerTest
    {
        [Test]
        public void CanUseStoredProcedure()
        {
            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                connection.Execute(@"
                    create table Posts(
                        Id int identity(1, 1) primary key,
                        Content nvarchar(256) not null)");
                
                connection.Execute(@"
                    create procedure CreatePost(@content nvarchar(256)) as
                    begin
                        insert into Posts(Content) values(@content);
                        select Id, Content from Posts where Id = scope_identity();
                    end");
            }

            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                Assert.AreEqual(0, connection.Query<int>("select count(Id) from Posts").Single());

                var post = connection.Query("CreatePost", new {content = "hello there"}, commandType: CommandType.StoredProcedure).Single();
                Assert.AreEqual(1, post.Id);
                Assert.AreEqual("hello there", post.Content);

                Assert.AreEqual(1, connection.Query<int>("select count(Id) from Posts").Single());
            }
        }

        [Test]
        public void CanReadMultipleResultSetsFromStoredProcedure()
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

                connection.Execute(@"
                    create procedure GetPost(@postId int) as
                    begin
                        select Id, Content from Posts where Id = @postId;
                        select Id, Text, PostId from Comments where PostId = @postId order by Id;
                    end");

                connection.Execute("insert into Posts(Id, Content) values(@id, @content)", new { id = 1, content = "First post" });
                connection.Execute("insert into Comments(Id, Text, PostId) values(@id, @text, @postId)", new { id = 1, text = "Comment #1", postId = 1 });
                connection.Execute("insert into Comments(Id, Text, PostId) values(@id, @text, @postId)", new { id = 2, text = "Comment #2", postId = 1 });
                connection.Execute("insert into Comments(Id, Text, PostId) values(@id, @text, @postId)", new { id = 3, text = "Comment #3", postId = 1 });
            }

            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                var reader = connection.QueryMultiple("GetPost", new {postId = 1}, commandType: CommandType.StoredProcedure);
                
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