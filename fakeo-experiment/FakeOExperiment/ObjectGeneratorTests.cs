using NUnit.Framework;

namespace FakeOExperiment
{
    public class ObjectGeneratorTests
    {
        [Test]
        public void CanGenerateRandomObject()
        {
            var fakePost = FakeO.Create.Fake<Post>(
                p => p.Title = FakeO.Lorem.Sentence(),
                p => p.Text = string.Join("\n\n", FakeO.Lorem.Paragraphs(2)));

            Assert.IsFalse(string.IsNullOrEmpty(fakePost.Title));
            Assert.IsFalse(string.IsNullOrEmpty(fakePost.Text));
        }

        [Test]
        public void CanRememberHowToGenerateRandomObject()
        {
            var faker = new FakeO.FakeCreator();
            faker.Remember<Post>(
                p => p.Title = FakeO.Lorem.Sentence(),
                p => p.Text = string.Join("\n\n", FakeO.Lorem.Paragraphs(2)));

            var post1 = faker.Fake<Post>();
            Assert.IsFalse(string.IsNullOrEmpty(post1.Title));
            Assert.IsFalse(string.IsNullOrEmpty(post1.Text));

            var post2 = faker.Fake<Post>();
            Assert.IsFalse(string.IsNullOrEmpty(post2.Title));
            Assert.IsFalse(string.IsNullOrEmpty(post2.Text));
        }
    }
}