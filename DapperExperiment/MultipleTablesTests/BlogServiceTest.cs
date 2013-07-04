using NUnit.Framework;

namespace DapperExperiment.MultipleTablesTests
{
    public class BlogServiceTest : AbstractDatabaseTest
    {
        private BlogService _blogService;

        [SetUp]
        public void ConstructUserDAO()
        {
            _blogService = new BlogService(DatabaseHelper);
            _blogService.CreateSchema();
        }

        [Test]
        public void CanCreateUser()
        {
            var user = _blogService.CreateUser("loki2302");
            Assert.AreEqual(1, user.UserId);
            Assert.AreEqual("loki2302", user.UserName);
            Assert.AreEqual(0, user.Posts.Count);
        }

        [Test]
        public void CanCreatePost()
        {
            var user = _blogService.CreateUser("loki2302");
            var post = _blogService.CreatePost(user.UserId, "test post");
            Assert.AreEqual(1, post.PostId);
            Assert.AreEqual("test post", post.PostText);
            Assert.NotNull(post.User);
            Assert.AreEqual(user.UserId, post.User.UserId);
            Assert.AreEqual(user.UserName, post.User.UserName);
        }
    }
}