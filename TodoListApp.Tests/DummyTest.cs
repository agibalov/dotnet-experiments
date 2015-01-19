using System.Linq;
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

            _connection.CreateTable<Note>();
        }

        [TestCleanup]
        public void ForgetConnection()
        {
            _connection.Dispose();
            _connection = null;
        }

        [TestMethod]
        public void ThereAreNoNotesByDefault()
        {
            Assert.AreEqual(0, _connection.Table<Note>().Count());
        }

        [TestMethod]
        public void WhenTheNoteIsInsertedThereIsOneNote()
        {
            var note = new Note
            {
                Text = "hello"
            };
            _connection.Insert(note);

            Assert.AreEqual(1, _connection.Table<Note>().Count());
        }

        [TestMethod]
        public void ItIsPossibleToDeleteANote()
        {
            var note = new Note
            {
                Text = "hello"
            };
            _connection.Insert(note);

            _connection.Delete<Note>(note.Id);

            Assert.AreEqual(0, _connection.Table<Note>().Count());
        }

        [TestMethod]
        public void ItShouldBePossibleToSaveMultipleNotes()
        {
            var note1 = new Note { Text = "Note One" };
            _connection.Insert(note1);

            var note2 = new Note { Text = "Note Two" };
            _connection.Insert(note2);

            Assert.AreEqual(2, _connection.Table<Note>().Count());
            Assert.AreEqual(2, new[] { note1.Id, note2.Id }.Distinct().Count());
        }

        [TestMethod]
        public void ItShouldBePossibleToFindANoteById()
        {
            var note1 = new Note { Text = "Note One" };
            _connection.Insert(note1);

            var note2 = new Note { Text = "Note Two" };
            _connection.Insert(note2);

            var retrievedNote2 = _connection.Find<Note>(n => n.Id == note2.Id);
            Assert.AreEqual(note2.Id, retrievedNote2.Id);
            Assert.AreEqual(note2.Text, retrievedNote2.Text);
        }

        [TestMethod]
        public void ItShouldPossibleToGetAnOrderedListOfNotes()
        {
            var noteA = new Note { Text = "Note A" };
            _connection.Insert(noteA);

            var noteB = new Note { Text = "Note B" };
            _connection.Insert(noteB);

            var abNotes = _connection.Table<Note>().OrderBy(n => n.Text).ToList();
            Assert.AreEqual(noteA.Id, abNotes[0].Id);
            Assert.AreEqual(noteB.Id, abNotes[1].Id);

            var baNotes = _connection.Table<Note>().OrderByDescending(n => n.Text).ToList();
            Assert.AreEqual(noteB.Id, baNotes[0].Id);
            Assert.AreEqual(noteA.Id, baNotes[1].Id);
        }
    }
}
