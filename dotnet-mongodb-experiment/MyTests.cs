using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using dotnet_mongodb_experiment.Entities;

namespace dotnet_mongodb_experiment
{
    public class MyTests
    {
        private readonly RenoteService _service;

        public MyTests()
        {
            var client = new MongoClient("mongodb://win7dev-home");
            var server = client.GetServer();
            var database = server.GetDatabase("dotnet");

            var userCollection = database.GetCollection<User>("users");
            var sessionCollection = database.GetCollection<Session>("sessions");
            var tagCollection = database.GetCollection<Tag>("tags");
            var noteCollection = database.GetCollection<Note>("notes");
            _service = new RenoteService(
                userCollection,
                sessionCollection,
                tagCollection,
                noteCollection);
        }

        [SetUp]
        public void CleanUpEverything()
        {
            _service.RemoveEverything();
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
            var authenticatation = _service.Authenticate("loki2302");
            var sessionToken = authenticatation.SessionToken;
            var note = _service.CreateNote(
                sessionToken, 
                "hello there", 
                new List<string> { "porn", "programming" });
            Assert.False(string.IsNullOrEmpty(note.NoteId));
            Assert.AreEqual("hello there", note.Text);
            Assert.False(string.IsNullOrEmpty(note.User.UserId));
            Assert.AreEqual("loki2302", note.User.UserName);
            Assert.AreEqual(2, note.Tags.Count);
            Assert.AreEqual(1, note.Tags.Count(t => t.TagName == "porn"));
            Assert.AreEqual(1, note.Tags.Count(t => t.TagName == "programming"));
        }

        [Test]
        public void CanGetNote()
        {
            var authenticatation = _service.Authenticate("loki2302");
            var sessionToken = authenticatation.SessionToken;
            var note = _service.CreateNote(
                sessionToken,
                "hello there",
                new List<string> { "porn", "programming" });

            var retrievedNote = _service.GetNote(sessionToken, note.NoteId);
            Assert.False(string.IsNullOrEmpty(retrievedNote.NoteId));
            Assert.AreEqual("hello there", retrievedNote.Text);
            Assert.False(string.IsNullOrEmpty(retrievedNote.User.UserId));
            Assert.AreEqual("loki2302", retrievedNote.User.UserName);
            Assert.AreEqual(2, retrievedNote.Tags.Count);
            Assert.AreEqual(1, retrievedNote.Tags.Count(t => t.TagName == "porn"));
            Assert.AreEqual(1, retrievedNote.Tags.Count(t => t.TagName == "programming"));
        }

        [Test]
        public void CanGetAllNotes()
        {
            var authenticatation = _service.Authenticate("loki2302");
            var sessionToken = authenticatation.SessionToken;
            var note1 = _service.CreateNote(
                sessionToken,
                "hello there",
                new List<string> { "porn", "programming" });
            var note2 = _service.CreateNote(
                sessionToken,
                "hello there 2",
                new List<string> { "music", "fun" });

            var notes = _service.GetAllNotes(sessionToken);
            Assert.AreEqual(2, notes.Count);
            Assert.AreEqual(1, notes.Count(n => n.NoteId == note1.NoteId));
            Assert.AreEqual(1, notes.Count(n => n.NoteId == note2.NoteId));
        }
    }
}