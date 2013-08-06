using NUnit.Framework;
using ServiceStack.ServiceClient.Web;
using ServiceStackWebServicesExperiment.GreetingServiceImpl.DTO;

namespace ServiceStackWebServicesExperiment.Tests
{
    public class GreetingServiceFunctionalTests : WebServiceTests
    {
        private readonly JsonServiceClient _client;

        public GreetingServiceFunctionalTests()
        {
            _client = new JsonServiceClient(ServiceUrl);
        }
        
        [Test]
        public void CanGreetMe()
        {
            var result = _client.Get(new GreetMe {Name = "loki2302"});
            Assert.AreEqual("Hello loki2302", result.Message);
        }

        [Test]
        public void CanUngreetMe()
        {
            var result = _client.Get(new UngreetMe { Name = "loki2302" });
            Assert.AreEqual("Bye loki2302", result.Message);
        }
    }
}