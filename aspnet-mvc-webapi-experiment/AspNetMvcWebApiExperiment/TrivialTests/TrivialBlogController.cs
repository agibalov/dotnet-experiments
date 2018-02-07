using System.Web.Http;

namespace AspNetMvcWebApiExperiment.TrivialTests
{
    public class TrivialBlogController : ApiController
    {
        [ActionName("SubmitPost")]
        [HttpPost]
        public Post SubmitPost(Post post)
        {
            return post;
        }
    }
}