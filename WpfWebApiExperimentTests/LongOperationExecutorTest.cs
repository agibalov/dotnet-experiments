using Moq;
using NUnit.Framework;
using WpfWebApiExperiment.ViewModels;

namespace WpfWebApiExperimentTests
{
    // TODO: check if OnOperationFinished only gets called after the task is done

    public class LongOperationExecutorTest
    {
        [Test]
        public async void LongOperationExecutorWorksAtAll()
        {
            var longOperationListener = new Mock<ILongOperationListener>(MockBehavior.Strict);
            longOperationListener.Setup(l => l.OnOperationStarted());
            longOperationListener.Setup(l => l.OnOperationFinished());

            var longOperationExecutor = new LongOperationExecutor(longOperationListener.Object);
            var result = await longOperationExecutor.Execute(() => 123);
            Assert.AreEqual(123, result);

            longOperationListener.Verify(l => l.OnOperationStarted(), Times.Once);
            longOperationListener.Verify(l => l.OnOperationFinished(), Times.Once);
        }
    }
}
