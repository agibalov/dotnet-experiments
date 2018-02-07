using System.Data.SqlServerCe;
using System.IO;
using NUnit.Framework;
using SisoDb;
using SisoDb.SqlCe4;

namespace SisoDbExperiment
{
    public abstract class AbstractSisoDbTest
    {
        private const string DatabaseFileName = "test.sdf";
        protected ISisoDatabase Db;

        [SetUp]
        public void InitDatabase()
        {
            if (File.Exists(DatabaseFileName))
            {
                File.Delete(DatabaseFileName);
            }

            var connectionString = new SqlCeConnectionStringBuilder
            {
                {"Data Source", DatabaseFileName}
            }.ConnectionString;

            var dbFactory = new SqlCe4DbFactory();
            var db = dbFactory.CreateDatabase(new SqlCe4ConnectionInfo(connectionString));
            if (!db.Exists())
            {
                db.EnsureNewDatabase();
            }

            Db = db;
        }

        [TearDown]
        public void DeleteDatabase()
        {
            File.Delete(DatabaseFileName);
        }
    }
}