using System.Configuration;
using NUnit.Framework;

namespace DapperExperiment.SqlServerTests
{
    public abstract class AbstractSqlServerTest
    {
        protected SqlServerDatabaseHelper SqlServerDatabaseHelper;

        [SetUp]
        public void CreateDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SqlServerConnectionString"].ConnectionString;
            SqlServerDatabaseHelper = new SqlServerDatabaseHelper(connectionString);
            SqlServerDatabaseHelper.DropDatabaseIfExistsAndCreateNewOne();
        }
    }
}