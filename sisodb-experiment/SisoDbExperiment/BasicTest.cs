using System;
using System.Linq;
using NUnit.Framework;

namespace SisoDbExperiment
{
    public class BasicTest : AbstractSisoDbTest
    {
        [Test]
        public void ThereAreNoNotesByDefault()
        {
            var notes = Db.UseOnceTo().Query<Note>().ToList();
            Assert.AreEqual(0, notes.Count);
        }

        [Test]
        public void CanSaveAndRetrieveSingleNote()
        {
            Db.UseOnceTo().Insert(new Note { Text = "loki2302" });

            var notes = Db.UseOnceTo().Query<Note>().ToList();
            Assert.AreEqual(1, notes.Count);

            var singleNote = notes.Single();
            Assert.NotNull(singleNote.Id);
            Assert.AreEqual("loki2302", singleNote.Text);
        }

        [Test]
        public void CanSaveAndRetrieveMultipleNotes()
        {
            Db.UseOnceTo().InsertMany(new[]
            {
                new Note { Text = "Note One"},
                new Note { Text = "Note Two"}
            });

            var noteCount = Db.UseOnceTo().Query<Note>().Count();
            Assert.AreEqual(2, noteCount);
        }

        [Test]
        public void CanUpdateNote()
        {
            Db.UseOnceTo().Insert(new Note { Text = "loki2302" });
            
            var note = Db.UseOnceTo().Query<Note>().Single();
            note.Text = "hi there";
            Db.UseOnceTo().Update(note);

            note = Db.UseOnceTo().Query<Note>().Single();
            Assert.AreEqual("hi there", note.Text);
        }

        [Test]
        public void CanDeleteNote()
        {
            Db.UseOnceTo().Insert(new Note { Text = "loki2302" });

            var note = Db.UseOnceTo().Query<Note>().Single();
            Db.UseOnceTo().DeleteById<Note>(note.Id);

            var noteCount = Db.UseOnceTo().Query<Note>().Count();
            Assert.AreEqual(0, noteCount);
        }

        [Test]
        public void CanUseTransactions()
        {
            using (var session = Db.BeginSession())
            {
                session.Insert(new Note {Text = "This will not be saved"});
                
                var sessionNoteCount = session.Query<Note>().Count();
                Assert.AreEqual(1, sessionNoteCount);

                session.Abort();
            }

            var noteCount = Db.UseOnceTo().Query<Note>().Count();
            Assert.AreEqual(0, noteCount);
        }

        public class Note
        {
            public Guid Id { get; set; }
            public string Text { get; set; }
        }
    }
}