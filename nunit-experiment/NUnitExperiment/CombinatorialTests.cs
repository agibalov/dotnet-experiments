using NUnit.Framework;

namespace NUnitExperiment
{
    public class CombinatorialTests
    {
        [Test]
        [Combinatorial]
        public void CanComputeSum(
            [Values(1, 2, 3, 4)] int a,
            [Values(1, 2, 3, 4)] int b)
        {
            Assert.That(a + b, Is.GreaterThanOrEqualTo(2));
        }
    }
}
