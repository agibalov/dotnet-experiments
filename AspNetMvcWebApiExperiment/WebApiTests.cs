using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using NUnit.Framework;

namespace AspNetMvcWebApiExperiment
{
    public abstract class WebApiTests
    {
        private readonly string _defaultControllerName;
        private HttpSelfHostServer _server;
        protected HttpClient Service { get; set; }

        protected WebApiTests(string defaultControllerName)
        {
            _defaultControllerName = defaultControllerName;
        }

        [SetUp]
        public void StartServer()
        {
            var configuration = new HttpSelfHostConfiguration("http://localhost:8081/");
            configuration.Routes.MapHttpRoute("Default", "{action}", new { controller = _defaultControllerName });
            _server = new HttpSelfHostServer(configuration);
            _server.OpenAsync().Wait();

            Service = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:8081/")
                };
        }

        [TearDown]
        public void StopServer()
        {
            _server.CloseAsync().Wait();
            _server.Dispose();
            _server = null;
        }
    }
}