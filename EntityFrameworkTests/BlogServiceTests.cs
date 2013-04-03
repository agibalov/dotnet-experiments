using System;
using EntityFrameworkExperiment;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace EntityFrameworkTests
{
    [TestClass]
    public class BlogServiceTests
    {
        private BloggingService _service;

        [TestInitialize]
        public void CleanUp()
        {
            using (var context = new BlogContext())
            {
                var database = context.Database;
                if (database.Exists())
                {
                    database.Delete();
                }

                database.Create();
            }

            var kernel = new StandardKernel();
            _service = kernel.Get<BloggingService>();
        }

        [TestMethod]
        public void CanCreateUser()
        {
            var user = ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            AssertGoodId(user.UserId);
            AssertNotEmptyString(user.UserName);
        }

        [TestMethod]
        public void CantCreateUserIfUserNameAlreadyUsed()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            ExpectFail(ServiceError.UserNameAlreadyRegistered, () => _service.CreateUser("loki2302", "qwerty"));
        }

        [TestMethod]
        public void CanAuthenticateExistingUser()
        {
            var createdUser = ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            AssertNotEmptyString(session.SessionToken);
            var authenticatedUser = session.User;
            Assert.AreEqual(createdUser.UserId, authenticatedUser.UserId);
            Assert.AreEqual(createdUser.UserName, authenticatedUser.UserName);
        }

        [TestMethod]
        public void CantAuthenticateUserThatDoesNotExist()
        {
            ExpectFail(ServiceError.NoSuchUser, () => _service.Authenticate("loki2302", "qwerty"));
        }

        [TestMethod]
        public void CantAuthenticateWithInvalidPassword()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            ExpectFail(ServiceError.InvalidPassword, () => _service.Authenticate("loki2302", "asdfgh"));
        }

        [TestMethod]
        public void CanCreatePost()
        {
            var createdUser = ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session.SessionToken, "test post"));
            AssertGoodId(post.PostId);
            Assert.AreEqual("test post", post.PostText);
            var postAuthor = post.Author;
            Assert.AreEqual(createdUser.UserId, postAuthor.UserId);
            Assert.AreEqual(createdUser.UserName, postAuthor.UserName);
        }

        [TestMethod]
        public void CantCreatePostWithInvalidSessionToken()
        {
            ExpectFail(ServiceError.InvalidSession, () => _service.CreatePost("123", "test post"));
        }

        [TestMethod]
        public void CanGetPost()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var createdPost = ExpectOk(() => _service.CreatePost(session.SessionToken, "test post"));
            var retrievedPost = ExpectOk(() => _service.GetPost(session.SessionToken, createdPost.PostId));
            Assert.AreEqual(createdPost.PostId, retrievedPost.PostId);
            Assert.AreEqual(createdPost.PostText, retrievedPost.PostText);
            Assert.AreEqual(createdPost.Author.UserId, retrievedPost.Author.UserId);
            Assert.AreEqual(createdPost.Author.UserName, retrievedPost.Author.UserName);
        }

        [TestMethod]
        public void CantGetPostWithInvalidSessionToken()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session.SessionToken, "test post"));
            ExpectFail(ServiceError.InvalidSession, () => _service.GetPost("123", post.PostId));
        }

        [TestMethod]
        public void CantGetPostThatDoesNotExist()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            ExpectFail(ServiceError.NoSuchPost, () => _service.GetPost(session.SessionToken, 123));
        }

        [TestMethod]
        public void CanUpdatePost()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session.SessionToken, "test post"));
            var updatedPost = ExpectOk(() => _service.UpdatePost(session.SessionToken, post.PostId, "other text"));
            Assert.AreEqual(post.PostId, updatedPost.PostId);
            Assert.AreEqual("other text", updatedPost.PostText);
            Assert.AreEqual(post.Author.UserId, updatedPost.Author.UserId);
            Assert.AreEqual(post.Author.UserName, updatedPost.Author.UserName);

            var retrievedPost = ExpectOk(() => _service.GetPost(session.SessionToken, post.PostId));
            Assert.AreEqual(post.PostId, retrievedPost.PostId);
            Assert.AreEqual("other text", retrievedPost.PostText);
            Assert.AreEqual(post.Author.UserId, retrievedPost.Author.UserId);
            Assert.AreEqual(post.Author.UserName, retrievedPost.Author.UserName);
        }

        [TestMethod]
        public void CantUpdatePostThatDoesNotExist()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            ExpectFail(ServiceError.NoSuchPost, () => _service.UpdatePost(session.SessionToken, 123, "other text"));
        }

        [TestMethod]
        public void CantUpdatePostWithInvalidSessionToken()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session.SessionToken, "test post"));
            ExpectFail(ServiceError.InvalidSession, () => _service.UpdatePost("123", post.PostId, "other text"));
        }

        [TestMethod]
        public void CantUpdatePostThatDoesNotBelongToTheUser()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session1 = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session1.SessionToken, "test post"));

            ExpectOk(() => _service.CreateUser("qwerty", "qwerty"));
            var session2 = ExpectOk(() => _service.Authenticate("qwerty", "qwerty"));
            ExpectFail(ServiceError.NoPermissions, () => _service.UpdatePost(session2.SessionToken, post.PostId, "other text"));
        }

        [TestMethod]
        public void CanDeletePost()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session.SessionToken, "test post"));
            ExpectOk(() => _service.DeletePost(session.SessionToken, post.PostId));
            ExpectFail(ServiceError.NoSuchPost, () => _service.GetPost(session.SessionToken, post.PostId));
        }

        [TestMethod]
        public void CantDeletePostThatDoesNotExist()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            ExpectFail(ServiceError.NoSuchPost, () => _service.DeletePost(session.SessionToken, 123));
        }

        [TestMethod]
        public void CantDeletePostWithInvalidSessionToken()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session.SessionToken, "test post"));
            ExpectFail(ServiceError.InvalidSession, () => _service.DeletePost("123", post.PostId));
        }

        [TestMethod]
        public void CantDeletePostThatDoesNotBelongToTheUser()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session1 = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session1.SessionToken, "test post"));

            ExpectOk(() => _service.CreateUser("qwerty", "qwerty"));
            var session2 = ExpectOk(() => _service.Authenticate("qwerty", "qwerty"));
            ExpectFail(ServiceError.NoPermissions, () => _service.DeletePost(session2.SessionToken, post.PostId));
        }

        [TestMethod]
        public void CanGetAllPosts()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));

            const int numberOfPosts = 7;
            for (var i = 0; i < numberOfPosts; ++i)
            {
                ExpectOk(() => _service.CreatePost(session.SessionToken, "post"));
            }

            var page1 = ExpectOk(() => _service.GetPosts(session.SessionToken, 4, 0));
            Assert.AreEqual(numberOfPosts, page1.TotalItemCount);
            Assert.AreEqual(4, page1.Items.Count);

            var page2 = ExpectOk(() => _service.GetPosts(session.SessionToken, 4, 1));
            Assert.AreEqual(numberOfPosts, page2.TotalItemCount);
            Assert.AreEqual(3, page2.Items.Count);
        }

        [TestMethod]
        public void CanGetUserDetailsForExistingUser()
        {
            var user = ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));

            ExpectOk(() => _service.CreatePost(session.SessionToken, "post 1"));
            var post2 = ExpectOk(() => _service.CreatePost(session.SessionToken, "post 2"));
            var post3 = ExpectOk(() => _service.CreatePost(session.SessionToken, "post 3"));
            var post4 = ExpectOk(() => _service.CreatePost(session.SessionToken, "post 4"));

            var userDetails = ExpectOk(() => _service.GetUserDetails(session.SessionToken, user.UserId, 3, 3));
            Assert.AreEqual(user.UserId, userDetails.UserId);
            Assert.AreEqual(user.UserName, userDetails.UserName);
            Assert.AreEqual(4, userDetails.NumberOfPosts);
            Assert.AreEqual(3, userDetails.RecentPosts.Count);
            Assert.AreEqual(post4.PostId, userDetails.RecentPosts[0].PostId);
            Assert.AreEqual(post3.PostId, userDetails.RecentPosts[1].PostId);
            Assert.AreEqual(post2.PostId, userDetails.RecentPosts[2].PostId);
            Assert.AreEqual(0, userDetails.RecentComments.Count);
        }

        [TestMethod]
        public void CantGetUserDetailsForUserThatDoesNotExist()
        {
            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            ExpectFail(ServiceError.NoSuchUser, () => _service.GetUserDetails(session.SessionToken, 123, 3, 3));
        }

        [TestMethod]
        public void CanGetMostActiveUsers()
        {
            var user1Id = CreateUserWithPosts("loki1", 10);
            CreateUserWithPosts("loki2", 1);
            var user3Id = CreateUserWithPosts("loki3", 8);
            var user4Id = CreateUserWithPosts("loki4", 3);
            CreateUserWithPosts("loki5", 1);

            ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var mostActiveUsers = ExpectOk(() => _service.GetMostActiveUsers(session.SessionToken, 3));
            Assert.AreEqual(3, mostActiveUsers.Count);
            Assert.AreEqual(user1Id, mostActiveUsers[0].UserId);
            Assert.AreEqual(user3Id, mostActiveUsers[1].UserId);
            Assert.AreEqual(user4Id, mostActiveUsers[2].UserId);
        }

        [TestMethod]
        public void CanCreateCommentForExistingPost()
        {
            var user = ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            var session = ExpectOk(() => _service.Authenticate("loki2302", "qwerty"));
            var post = ExpectOk(() => _service.CreatePost(session.SessionToken, "my post"));
            ExpectOk(() => _service.CreateComment(session.SessionToken, post.PostId, "my comment"));
            var retrievedPost = ExpectOk(() => _service.GetPost(session.SessionToken, post.PostId));
            Assert.AreEqual(1, retrievedPost.Comments.Count);
            var comment = retrievedPost.Comments[0];
            AssertGoodId(comment.CommentId);
            Assert.AreEqual("my comment", comment.CommentText);
            Assert.AreEqual(user.UserId, comment.Author.UserId);
            Assert.AreEqual(user.UserName, comment.Author.UserName);
        }

        private static T ExpectOk<T>(Func<ServiceResult<T>> action)
        {
            var result = action();
            Assert.IsTrue(result.Ok, "Error: {0}", result.ServiceError);
            return result.Payload;
        }

        private static void ExpectFail<T>(ServiceError expectedServiceError, Func<ServiceResult<T>> action)
        {
            var result = action();
            Assert.IsFalse(result.Ok);
            Assert.AreEqual(expectedServiceError, result.ServiceError);
        }

        private static void AssertGoodId(int id)
        {
            Assert.AreNotEqual(0, id);
        }

        private static void AssertNotEmptyString(string s)
        {
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }

        private int CreateUserWithPosts(string userName, int numberOfPosts)
        {
            var user = ExpectOk(() => _service.CreateUser(userName, "qwerty"));
            var session = ExpectOk(() => _service.Authenticate(userName, "qwerty"));

            for (var i = 0; i < numberOfPosts; ++i)
            {
                ExpectOk(() => _service.CreatePost(session.SessionToken, "the post"));
            }
            
            return user.UserId;
        }
    }
}
