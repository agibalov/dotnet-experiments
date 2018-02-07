using System.Collections.Generic;
using System.Linq;
using Dapper;
using NUnit.Framework;

namespace DapperExperiment.SqlServerTests
{
    public class BulkInsertTest : AbstractSqlServerTest
    {
        [Test]
        public void CanUseBulkInsert()
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
                connection.Open();

                var posts = new List<Post>();
                for (var i = 0; i < 100; ++i)
                {
                    posts.Add(new Post
                        {
                            Content = string.Format("Post #{0}", i + 1)
                        });
                }

                connection.Execute("insert into Posts(Content) values(@Content)", posts);
            }

            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                Assert.AreEqual(100, connection.Query<int>("select count(Id) from Posts").Single());
            }
        }

        public class Post
        {
            public int Id { get; set; }
            public string Content { get; set; }
        }
    }
}