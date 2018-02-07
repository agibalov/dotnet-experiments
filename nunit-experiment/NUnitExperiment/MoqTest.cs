using Moq;
using NUnit.Framework;

namespace NUnitExperiment
{
    public class MoqTest
    {
        [Test]
        public void CanHaveACalculatorMock()
        {
            var calculatorMock = new Mock<ICalculator>();
            calculatorMock.Setup(c => c.Add(2, 3)).Returns(5);

            var calculator = calculatorMock.Object;
            Assert.AreEqual(5, calculator.Add(2, 3));

            calculatorMock.Verify(c => c.Add(2, 3), Times.Once);
        }

        [Test]
        public void WhenUsingLooseBehaviorCallingMethodsWithoutSetupDoesNotFail()
        {
            var calculatorMock = new Mock<ICalculator>(MockBehavior.Loose);
            var calculator = calculatorMock.Object;
            
            var result = calculator.Add(2, 3);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void WhenUsingStrictBehaviorCallingMethodsWithoutSetupFails()
        {
            var calculatorMock = new Mock<ICalculator>(MockBehavior.Strict);
            var calculator = calculatorMock.Object;

            try
            {
                calculator.Add(2, 3);
                Assert.Fail();
            }
            catch (MockException)
            {
            }
        }

        public interface ICalculator
        {
            int Add(int a, int b);
        }
    }
}