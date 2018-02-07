using System.Data;
using System.Data.SqlServerCe;
using System.IO;

namespace DapperExperiment
{
    public class SqlCeDatabaseHelper
    {
        private readonly string _connectionString;

        public SqlCeDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DropDatabaseIfExistsAndCreateNewOne()
        {
            var builder = new SqlCeConnectionStringBuilder(_connectionString);
            var databaseFileName = builder.DataSource;

            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            using (var engine = new SqlCeEngine(_connectionString))
            {
                engine.CreateDatabase();
            }
        }

        public IDbConnection MakeConnection()
        {
            return new SqlCeConnection(_connectionString);
        }
    }
}