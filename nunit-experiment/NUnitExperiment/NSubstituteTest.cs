using NSubstitute;
using NUnit.Framework;

namespace NUnitExperiment
{
    public class NSubstituteTest
    {
        [Test]
        public void Dummy()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(2, 3).Returns(5);

            Assert.AreEqual(5, calculator.Add(2, 3));
        }

        public interface ICalculator
        {
            int Add(int a, int b);
        }
    }
}