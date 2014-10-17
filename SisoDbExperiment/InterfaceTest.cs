using System;
using NUnit.Framework;

namespace SisoDbExperiment
{
    public class InterfaceTest : AbstractSisoDbTest
    {
        [Test]
        public void InterfaceSupportSeemsToBeBroken()
        {
            using (var session = Db.BeginSession())
            {
                session.Insert<INote>(new TextNote
                {
                    Title = "Text Note",
                    Text = "I am text note"
                });

                session.Insert<INote>(new ImageNote
                {
                    Title = "Image Note",
                    ImageUrl = "http://image.url"
                });
            }

            var notes = Db.UseOnceTo().Query<INote>().ToList();
            Assert.AreEqual(2, notes.Count);
            Assert.Null(notes[0]);
            Assert.Null(notes[1]);
        }

        public interface INote
        {
            Guid Id { get; set; }
            string Title { get; set; }
        }

        public class TextNote : INote
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
        }

        public class ImageNote : INote
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}