using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using NUnit.Framework;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class CustomControllerAndActionResolutionTest : AbstractWebApiTest
    {
        [Test]
        public void CanHandleMultipleUrlsWithSingleAction()
        {
            var restClient = new RestClient(BaseAddress);
            Assert.AreEqual("/DoSomething", restClient.Get<ResultDto>(new RestRequest("DoSomething")).Data.LocalPath);
            Assert.AreEqual("/DontDoAnything", restClient.Post<ResultDto>(new RestRequest("DontDoAnything")).Data.LocalPath);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{whatever}");

            httpConfiguration.Services.Replace(typeof(IHttpControllerSelector), new FixedControllerSelector(typeof(DummyController)));
            httpConfiguration.Services.Replace(typeof(IHttpActionSelector), new FixedActionSelector(typeof(DummyController).GetMethod("GetResult")));

            appBuilder.UseWebApi(httpConfiguration);
        }

        public class FixedControllerSelector : IHttpControllerSelector
        {
            private readonly Type _controllerType;

            public FixedControllerSelector(Type controllerType)
            {
                _controllerType = controllerType;
            }

            public HttpControllerDescriptor SelectController(HttpRequestMessage request)
            {
                return new HttpControllerDescriptor(request.GetConfiguration(), "dummy", _controllerType);
            }

            public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
            {
                return new Dictionary<string, HttpControllerDescriptor>
                    {
                        { "dummy", new HttpControllerDescriptor(null, "dummy", _controllerType) }
                    };
            }
        }

        public class FixedActionSelector : IHttpActionSelector
        {
            private readonly MethodInfo _methodInfo;

            public FixedActionSelector(MethodInfo methodInfo)
            {
                _methodInfo = methodInfo;
            }

            public HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
            {
                return new ReflectedHttpActionDescriptor(controllerContext.ControllerDescriptor, _methodInfo);
            }

            public ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
            {
                return new Dictionary<string, HttpActionDescriptor> 
                    {
                        { _methodInfo.Name, new ReflectedHttpActionDescriptor(controllerDescriptor, _methodInfo) }
                    }.ToLookup(x => x.Key, x => x.Value);
            }
        }

        public class DummyController : ApiController
        {
            public ResultDto GetResult()
            {
                return new ResultDto
                    {
                        Message = "Hello " + Request.RequestUri.LocalPath,
                        LocalPath = Request.RequestUri.LocalPath
                    };
            }
        }

        public class ResultDto
        {
            public string Message { get; set; }
            public string LocalPath { get; set; }
        }
    }
}