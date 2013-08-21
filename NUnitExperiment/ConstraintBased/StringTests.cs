using NUnit.Framework;

namespace NUnitExperiment.ConstraintBased
{
    public class StringTests : IStringTests
    {
        [Test]
        public void CanCheckIfStringsAreEqual()
        {
            Assert.That("qwerty", Is.EqualTo("qwerty"));
        }

        [Test]
        public void CanCheckIfStringsAreNotEqual()
        {
            Assert.That("qwerty", Is.Not.EqualTo("asdfgh"));
        }

        [Test]
        public void CanCheckIfStringHasASubstring()
        {
            Assert.That("qwerty", Is.StringContaining("wer"));
            Assert.That("qwerty", Contains.Substring("wer"));
        }

        [Test]
        public void CanCheckIfStringHasNoSubstring()
        {
            Assert.That("asdfgh", Is.Not.StringContaining("wer"));
        }

        [Test]
        public void CanCheckIfStringStartsWith()
        {
            Assert.That("qwerty", Is.StringStarting("qwe"));
        }

        [Test]
        public void CanCheckIfStringDoesNotStartWith()
        {
            Assert.That("qwerty", Is.Not.StringStarting("asd"));
        }

        [Test]
        public void CanCheckIfStringEndsWith()
        {
            Assert.That("qwerty", Is.StringEnding("rty"));
        }

        [Test]
        public void CanCheckIfStringDoesNotEndWith()
        {
            Assert.That("asdfgh", Is.Not.StringEnding("rty"));
        }

        [Test]
        public void CanCheckIfStringsAreEqualIgnoringCase()
        {
            Assert.That("qwerty", Is.EqualTo("QwErTy").IgnoreCase);
        }

        [Test]
        public void CanCheckIfStringMatches()
        {
            Assert.That("$100", Is.StringMatching(@"\$\d+"));
        }

        [Test]
        public void CanCheckIfStringDoesNotMatch()
        {
            Assert.That("hello", Is.Not.StringMatching(@"\$\d+"));
        }
    }
}
