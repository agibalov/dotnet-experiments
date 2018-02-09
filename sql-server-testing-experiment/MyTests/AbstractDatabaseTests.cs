using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using NUnit.Framework;

namespace MyTests
{
    public abstract class AbstractDatabaseTests
    {
        private static bool _databaseIsDeployed;
        private TransactionScope _transactionScope;
        private readonly string _connectionString;
        private readonly DropCreateSqlServerDatabaseService _dropCreateSqlServerDatabaseService;
        private readonly DatabaseProjectDeploymentService _databaseProjectDeploymentService;

        protected AbstractDatabaseTests()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            _dropCreateSqlServerDatabaseService = new DropCreateSqlServerDatabaseService(_connectionString);

            var sqlProjFilePath = ConfigurationManager.AppSettings["SqlProjFilePath"];
            var publishProfileFilePath = ConfigurationManager.AppSettings["PublishProfileFilePath"];
            _databaseProjectDeploymentService = new DatabaseProjectDeploymentService(sqlProjFilePath, publishProfileFilePath);
        }

        [SetUp]
        public void BeginTransactionAndDeployDatabaseIfNecessary()
        {
            if (!_databaseIsDeployed)
            {
                _dropCreateSqlServerDatabaseService.DropDatabaseIfExistsAndCreateANewOne();
                _databaseProjectDeploymentService.DeployDatabase();

                _databaseIsDeployed = true;
            }

            _transactionScope = new TransactionScope();
        }

        [TearDown]
        public void RevertTransaction()
        {
            _transactionScope.Dispose();
        }

        protected IDbConnection MakeConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}