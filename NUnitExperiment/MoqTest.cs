using Moq;
using NUnit.Framework;

namespace NUnitExperiment
{
    public class MoqTest
    {
        [Test]
        public void Dummy()
        {
            var mock = new Mock<ICalculator>();
            mock.Setup(c => c.Add(2, 3)).Returns(5);

            var calculator = mock.Object;
            Assert.AreEqual(5, calculator.Add(2, 3));
        }

        public interface ICalculator
        {
            int Add(int a, int b);
        }
    }
}