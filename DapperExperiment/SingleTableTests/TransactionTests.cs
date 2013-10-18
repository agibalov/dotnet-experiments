using System.Linq;
using System.Transactions;
using Dapper;
using NUnit.Framework;

namespace DapperExperiment.SingleTableTests
{
    public class TransactionTests : AbstractSqlCeTest
    {
        [SetUp]
        public void InitializeSchema()
        {
            using (var connection = SqlCeDatabaseHelper.MakeConnection())
            {
                connection.Execute(
                    "create table Users(" + 
                    "UserId int not null identity(1, 1) primary key, " + 
                    "UserName nvarchar(256) not null)");
            }
        }

        [Test]
        public void NoDataIsCommitedToDatabaseIfTransactionIsNotSuccessful()
        {
            using (var connection = SqlCeDatabaseHelper.MakeConnection())
            {
                using (var transactionScope = new TransactionScope())
                {
                    connection.Execute("insert into Users(UserName) values(@userName)", new {userName = "loki2302"});
                    // intentionally no .Complete() call here
                }

                var userCount = connection.Query<int>("select count(*) from Users").Single();
                Assert.AreEqual(0, userCount);
            }
        }

        [Test]
        public void DataIsCommitedToDatabaseIfTransactionIsSuccessful()
        {
            using (var connection = SqlCeDatabaseHelper.MakeConnection())
            {
                using (var transactionScope = new TransactionScope())
                {
                    connection.Execute("insert into Users(UserName) values(@userName)", new { userName = "loki2302" });
                    transactionScope.Complete();
                }

                var userCount = connection.Query<int>("select count(*) from Users").Single();
                Assert.AreEqual(1, userCount);
            }
        }
    }
}