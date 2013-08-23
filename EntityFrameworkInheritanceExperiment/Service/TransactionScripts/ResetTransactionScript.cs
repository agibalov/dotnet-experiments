using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using Ninject;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class ResetTransactionScript
    {
        private readonly string _connectionStringName;

        public ResetTransactionScript([Named("ConnectionStringName")] string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public void Reset()
        {
            var connectionStringBuilder = new SqlCeConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString);
            var databaseFileName = connectionStringBuilder.DataSource;
            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            using (var context = new UsersContext(_connectionStringName))
            {
                context.Database.Create();
            }
        }
    }
}