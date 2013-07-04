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

        [Test]
        public void DifferentUsersHaveDifferentPosts()
        {
            var user1 = BlogService.CreateUser("user1");
            var user1post1 = BlogService.CreatePost(user1.UserId, "user 1 post 1");
            var user1post2 = BlogService.CreatePost(user1.UserId, "user 1 post 2");

            var user2 = BlogService.CreateUser("user2");
            var user2post1 = BlogService.CreatePost(user2.UserId, "user 2 post 1");
            var user2post2 = BlogService.CreatePost(user2.UserId, "user 2 post 2");
            var user2post3 = BlogService.CreatePost(user2.UserId, "user 2 post 3");

            var userCount = BlogService.GetUserCount();
            Assert.AreEqual(2, userCount);

            var postCount = BlogService.GetPostCount();
            Assert.AreEqual(5, postCount);

            var posts = BlogService.GetAllPosts();
            AssertEqualPosts(user1post1, posts[0]);
            AssertEqualPosts(user1post2, posts[1]);
            AssertEqualPosts(user2post1, posts[2]);
            AssertEqualPosts(user2post2, posts[3]);
            AssertEqualPosts(user2post3, posts[4]);
        }
    }
}