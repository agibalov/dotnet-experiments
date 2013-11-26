using System;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using ImpromptuInterface;
using NUnit.Framework;
using Newtonsoft.Json;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class MimicWcfTest : AbstractWebApiTest
    {
        [Test]
        public void Dummy()
        {
            var calculatorService = MakeProxy<ICalculatorService>(BaseAddress);
            
            var response1 = calculatorService.AddNumbers(1, 2);
            Assert.AreEqual(3, response1.Result);

            var response2 = calculatorService.SubNumbers(1, 2);
            Assert.AreEqual(-1, response2.Result);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{action}", new { controller = "CalculatorService" });
            appBuilder.UseWebApi(httpConfiguration);
        }

        private static TApi MakeProxy<TApi>(string baseAddress) where TApi : class
        {
            var restClient = new RestClient(BaseAddress);
            var proxy = new ProxyImplementor(restClient, typeof(TApi)).ActLike<TApi>();
            return proxy;
        }
        
        public class ProxyImplementor : DynamicObject
        {
            private readonly RestClient _restClient;
            private readonly Type _serviceType;

            public ProxyImplementor(RestClient restClient, Type serviceType)
            {
                _restClient = restClient;
                _serviceType = serviceType;
            }

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                var methodName = binder.Name;
                var methodInfo = _serviceType.GetMethod(methodName);
                if (methodInfo == null)
                {
                    throw new Exception("No such method");
                }

                var actionHttpMethodProviderAttributes = methodInfo.GetCustomAttributes(typeof (IActionHttpMethodProvider), true);
                if (actionHttpMethodProviderAttributes.Length != 1)
                {
                    throw new Exception("Don't know how to access the action");
                }

                var actionHttpMethodProviderAttribute = actionHttpMethodProviderAttributes.OfType<IActionHttpMethodProvider>().Single();
                if (actionHttpMethodProviderAttribute.HttpMethods.Count != 1)
                {
                    throw new Exception("Don't know how to access the action");
                }

                var httpMethod = actionHttpMethodProviderAttribute.HttpMethods.Single();
                Method method;
                if (httpMethod == HttpMethod.Get)
                {
                    method = Method.GET;
                }
                else if (httpMethod == HttpMethod.Post)
                {
                    method = Method.POST;
                }
                else
                {
                    throw new Exception("Unknown HTTP method");
                }

                var restRequest = new RestRequest(methodName, method)
                    {
                        RequestFormat = DataFormat.Json
                    };
                var methodParameters = methodInfo.GetParameters();
                for(var parameterIndex = 0; parameterIndex < methodParameters.Length; ++parameterIndex)
                {
                    var methodParameter = methodParameters[parameterIndex];
                    var argument = args[parameterIndex];
                    restRequest.AddParameter(methodParameter.Name, argument);
                }
                
                var content = _restClient.Execute(restRequest).Content;

                result = JsonConvert.DeserializeObject(content, methodInfo.ReturnType);

                return true;
            }
        }

        public interface ICalculatorService
        {
            [HttpGet]
            ResultDto AddNumbers(int a, int b);

            [HttpGet]
            ResultDto SubNumbers(int a, int b);
        }

        public class ResultDto
        {
            public int Result { get; set; }
        }

        public class CalculatorServiceController : ApiController, ICalculatorService
        {
            [HttpGet] // TODO: get rid of this
            public ResultDto AddNumbers(int a, int b)
            {
                return new ResultDto {Result = a + b};
            }

            [HttpGet] // TODO: get rid of this
            public ResultDto SubNumbers(int a, int b)
            {
                return new ResultDto { Result = a - b };
            }
        }
    }
}