using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using SQLite;

// http://blog.tpcware.com/2014/05/universal-app-with-sqlite-part-2/

namespace TodoListApp.Tests
{
    [TestClass]
    public class DummyTest
    {
        private const string TestDbFileName = "1.db";
        private SQLiteConnection _connection;

        [TestInitialize]
        public async Task CleanUpDatabase()
        {
            await SqliteHelper.DropDatabaseIfExists(TestDbFileName);
            var databaseFilePath = await SqliteHelper.CreateDatabaseFilePath(TestDbFileName);
            _connection = new SQLiteConnection(databaseFilePath);
        }

        [TestCleanup]
        public void ForgetConnection()
        {
            _connection = null;
        }

        [TestMethod]
        public void DummyScenarioWorks()
        {
            _connection.CreateTable<Note>();
            _connection.Insert(new Note
            {
                Text = "hello"
            });

            Assert.AreEqual(1, _connection.Table<Note>().Count());
        }
    }
}
