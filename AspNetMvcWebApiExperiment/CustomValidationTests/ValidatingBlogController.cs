using System.Web.Http;

namespace AspNetMvcWebApiExperiment.CustomValidationTests
{
    [ValidateModel]
    public class ValidatingBlogController : ApiController
    {
        [ActionName("SubmitPost")]
        [HttpPost]
        public Post SubmitPost(Post post)
        {
            return post;
        }
    }
}