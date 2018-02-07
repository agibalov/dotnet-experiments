using NUnit.Framework;

namespace NUnitExperiment
{
    public class SequentialTests
    {
        [Test]
        [Sequential]
        public void CanComputeSum(
            [Values(1, 3, 0)] int a,
            [Values(2, 5, -1)] int b,
            [Values(3, 8, -1)] int result)
        {
            Assert.That(a + b, Is.EqualTo(result));
        }
    }
}