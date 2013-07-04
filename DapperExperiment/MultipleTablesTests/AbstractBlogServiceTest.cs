using DapperExperiment.MultipleTablesTests.DAL;
using DapperExperiment.MultipleTablesTests.Service;
using DapperExperiment.MultipleTablesTests.Service.DTO;
using NUnit.Framework;

namespace DapperExperiment.MultipleTablesTests
{
    public class AbstractBlogServiceTest : AbstractDatabaseTest
    {
        protected BlogService BlogService;

        [SetUp]
        public void ConstructBlogService()
        {
            var userDao = new UserDAO();
            var postDao = new PostDAO();
            var tagDao = new TagDAO();
            BlogService = new BlogService(DatabaseHelper, userDao, postDao, tagDao);
            BlogService.CreateSchema();
        }

        protected static void AssertEqualUsers(UserDTO expected, UserDTO actual)
        {
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.UserName, actual.UserName);
            Assert.AreEqual(expected.Posts.Count, actual.Posts.Count);
            // TODO: compare posts
        }

        protected static void AssertEqualUsers(BriefUserDTO expected, UserDTO actual)
        {
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.UserName, actual.UserName);
        }

        protected static void AssertEqualUsers(UserDTO expected, BriefUserDTO actual)
        {
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.UserName, actual.UserName);
        }

        protected static void AssertEqualUsers(BriefUserDTO expected, BriefUserDTO actual)
        {
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.UserName, actual.UserName);
        }

        protected static void AssertEqualPosts(PostDTO expected, PostDTO actual)
        {
            Assert.AreEqual(expected.PostId, actual.PostId);
            Assert.AreEqual(expected.PostText, actual.PostText);
            AssertEqualUsers(expected.User, actual.User);
        }
    }
}