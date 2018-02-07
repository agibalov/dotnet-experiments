using NUnit.Framework;

namespace NUnitExperiment
{
    public class TestCaseSourceDemo
    {
        [Test]
        [TestCaseSource("SumCases")]
        public void CanComputeSum(int a, int b, int result)
        {
            Assert.That(a + b, Is.EqualTo(result));
        }

        static readonly object[] SumCases =
            {
                new object[] {1, 2, 3},
                new object[] {7, 3, 10}
            };
    }
}