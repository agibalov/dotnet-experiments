using System.Web.Http;

namespace AspNetMvcWebApiExperiment.CustomAuthenticationTests
{
    [Authenticate]
    public class AuthenticatingBlogController : ApiController
    {
        [ActionName("SubmitPost")]
        [HttpPost]
        public Post SubmitPost(Post post)
        {
            return post;
        }
    }
}