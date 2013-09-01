using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace AspNetMvcWebApiExperiment.CustomAuthenticationTests
{
    public class AuthenticationTests : WebApiTests
    {
        public AuthenticationTests()
            : base("AuthenticatingBlog")
        {}

        [Test]
        public void CanOk()
        {
            Service.DefaultRequestHeaders.Add("MagicSessionToken", "secret123token");

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

        [Test]
        public void CanFail()
        {
            var result = Service.PostAsJsonAsync(
                "/SubmitPost",
                new Post
                {
                    Title = "very long title goes here",
                    Text = "text"
                })
                .Result;

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(result.ReasonPhrase, Is.EqualTo("You should authenticate"));
        }
    }
}
