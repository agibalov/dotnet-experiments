using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;

namespace AspNetMvcWebApiTestingExperiment
{
    [TestFixture]
    public class HttpServerTest
    {
        private HttpServer _server;

        [SetUp]
        public void StartServer()
        {
            var configuration = new HttpConfiguration();
            configuration.Routes.MapHttpRoute("Default", "{action}", new { controller = "CalculatorWebApi" });
            _server = new HttpServer(configuration);
        }

        [TearDown]
        public void StopServer()
        {
            _server.Dispose();
            _server = null;
        }

        [Test]
        public void OneAndTwoIsThree()
        {
            var response = new HttpClient(_server)
                .GetAsync("http://CanBeAnything/AddNumbers/?a=1&b=2")
                .Result
                .Content
                .ReadAsAsync<AddNumbersResult>()
                .Result;

            Assert.AreEqual(1, response.NumberA);
            Assert.AreEqual(2, response.NumberB);
            Assert.AreEqual(3, response.Result);
        }

        [Test]
        public void TwoAndThreeIsFive()
        {
            var response = new HttpClient(_server)
                .GetAsync("http://CanBeAnything/AddNumbers/?a=2&b=3")
                .Result
                .Content
                .ReadAsAsync<AddNumbersResult>()
                .Result;

            Assert.AreEqual(2, response.NumberA);
            Assert.AreEqual(3, response.NumberB);
            Assert.AreEqual(5, response.Result);
        }
    }
}