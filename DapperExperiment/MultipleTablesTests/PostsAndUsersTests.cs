using DapperExperiment.MultipleTablesTests.DAL;
using NUnit.Framework;

namespace DapperExperiment.MultipleTablesTests
{
    public class PostsAndUsersTests : AbstractBlogServiceTest
    {
        [Test]
        public void CanDeleteUserThatHasPosts()
        {
            var user = BlogService.CreateUser("loki2302");
            BlogService.CreatePost(user.UserId, "test post");
            BlogService.DeleteUser(user.UserId);

            var userCount = BlogService.GetUserCount();
            Assert.AreEqual(0, userCount);

            var postCount = BlogService.GetPostCount();
            Assert.AreEqual(0, postCount);
        }

        [Test]
        [ExpectedException(typeof(NoSuchUserException))]
        public void CantCreatePostIfUserDoesNotExist()
        {
            BlogService.CreatePost(123, "sample post");
        }
    }
}