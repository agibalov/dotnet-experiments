using Ninject;
using Ninject.Extensions.ChildKernel;
using NUnit.Framework;

namespace NinjectExtensionsChildKernelTests
{
    public class ChildKernelTests
    {
        [Test]
        public void CanUseChildKernel()
        {
            var kernel = new StandardKernel();

            var requestAKernel = new ChildKernel(kernel);
            requestAKernel.Bind<Request>().ToConstant(new Request { RequestId = 1 });
            Assert.AreEqual(1, requestAKernel.Get<RequestProcessor>().Request.RequestId);

            var requestBKernel = new ChildKernel(kernel);
            requestBKernel.Bind<Request>().ToConstant(new Request { RequestId = 2 });
            Assert.AreEqual(2, requestBKernel.Get<RequestProcessor>().Request.RequestId);

            Assert.AreEqual(1, requestAKernel.Get<RequestProcessor>().Request.RequestId);
            Assert.AreEqual(2, requestBKernel.Get<RequestProcessor>().Request.RequestId);
        }

        public class RequestProcessor
        {
            [Inject]
            public Request Request { get; set; }
        }

        public class Request
        {
            public int RequestId { get; set; }
        }
    }
}
