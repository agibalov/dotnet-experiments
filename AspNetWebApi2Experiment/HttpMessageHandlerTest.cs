using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using NUnit.Framework;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class HttpMessageHandlerTest : AbstractWebApiTest
    {
        [Test]
        public void CanUseCustomHttpMessageHandler()
        {
            var restClient = new RestClient(BaseAddress);
            
            var response1 = restClient.Get<ResultDto>(new RestRequest("something"));
            Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
            Assert.AreEqual("/something", response1.Data.Url);

            var response2 = restClient.Get<ResultDto>(new RestRequest("something-else/here"));
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            Assert.AreEqual("/something-else/here", response2.Data.Url);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MessageHandlers.Add(new MyHttpMessageHandler());
            appBuilder.UseWebApi(httpConfiguration);
        }

        public class MyHttpMessageHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return base.SendAsync(request, cancellationToken).ContinueWith(task =>
                    {
                        var resultDto = new ResultDto
                            {
                                Message = "hello!", 
                                Url = request.RequestUri.LocalPath
                            };
                        return request.CreateResponse(HttpStatusCode.OK, resultDto);
                    });
            }
        }

        public class ResultDto
        {
            public string Message { get; set; }
            public string Url { get; set; }
        }
    }
}