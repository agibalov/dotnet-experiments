using DapperExperiment.MultipleTablesTests.DAL;
using DapperExperiment.MultipleTablesTests.Service.DTO;
using NUnit.Framework;

namespace DapperExperiment.MultipleTablesTests
{
    public class UserTests : AbstractBlogServiceTest
    {
        [Test]
        public void CanCreateUser()
        {
            var user = BlogService.CreateUser("loki2302");
            Assert.AreNotEqual(0, user.UserId);
            Assert.AreEqual("loki2302", user.UserName);
            Assert.NotNull(user.Posts);
            Assert.AreEqual(0, user.Posts.Count);
        }

        [Test]
        public void CanUpdateUser()
        {
            var user = BlogService.CreateUser("loki2302");
            Assert.AreEqual("loki2302", user.UserName);

            var updatedUser = BlogService.UpdateUser(user.UserId, "qwerty");
            Assert.AreEqual(user.UserId, updatedUser.UserId);
            Assert.AreEqual("qwerty", updatedUser.UserName);
        }

        [Test]
        [ExpectedException(typeof(NoSuchUserException))]
        public void CantUpdateUserThatDoesNotExist()
        {
            BlogService.UpdateUser(123, "loki2302");
        }

        [Test]
        public void CanGetUser()
        {
            var user = BlogService.CreateUser("loki2302");
            Assert.AreNotEqual(0, user.UserId);

            var fetchedUser = BlogService.GetUser(user.UserId);
            AssertEqualUsers(user, fetchedUser);
        }

        [Test]
        [ExpectedException(typeof(NoSuchUserException))]
        public void CantGetUserThatDoesNotExist()
        {
            BlogService.GetUser(123);
        }

        [Test]
        public void CanGetMultipleUsers()
        {
            var user1 = BlogService.CreateUser("loki2302");
            var user2 = BlogService.CreateUser("loki2302_2");
            var user3 = BlogService.CreateUser("loki2302_3");

            var users = BlogService.GetUsers(new[] {user1.UserId, user3.UserId});
            AssertEqualUsers(user1, users[0]);
            AssertEqualUsers(user3, users[1]);
        }

        [Test]
        [ExpectedException(typeof(NoSuchUserException))]
        public void CantGetUsersIfAtLeastOneDoesNotExist()
        {
            var user1 = BlogService.CreateUser("loki2302");
            Assert.AreNotEqual(123, user1.UserId);

            var user2 = BlogService.CreateUser("loki2302_2");
            Assert.AreNotEqual(123, user2.UserId);

            BlogService.GetUsers(new[] {user1.UserId, user2.UserId, 123});
        }

        [Test]
        public void CanGetAllUsers()
        {
            var user1 = BlogService.CreateUser("loki2302");
            var user2 = BlogService.CreateUser("loki2302_2");

            var users = BlogService.GetAllUsers();
            AssertEqualUsers(user1, users[0]);
            AssertEqualUsers(user2, users[1]);
        }

        [Test]
        public void CanGetUserCount()
        {
            var userCount = BlogService.GetUserCount();
            Assert.AreEqual(0, userCount);
        }

        [Test]
        public void CanDeleteUser()
        {
            var user1 = BlogService.CreateUser("loki2302");
            var user2 = BlogService.CreateUser("loki2302_2");

            var numberOfUsersBeforeDelete = BlogService.GetUserCount();
            Assert.AreEqual(2, numberOfUsersBeforeDelete);

            BlogService.DeleteUser(user1.UserId);

            var numberOfUsersAfterDelete = BlogService.GetUserCount();
            Assert.AreEqual(1, numberOfUsersAfterDelete);

            var remainingUser = BlogService.GetUser(user2.UserId);
            AssertEqualUsers(user2, remainingUser);
        }

        [Test]
        [ExpectedException(typeof(NoSuchUserException))]
        public void CantDeleteUserThatDoesNotExist()
        {
            BlogService.DeleteUser(123);
        }

        [Test]
        public void CanDeleteMultipleUsers()
        {
            var user1 = BlogService.CreateUser("loki2302");
            var user2 = BlogService.CreateUser("loki2302_2");
            var user3 = BlogService.CreateUser("loki2302_3");

            var numberOfUsersBeforeDelete = BlogService.GetUserCount();
            Assert.AreEqual(3, numberOfUsersBeforeDelete);

            BlogService.DeleteUsers(new[] { user1.UserId, user3.UserId });

            var numberOfUsersAfterDelete = BlogService.GetUserCount();
            Assert.AreEqual(1, numberOfUsersAfterDelete);

            var remainingUser = BlogService.GetUser(user2.UserId);
            AssertEqualUsers(user2, remainingUser);
        }

        [Test]
        [ExpectedException(typeof(NoSuchUserException))]
        public void CantDeleteMultipleUsersIfAtLeastOneDoesNotExist()
        {
            var user1 = BlogService.CreateUser("loki2302");
            Assert.AreNotEqual(123, user1.UserId);

            var user2 = BlogService.CreateUser("loki2302_2");
            Assert.AreNotEqual(123, user2.UserId);

            BlogService.DeleteUsers(new[] { user1.UserId, user2.UserId, 123 });
        }
    }
}