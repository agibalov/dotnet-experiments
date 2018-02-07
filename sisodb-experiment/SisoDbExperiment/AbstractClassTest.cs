using System;
using NUnit.Framework;

namespace SisoDbExperiment
{
    public class AbstractClassTest : AbstractSisoDbTest
    {
        [Test]
        public void AbstractClassSupportSeemsToBeBroken()
        {
            using (var session = Db.BeginSession())
            {
                session.Insert<Note>(new TextNote
                {
                    Title = "Text Note", 
                    Text = "I am text note"
                });

                session.Insert<Note>(new ImageNote
                {
                    Title = "Image Note",
                    ImageUrl = "http://image.url"
                });
            }

            var notes = Db.UseOnceTo().Query<Note>().ToList();
            Assert.AreEqual(2, notes.Count);
            Assert.Null(notes[0]);
            Assert.Null(notes[1]);
        }

        public abstract class Note
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
        }

        public class TextNote : Note
        {
            public string Text { get; set; }
        }

        public class ImageNote : Note
        {
            public string ImageUrl { get; set; }
        }
    }
}