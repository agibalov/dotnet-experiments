using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DapperExperiment.SqlServerTests
{
    public class SqlServerDatabaseHelper
    {
        private readonly string _connectionString;

        public SqlServerDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DropDatabaseIfExistsAndCreateNewOne()
        {
            var masterConnectionStringBuilder = new SqlConnectionStringBuilder(_connectionString);
            var databaseName = masterConnectionStringBuilder.InitialCatalog;
            masterConnectionStringBuilder.InitialCatalog = "master";
            var masterConnectionString = masterConnectionStringBuilder.ConnectionString;

            using (var connection = new SqlConnection(masterConnectionString))
            {
                var databaseCount = connection.Query<int>("select count(db_id(@databaseName))", new { databaseName }).Single();
                if (databaseCount > 0)
                {
                    connection.Execute(string.Format("alter database {0} set single_user with rollback immediate", databaseName));
                    connection.Execute(string.Format("drop database {0}", databaseName));
                }

                connection.Execute(string.Format("create database {0}", databaseName));
            }
        }

        public IDbConnection MakeConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}