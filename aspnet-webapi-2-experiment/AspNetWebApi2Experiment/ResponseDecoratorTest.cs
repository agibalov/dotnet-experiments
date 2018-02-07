using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using NUnit.Framework;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class ResponseDecoratorTest : AbstractWebApiTest
    {
        [Test]
        public void CanGetDecoratedSuccessResponse()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("GetPost", Method.GET);
            request.AddParameter("postId", 123);
            var response = restClient.Get<ActualGetPostResultDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var getPostResultDto = response.Data;
            Assert.IsTrue(getPostResultDto.Ok);
            Assert.AreEqual(123, getPostResultDto.Payload.Id);
            Assert.AreEqual("Content 123", getPostResultDto.Payload.Content);
        }

        [Test]
        public void CanGetDecoratedFailureResponse()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("GetPost", Method.GET);
            request.AddParameter("postId", -1);
            var response = restClient.Get<ActualGetPostResultDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var getPostResultDto = response.Data;
            Assert.IsFalse(getPostResultDto.Ok);
            Assert.IsNull(getPostResultDto.Payload);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Filters.Add(new DecorateResponseActionFilterAttribute());
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{action}", new { controller = "WantsToHaveItsResponsesDecoratedApi" });
            appBuilder.UseWebApi(httpConfiguration);
        }

        public class WantsToHaveItsResponsesDecoratedApiController : ApiController
        {
            public PostDto GetPost(int postId)
            {
                if (postId < 0)
                {
                    throw new Exception();
                }

                return new PostDto
                    {
                        Id = postId,
                        Content = string.Format("Content {0}", postId)
                    };
            }
        }

        public class PostDto
        {
            public int Id { get; set; }
            public string Content { get; set; }
        }

        public class ResultDto
        {
            public bool Ok { get; set; }
            public object Payload { get; set; }
        }

        public class ActualGetPostResultDto
        {
            public bool Ok { get; set; }
            public PostDto Payload { get; set; }
        }

        public class DecorateResponseActionFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
            {
                var request = actionExecutedContext.Request;
                if (actionExecutedContext.Exception == null)
                {
                    var objectContent = (ObjectContent) actionExecutedContext.Response.Content;
                    var result = new ResultDto
                        {
                            Ok = true,
                            Payload = objectContent.Value
                        };
                    actionExecutedContext.Response = request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    var result = new ResultDto
                        {
                            Ok = false
                        };
                    actionExecutedContext.Response = request.CreateResponse(HttpStatusCode.OK /* it's by intention */, result);
                }
            }
        }
    }
}