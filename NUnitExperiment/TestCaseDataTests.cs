using System.Collections;
using NUnit.Framework;

namespace NUnitExperiment
{
    public class TestCaseDataTests
    {
        [Test]
        [TestCaseSource("SumTestCases")]
        public int Add(int a, int b)
        {
            return a + b;
        }

        public static IEnumerable SumTestCases
        {
            get
            {
                yield return new TestCaseData(1, 2).Returns(3);
                yield return new TestCaseData(2, 3).Returns(5);
                yield return new TestCaseData(3, 10).Returns(13);
            }
        }
    }
}