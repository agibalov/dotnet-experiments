using System;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Owin;

namespace AspNetWebApi2Experiment
{
    public abstract class AbstractWebApiTest
    {
        protected const string BaseAddress = "http://localhost:8080/";
        
        private IDisposable _webApiContext;

        [SetUp]
        public void StartWebApi()
        {
            _webApiContext = WebApp.Start(BaseAddress, SetUpWebApi);
        }

        [TearDown]
        public void StopWebApi()
        {
            _webApiContext.Dispose();
            _webApiContext = null;
        }

        protected abstract void SetUpWebApi(IAppBuilder appBuilder);
    }
}