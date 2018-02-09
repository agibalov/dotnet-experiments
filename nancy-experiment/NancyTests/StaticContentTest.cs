using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Hosting.Self;
using NUnit.Framework;
using RestSharp;

namespace NancyTests
{
    public class StaticContentTest
    {
        [Test]
        public void CanAccessContentMappedAsASingleFile()
        {
            using (var host = new NancyHost(new StaticContentBootstrapper(), new Uri("http://localhost:2302")))
            {
                host.Start();

                var restClient = new RestClient("http://localhost:2302");
                var response = restClient.Execute(new RestRequest("/hello", Method.GET));
                Assert.AreEqual(ResponseStatus.Completed, response.ResponseStatus);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
                Assert.True(response.Content.Contains("I am somefile.txt!"));
            }
        }

        [Test]
        public void CanAccessContentMappedAsADirectory()
        {
            using (var host = new NancyHost(new StaticContentBootstrapper(), new Uri("http://localhost:2302")))
            {
                host.Start();

                var restClient = new RestClient("http://localhost:2302");
                var response = restClient.Execute(new RestRequest("/extras/another-file.txt", Method.GET));
                Assert.AreEqual(ResponseStatus.Completed, response.ResponseStatus);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
                Assert.True(response.Content.Contains("I am another-file.txt!"));
            }
        }

        public class StaticContentBootstrapper : DefaultNancyBootstrapper
        {
            protected override IRootPathProvider RootPathProvider
            {
                get
                {
                    return new ExecutingAssemblyRootPathProvider();
                }
            }

            protected override void ConfigureConventions(NancyConventions conventions)
            {
                base.ConfigureConventions(conventions);

                conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddFile("/hello", "/public/somefile.txt"));
                conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/extras", "/public/more"));
            }

            protected override IEnumerable<ModuleRegistration> Modules
            {
                get
                {
                    return new ModuleRegistration[] {};
                }
            }
        }

        public class ExecutingAssemblyRootPathProvider : IRootPathProvider
        {
            public string GetRootPath()
            {
                var codeBaseLocationUri = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                if (codeBaseLocationUri == null)
                {
                    throw new Exception();
                }

                var codeBaseLocationPath = new Uri(codeBaseLocationUri).LocalPath;
                return codeBaseLocationPath;
            }
        }
    }
}
