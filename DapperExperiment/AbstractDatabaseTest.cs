using System.Configuration;
using NUnit.Framework;

namespace DapperExperiment
{
    public abstract class AbstractDatabaseTest
    {
        protected DatabaseHelper DatabaseHelper;

        [SetUp]
        public void CreateDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
            DatabaseHelper = new DatabaseHelper(connectionString);
            DatabaseHelper.DropDatabaseIfExistsAndCreateNewOne();
        }   
    }
}