using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DapperExperiment
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
                    connection.Execute("drop database " + databaseName);
                }

                connection.Execute("create database " + databaseName);
            }
        }

        public IDbConnection MakeConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}