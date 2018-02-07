using System.Net.Http;
using NUnit.Framework;

namespace AspNetMvcWebApiExperiment.TrivialTests
{
    public class TrivialTests : WebApiTests
    {
        public TrivialTests()
            : base("TrivialBlog")
        {}

        [Test]
        public void CanSubmitBlogPostAndGetItBackAsIs()
        {
            var result = Service.PostAsJsonAsync(
                "/SubmitPost", 
                new Post
                {
                    Title = "title",
                    Text = "text"
                })
                .Result.Content.ReadAsAsync<Post>().Result;
            Assert.That(result.Title, Is.EqualTo("title"));
            Assert.That(result.Text, Is.EqualTo("text"));
        }
    }
}
