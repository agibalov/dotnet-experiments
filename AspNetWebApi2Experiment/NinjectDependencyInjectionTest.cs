using System.Net;
using System.Web.Http;
using NUnit.Framework;
using Ninject;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class NinjectDependencyInjectionTest : AbstractWebApiTest
    {
        [Test]
        public void CanUseNinjectToInjectDependenciesToControllers()
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

            var kernel = new StandardKernel();
            kernel.Bind<ContentGenerator>().ToSelf().InSingletonScope();
            httpConfiguration.DependencyResolver = new NinjectDependencyResolver(kernel);

            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{action}", new { controller = "WantsContentGeneratorInjectedApi" });
            appBuilder.UseWebApi(httpConfiguration);
        }

        public class ContentGenerator
        {
            public string GenerateContent(int postId)
            {
                return string.Format("Content {0}", postId);
            }
        }

        public class WantsContentGeneratorInjectedApiController : ApiController
        {
            [Inject]
            public ContentGenerator ContentGenerator { get; set; }

            public PostDto GetPost(int postId)
            {
                return new PostDto
                    {
                        Id = postId,
                        Content = ContentGenerator.GenerateContent(postId)
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