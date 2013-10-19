using System.Linq;
using Dapper;
using NUnit.Framework;

namespace DapperExperiment.SqlServerTests
{
    public class TsqlSqlServerTest : AbstractSqlServerTest
    {
        [Test]
        public void CanUseArbitraryTsql()
        {
            using (var connection = SqlServerDatabaseHelper.MakeConnection())
            {
                var x = connection.Query<int>(@"
                    declare @a int = 1;
                    declare @b int = 2;
                    declare @c int = @a + @b;
                    select @c").Single();
                Assert.AreEqual(3, x);
            }
        }
    }
}