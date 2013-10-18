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

    public class RecursiveCommonTableExpressionsTest : AbstractSqlServerTest
    {
        [Test]
        public void CanUserRecursiveCte()
        {
            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                connection.Execute(@"
                    create table Categories(
	                    Id int primary key not null,
	                    Name nvarchar(256) not null,
	                    ParentCategoryId int foreign key references Categories(Id))");

                connection.Execute(@"
                    insert into Categories(Id, Name, ParentCategoryId) values(@id, @name, @parentId)",
                    new { id = 1, name = "Me", parentId = (int?)null });

                connection.Execute(@"
                    insert into Categories(Id, Name, ParentCategoryId) values(@id, @name, @parentId)",
                    new { id = 2, name = "Programming", parentId = 1 });

                connection.Execute(@"
                    insert into Categories(Id, Name, ParentCategoryId) values(@id, @name, @parentId)",
                    new { id = 3, name = "Java", parentId = 2 });

                connection.Execute(@"
                    insert into Categories(Id, Name, ParentCategoryId) values(@id, @name, @parentId)",
                    new { id = 4, name = "C++", parentId = 2 });
            }

            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                var levels = connection.Query(@"
                    with cte(Level, Id, Name, ParentCategoryId) as (
	                    select 0, Id, Name, ParentCategoryId from Categories where Id = @id
	                    union all 
	                    select cte.Level + 1, C.Id, C.Name, C.ParentCategoryId from Categories as C
	                    join cte on C.Id = cte.ParentCategoryId
                    )
                    select Level, Id, Name, ParentCategoryId from cte order by Level desc;",
                    new { id = 4 }).ToList();

                Assert.AreEqual(2, levels[0].Level);
                Assert.AreEqual(1, levels[0].Id);
                Assert.AreEqual("Me", levels[0].Name);
                Assert.AreEqual(null, levels[0].ParentCategoryId);

                Assert.AreEqual(1, levels[1].Level);
                Assert.AreEqual(2, levels[1].Id);
                Assert.AreEqual("Programming", levels[1].Name);
                Assert.AreEqual(1, levels[1].ParentCategoryId);

                Assert.AreEqual(0, levels[2].Level);
                Assert.AreEqual(4, levels[2].Id);
                Assert.AreEqual("C++", levels[2].Name);
                Assert.AreEqual(2, levels[2].ParentCategoryId);
            }
        }
    }
}