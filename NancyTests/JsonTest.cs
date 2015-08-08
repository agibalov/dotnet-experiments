using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using ISerializer = RestSharp.Serializers.ISerializer;

namespace NancyTests
{
    public class JsonTest
    {
        [Test]
        public void CanUseJSON()
        {
            using (var host = new NancyHost(new MyJsonBootstrapper(), new Uri("http://localhost:2302")))
            {
                host.Start();

                var restClient = new RestClient("http://localhost:2302");
                var request = new RestRequest("/sum", Method.POST)
                {
                    JsonSerializer = new MyRestSharpJsonSerializer() { ContentType = "application/json" },
                    RequestFormat = DataFormat.Json
                };
                request.AddBody(new
                {
                    A = 2,
                    B = 3
                });

                var response = restClient.Execute(request);
                Assert.AreEqual(ResponseStatus.Completed, response.ResponseStatus);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var responseBodyObject = JObject.Parse(response.Content);
                Assert.AreEqual(2, responseBodyObject["A"].Value<int>());
                Assert.AreEqual(3, responseBodyObject["B"].Value<int>());
                Assert.AreEqual(5, responseBodyObject["Sum"].Value<int>());
            }
        }

        public class MyRestSharpJsonSerializer : ISerializer
        {
            public string Serialize(object obj)
            {
                return JsonConvert.SerializeObject(obj);
            }

            public string RootElement { get; set; }
            public string Namespace { get; set; }
            public string DateFormat { get; set; }
            public string ContentType { get; set; }
        }

        public class JsonModule : NancyModule
        {
            public JsonModule()
            {
                Post["/sum"] = parameters =>
                {
                    var request = this.Bind<SumRequest>();
                    return new
                    {
                        A = request.A,
                        B = request.B,
                        Sum = request.A + request.B
                    };
                };
            }

            public class SumRequest
            {
                public int A { get; set; }
                public int B { get; set; }
            }
        }

        public class MyJsonBootstrapper : DefaultNancyBootstrapper
        {
            protected override IEnumerable<ModuleRegistration> Modules
            {
                get { return new[] { new ModuleRegistration(typeof(JsonModule)) }; }
            }
        }
    }
}
