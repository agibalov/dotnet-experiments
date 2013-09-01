using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace AspNetMvcWebApiExperiment.CustomValidationTests
{
    public class ValidationTests : WebApiTests
    {
        public ValidationTests()
            : base("ValidatingBlog")
        {}

        [Test]
        public void CanGetErrorForTitleProperty()
        {
            var result = Service.PostAsJsonAsync(
                "/SubmitPost",
                new Post
                {
                    Title = "very long title goes here",
                    Text = "text"
                })
                .Result;

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var errors = result.Content.ReadAsAsync<IDictionary<string, string>>().Result;
            Assert.That(errors.Count, Is.EqualTo(1));

            var theOnlyPropertyInError = errors.Keys.Single();
            Assert.That(theOnlyPropertyInError, Is.EqualTo("post.Title"));

            var errorForTheOnlyPropertyInError = errors[theOnlyPropertyInError];
            Assert.That(errorForTheOnlyPropertyInError, Is.EqualTo("At most 10"));
        }

        [Test]
        public void CanGetErrorForTextProperty()
        {
            var result = Service.PostAsJsonAsync(
                "/SubmitPost",
                new Post
                {
                    Title = "hello",
                    Text = null
                })
                .Result;

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var errors = result.Content.ReadAsAsync<IDictionary<string, string>>().Result;
            Assert.That(errors.Count, Is.EqualTo(1));

            var theOnlyPropertyInError = errors.Keys.Single();
            Assert.That(theOnlyPropertyInError, Is.EqualTo("post.Text"));

            var errorForTheOnlyPropertyInError = errors[theOnlyPropertyInError];
            Assert.That(errorForTheOnlyPropertyInError, Is.EqualTo("Not empty"));
        }
    }
}
