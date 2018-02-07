using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace EntityFrameworkExperiment2
{
    public class RenoteServiceTests
    {
        private readonly RenoteService _service = new RenoteService("RenoteConnectionString");

        [SetUp]
        public void ResetEverything()
        {
            _service.Reset();
        }

        [Test]
        public void CanAuthenticate()
        {
            var authentication = _service.Authenticate("loki2302");
            Assert.False(string.IsNullOrEmpty(authentication.SessionToken));
            Assert.AreEqual("loki2302", authentication.UserName);
        }

        [Test]
        public void CanCreateNote()
        {
            var authentication = _service.Authenticate("loki2302");
            var note = _service.CreateNote(authentication.SessionToken, "hello there", new List<string> {"porn", "programming"});
            Assert.AreNotEqual(0, note.NoteId);
            Assert.AreEqual("hello there", note.NoteText);
            Assert.AreEqual(2, note.Tags.Count);
            Assert.IsTrue(note.Tags.Contains("porn"));
            Assert.IsTrue(note.Tags.Contains("programming"));
        }

        [Test]
        public void CanFindNoteByTags()
        {
            var authentication = _service.Authenticate("loki2302");
            var note1 = _service.CreateNote(authentication.SessionToken, "hello there", new List<string> { "porn", "programming" });
            var note2 = _service.CreateNote(authentication.SessionToken, "hello there", new List<string> { "music", "programming" });
            
            var pornProgrammingNotes = _service.FindNotes(authentication.SessionToken, new List<string> { "porn", "programming" });
            Assert.AreEqual(note1.NoteId, pornProgrammingNotes.Single().NoteId);

            var musicNotes = _service.FindNotes(authentication.SessionToken, new List<string> { "music" });
            Assert.AreEqual(note2.NoteId, musicNotes.Single().NoteId);

            var programmingNotes = _service.FindNotes(authentication.SessionToken, new List<string> {"programming"});
            Assert.AreEqual(2, programmingNotes.Count);
            Assert.AreEqual(1, programmingNotes.Count(n => n.NoteId == note1.NoteId));
            Assert.AreEqual(1, programmingNotes.Count(n => n.NoteId == note2.NoteId));
        }

        [Test]
        public void CanGetNote()
        {
            var authentication = _service.Authenticate("loki2302");
            var note = _service.CreateNote(authentication.SessionToken, "hello there", new List<string> { "porn", "programming" });
            var retrievedNote = _service.GetNote(authentication.SessionToken, note.NoteId);
            Assert.AreNotEqual(0, retrievedNote.NoteId);
            Assert.AreEqual("hello there", retrievedNote.NoteText);
            Assert.AreEqual(2, retrievedNote.Tags.Count);
            Assert.IsTrue(retrievedNote.Tags.Contains("porn"));
            Assert.IsTrue(retrievedNote.Tags.Contains("programming"));
        }

        [Test]
        public void CanGetTagNames()
        {
            var authentication = _service.Authenticate("loki2302");
            Assert.AreEqual(0, _service.GetTags(authentication.SessionToken).Count);

            _service.CreateNote(authentication.SessionToken, "hello there", new List<string> { "porn", "programming" });
            _service.CreateNote(authentication.SessionToken, "hello there", new List<string> { "music", "programming" });

            var tagNames = _service.GetTags(authentication.SessionToken);
            Assert.AreEqual(3, tagNames.Count);
            Assert.IsTrue(tagNames.Contains("porn"));
            Assert.IsTrue(tagNames.Contains("music"));
            Assert.IsTrue(tagNames.Contains("programming"));
        }

        [Test]
        public void CanUpdateNote()
        {
            var authentication = _service.Authenticate("loki2302");
            
            var originalNote = _service.CreateNote(authentication.SessionToken, "hello there", new List<string> { "porn", "programming" });
            var tagNames = _service.GetTags(authentication.SessionToken);
            Assert.AreEqual(2, tagNames.Count);
            Assert.IsTrue(tagNames.Contains("porn"));
            Assert.IsTrue(tagNames.Contains("programming"));

            var updatedNote = _service.UpdateNote(authentication.SessionToken, originalNote.NoteId, "qwerty", new List<string> { "porn", "music" });
            Assert.AreEqual(originalNote.NoteId, updatedNote.NoteId);
            Assert.AreEqual("qwerty", updatedNote.NoteText);
            Assert.AreEqual(2, updatedNote.Tags.Count);
            Assert.IsTrue(updatedNote.Tags.Contains("porn"));
            Assert.IsTrue(updatedNote.Tags.Contains("music"));

            tagNames = _service.GetTags(authentication.SessionToken);
            Assert.AreEqual(2, tagNames.Count);
            Assert.IsTrue(tagNames.Contains("porn"));
            Assert.IsTrue(tagNames.Contains("music"));
        }

        [Test]
        public void CanDeleteNote()
        {
            var authentication = _service.Authenticate("loki2302");
            var note = _service.CreateNote(authentication.SessionToken, "hello there", new List<string> { "porn", "programming" });
            Assert.AreEqual(2, _service.GetTags(authentication.SessionToken).Count);
            _service.DeleteNote(authentication.SessionToken, note.NoteId);
            Assert.AreEqual(0, _service.GetTags(authentication.SessionToken).Count);
        }
    }
}