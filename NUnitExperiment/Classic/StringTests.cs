using NUnit.Framework;
using NUnitExperiment.ConstraintBased;

namespace NUnitExperiment.Classic
{
    public class StringTests : IStringTests
    {
        [Test]
        public void CanCheckIfStringsAreEqual()
        {
            Assert.AreEqual("qwerty", "qwerty");
        }

        [Test]
        public void CanCheckIfStringsAreNotEqual()
        {
            Assert.AreNotEqual("qwerty", "asdfgh");
        }

        [Test]
        public void CanCheckIfStringHasASubstring()
        {
            StringAssert.Contains("wer", "qwerty");
        }

        [Test]
        public void CanCheckIfStringHasNoSubstring()
        {
            StringAssert.DoesNotContain("wer", "asdfgh");
        }

        [Test]
        public void CanCheckIfStringStartsWith()
        {
            StringAssert.StartsWith("qwe", "qwerty");
        }

        [Test]
        public void CanCheckIfStringDoesNotStartWith()
        {
            StringAssert.DoesNotStartWith("asd", "qwerty");
        }

        [Test]
        public void CanCheckIfStringEndsWith()
        {
            StringAssert.EndsWith("rty", "qwerty");
        }

        [Test]
        public void CanCheckIfStringDoesNotEndWith()
        {
            StringAssert.DoesNotEndWith("rty", "asdfgh");
        }

        [Test]
        public void CanCheckIfStringsAreEqualIgnoringCase()
        {
            StringAssert.AreEqualIgnoringCase("qwerty", "QwErTy");
        }

        [Test]
        public void CanCheckIfStringMatches()
        {
            StringAssert.IsMatch(@"\$\d+", "$100");
        }

        [Test]
        public void CanCheckIfStringDoesNotMatch()
        {
            StringAssert.DoesNotMatch(@"\$\d+", "hello");
        }
    }
}
