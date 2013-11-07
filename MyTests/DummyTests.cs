using System.Linq;
using Dapper;
using NUnit.Framework;

namespace MyTests
{
    public class DummyTests : AbstractDatabaseTests
    {
        [Test]
        public void ThereAreNoNotesByDefault()
        {
            using (var connection = MakeConnection())
            {
                var noteCount = connection.Query<int>(@"select count(Id) from Notes").Single();
                Assert.AreEqual(0, noteCount);
            }
        }

        [Test]
        public void CanInsertAndGetNote()
        {
            using (var connection = MakeConnection())
            {
                connection.Execute(@"
                    insert into Notes(NoteText)
                    values(@noteText)",
                    new { noteText = "hello" });

                var noteText = connection.Query<string>(@"select top 1 NoteText from Notes").Single();
                Assert.AreEqual("hello", noteText);
            }
        }
    }
}
