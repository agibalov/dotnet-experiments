using NUnit.Framework;
using ServiceStack.ServiceClient.Web;
using ServiceStackWebServicesExperiment.CalculatorServiceImpl.DTO;

namespace ServiceStackWebServicesExperiment.Tests
{
    public class CalculatorServiceFunctionalTests : WebServiceTests
    {
        private readonly JsonServiceClient _client;

        public CalculatorServiceFunctionalTests()
        {
            _client = new JsonServiceClient(ServiceUrl);
        }

        [Test]
        public void CanAddNumbers()
        {
            var result = _client.Get(new AddNumbers
                {
                    A = 1,
                    B = 2
                });

            Assert.AreEqual(1, result.A);
            Assert.AreEqual(2, result.B);
            Assert.AreEqual(3, result.Result);
        }

        [Test]
        public void CanSubNumbers()
        {
            var result = _client.Get(new SubNumbers
                {
                    A = 1,
                    B = 2
                });

            Assert.AreEqual(1, result.A);
            Assert.AreEqual(2, result.B);
            Assert.AreEqual(-1, result.Result);
        }
    }
}