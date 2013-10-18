using System.Configuration;
using NUnit.Framework;

namespace DapperExperiment
{
    public abstract class AbstractSqlCeTest
    {
        protected SqlCeDatabaseHelper SqlCeDatabaseHelper;

        [SetUp]
        public void CreateDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
            SqlCeDatabaseHelper = new SqlCeDatabaseHelper(connectionString);
            SqlCeDatabaseHelper.DropDatabaseIfExistsAndCreateNewOne();
        }   
    }
}