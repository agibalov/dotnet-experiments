using NUnit.Framework;

namespace FakeOExperiment
{
    public class TrivialTests
    {
        [Test]
        public void CanGenerateRandomSentence()
        {
            var sentence = FakeO.Lorem.Sentence();
            Assert.IsTrue(sentence.Split(' ').Length > 0);
        }
    }
}