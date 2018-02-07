using System.Net;
using System.Web.Http;
using NUnit.Framework;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class ControllerClassPerRequestTypeTest : AbstractWebApiTest
    {
        [Test]
        public void CanAddNumbers()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("AddNumbers", Method.GET);
            request.AddParameter("a", 1);
            request.AddParameter("b", 2);
            var response = restClient.Get<ResultDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(3, response.Data.Result);
        }

        [Test]
        public void CanSubNumbers()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("SubNumbers", Method.GET);
            request.AddParameter("a", 1);
            request.AddParameter("b", 2);
            var response = restClient.Get<ResultDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(-1, response.Data.Result);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{controller}", new { action = "Process" });
            appBuilder.UseWebApi(httpConfiguration);
        }

        public class AddNumbersController : ApiController
        {
            [HttpGet]
            public ResultDto Process(int a, int b)
            {
                return new ResultDto
                    {
                        Result = a + b
                    };
            }
        }

        public class SubNumbersController : ApiController
        {
            [HttpGet]
            public ResultDto Process(int a, int b)
            {
                return new ResultDto
                    {
                        Result = a - b
                    };
            }
        }

        public class ResultDto
        {
            public int Result { get; set; }
        }
    }
}