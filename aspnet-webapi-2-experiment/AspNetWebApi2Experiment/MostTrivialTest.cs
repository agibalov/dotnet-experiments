using System.Net;
using System.Web.Http;
using NUnit.Framework;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class MostTrivialTest : AbstractWebApiTest
    {
        [Test]
        public void CanSelfHostWebApiAndMakeARequest()
        {
            var restClient = new RestClient(BaseAddress);
            
            var request = new RestRequest("GetPost", Method.GET);
            request.AddParameter("postId", 123);
            var response = restClient.Get<PostDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var postDto = response.Data;
            Assert.AreEqual(123, postDto.Id);
            Assert.AreEqual("Content 123", postDto.Content);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{action}", new { controller = "TrivialApi" });
            appBuilder.UseWebApi(httpConfiguration);
        }

        public class TrivialApiController : ApiController
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
    }
}