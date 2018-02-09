using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Hosting.Self;
using Ninject;
using NUnit.Framework;
using RestSharp;

namespace NancyTests
{
    public class NinjectBootstrapperTest
    {
        [Test]
        public void CanGetResponseFromTheApp()
        {
            using (var host = new NancyHost(new MyNinjectBootstrapper(), new Uri("http://localhost:2302")))
            {
                host.Start();

                var restClient = new RestClient("http://localhost:2302");
                var response = restClient.Execute(new RestRequest("/hello/loki2302", Method.GET));
                Assert.AreEqual(ResponseStatus.Completed, response.ResponseStatus);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual("Hello, loki2302 /hello/loki2302!", response.Content);
            }
        }

        public class MyNinjectBootstrapper : NinjectNancyBootstrapper
        {
            protected override void ConfigureApplicationContainer(IKernel container)
            {
                container.Bind<HelloService>()
                    .ToSelf()
                    .InSingletonScope()
                    .WithConstructorArgument("messageTemplate", "Hello, {0}!");
            }

            protected override void ConfigureRequestContainer(IKernel container, NancyContext context)
            {
                container.Bind<string>()
                    .ToConstant(context.Request.Path)
                    .InSingletonScope()
                    .Named("theRequestPath");
            }

            protected override IEnumerable<ModuleRegistration> Modules
            {
                get { return new[] { new ModuleRegistration(typeof(HelloWorldModule)) }; }
            }

            protected override IRootPathProvider RootPathProvider
            {
                get
                {
                    return new FileSystemRootPathProvider();
                }
            }
        }

        public class HelloService
        {
            private readonly string _messageTemplate;

            public HelloService(string messageTemplate)
            {
                _messageTemplate = messageTemplate;
            }

            public string MakeHelloMessage(string name)
            {
                return string.Format(_messageTemplate, name);
            }
        }

        public class HelloWorldModule : NancyModule
        {
            public HelloWorldModule(HelloService helloService, [Named("theRequestPath")] string requestPath)
            {
                Get["/hello/{name}"] = parameters => helloService.MakeHelloMessage(parameters.name + " " + requestPath);
            }
        }
    }
}