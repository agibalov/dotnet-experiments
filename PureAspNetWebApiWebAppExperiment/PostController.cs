using System.Web.Http;

namespace PureAspNetWebApiWebAppExperiment
{
    public class PostController : ApiController
    {
        [HttpGet]
        public Post GetPost(int postId)
        {
            return new Post
                {
                    Title = string.Format("hello there #{0}", postId),
                    Text = "the rest of the post goes here",
                    Self = Url.Link("BlogApi", new { action = "GetPost", postId })
                };
        }
    }

    public class Post
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Self { get; set; }
    }
}