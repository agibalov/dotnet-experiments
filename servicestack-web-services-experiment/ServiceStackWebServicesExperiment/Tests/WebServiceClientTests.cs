using System;
using NUnit.Framework;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;
using ServiceStackWebServicesExperiment.GreetingServiceImpl.DTO;

namespace ServiceStackWebServicesExperiment.Tests
{
    public class WebServiceClientTests : WebServiceTests
    {
        [Test]
        public void CanUseExtensionMethodsForRequest()
        {
            var greetMeResponseDto = string.Format("{0}/hello/?Name=loki2302", ServiceUrl)
                .GetJsonFromUrl()
                .FromJson<GreetMeResponseDTO>();

            Assert.AreEqual("Hello loki2302", greetMeResponseDto.Message);
        }

        [Test]
        public void CanUseJsonServiceClientWithExplicitUrlForRequest()
        {
            var restClient = new JsonServiceClient(ServiceUrl);
            var greetMeResponseDto = restClient.Get<GreetMeResponseDTO>("/hello/?Name={0}".Fmt(Uri.EscapeDataString("loki2302")));
            Assert.AreEqual("Hello loki2302", greetMeResponseDto.Message);
        }

        [Test]
        public void CanUseJsonServiceClientWithExplicitRequestObject()
        {
            var restClient = new JsonServiceClient(ServiceUrl);
            var greetMeResponseDto = restClient.Get(new GreetMe { Name = "loki2302" });
            Assert.AreEqual("Hello loki2302", greetMeResponseDto.Message);
        }
    }
}