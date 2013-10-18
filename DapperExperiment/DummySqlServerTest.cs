using System.Linq;
using Dapper;
using NUnit.Framework;

namespace DapperExperiment
{
    public class DummySqlServerTest : AbstractSqlServerTest
    {
        [Test]
        public void Dummy()
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
                connection.Execute("insert into Posts(Content) values(@content)", new {content = "hello there"});
                var content = connection.Query<string>("select Content from Posts").Single();
                Assert.AreEqual("hello there", content);
            }
        }
    }
}