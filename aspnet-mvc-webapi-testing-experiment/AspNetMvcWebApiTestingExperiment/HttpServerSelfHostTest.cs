using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using NUnit.Framework;

namespace AspNetMvcWebApiTestingExperiment
{
    [TestFixture]
    public class HttpServerSelfHostTest
    {
        private HttpSelfHostServer _server;

        [SetUp]
        public void StartServer()
        {
            var configuration = new HttpSelfHostConfiguration("http://localhost:8081/");
            configuration.Routes.MapHttpRoute("Default", "{action}", new { controller = "CalculatorWebApi" });
            _server = new HttpSelfHostServer(configuration);
            _server.OpenAsync().Wait();
        }

        [TearDown]
        public void StopServer()
        {
            _server.CloseAsync().Wait();
            _server.Dispose();
            _server = null;
        }

        [Test]
        public void OneAndTwoIsThree()
        {
            var response = new HttpClient()
                .GetAsync("http://localhost:8081/AddNumbers/?a=1&b=2")
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
            var response = new HttpClient()
                .GetAsync("http://localhost:8081/AddNumbers/?a=2&b=3")
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
