using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NUnit.Framework;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class RequestValidatorTest : AbstractWebApiTest
    {
        [Test]
        public void CanGetSuccessResponseWithAdequateRequest()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("GetPost", Method.GET);
            request.AddParameter("postId", 123);
            var response = restClient.Get<PostDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void CanGetFailureResponseWithInadequateRequest()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("GetPost", Method.GET);
            request.AddParameter("postId", -1);
            var response = restClient.Get<PostDto>(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Filters.Add(new ValidateRequestActionFilterAttribute());
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{action}", new { controller = "WantsToHaveItsRequestsValidatedApi" });
            appBuilder.UseWebApi(httpConfiguration);
        }

        public class WantsToHaveItsRequestsValidatedApiController : ApiController
        {
            public PostDto GetPost(int postId)
            {
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

        public class ValidateRequestActionFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(HttpActionContext actionContext)
            {
                var postId = (int)actionContext.ActionArguments["postId"];
                if (postId < 0)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
        }
    }
}