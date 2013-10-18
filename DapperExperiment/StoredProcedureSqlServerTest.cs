using System.Data;
using System.Linq;
using Dapper;
using NUnit.Framework;

namespace DapperExperiment
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
    }
}