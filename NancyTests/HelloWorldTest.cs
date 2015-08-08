using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;
using NUnit.Framework;
using RestSharp;

namespace NancyTests
{
    public class HelloWorldTest
    {
        [Test]
        public void CanGetResponseFromTheApp()
        {
            using (var host = new NancyHost(new HelloWorldModuleBootstrapper(), new Uri("http://localhost:2302")))
            {
                host.Start();

                var restClient = new RestClient("http://localhost:2302");
                var response = restClient.Execute(new RestRequest("/hello/loki2302", Method.GET));
                Assert.AreEqual(ResponseStatus.Completed, response.ResponseStatus);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual("Hello, loki2302!", response.Content);
            }
        }

        public class HelloWorldModuleBootstrapper : DefaultNancyBootstrapper
        {
            protected override IEnumerable<ModuleRegistration> Modules
            {
                get { return new[] {new ModuleRegistration(typeof (HelloWorldModule))}; }
            }
        }

        public class HelloWorldModule : NancyModule
        {
            public HelloWorldModule()
            {
                Get["/hello/{name}"] = parameters => string.Format("Hello, {0}!", parameters.name);
            }
        }
    }
}
