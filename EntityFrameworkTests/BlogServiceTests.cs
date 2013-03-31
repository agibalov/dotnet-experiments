using System;
using EntityFrameworkExperiment;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            _service = new BloggingService();
        }

        [TestMethod]
        public void CanCreateUser()
        {
            var user = ExpectOk(() => _service.CreateUser("loki2302", "qwerty"));
            Assert.AreNotEqual(0, user.UserId);
            Assert.IsFalse(string.IsNullOrEmpty(user.UserName));
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
            Assert.IsFalse(string.IsNullOrEmpty(session.SessionToken));
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
            Assert.AreNotEqual(0, post.PostId);
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
    }
}
