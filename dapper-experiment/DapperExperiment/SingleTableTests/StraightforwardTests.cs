using NUnit.Framework;

namespace DapperExperiment.SingleTableTests
{
    public class StraightforwardTests : AbstractSqlCeTest
    {
        private UserDAO _dao;

        [SetUp]
        public void ConstructUserDAO()
        {
            _dao = new UserDAO(SqlCeDatabaseHelper);
            _dao.CreateSchema();
        }

        [Test]
        public void ThereAreNoUsersByDefault()
        {
            var userCount = _dao.GetUserCount();
            Assert.AreEqual(0, userCount);
        }

        [Test]
        public void CanCreateUser()
        {
            var user = _dao.CreateUser("loki2302");
            Assert.AreEqual(1, user.UserId);
            Assert.AreEqual("loki2302", user.UserName);
        }

        [Test]
        public void CanFetchSingleUser()
        {
            var user = _dao.CreateUser("loki2302");
            var fetchedUser = _dao.GetUser(user.UserId);
            Assert.AreEqual(user.UserId, fetchedUser.UserId);
            Assert.AreEqual(user.UserName, fetchedUser.UserName);
        }

        [Test]
        public void CanFetchMultipleUsers()
        {
            var user1 = _dao.CreateUser("loki2302_1");
            _dao.CreateUser("loki2302_2");
            var user3 = _dao.CreateUser("loki2302_3");

            var users = _dao.GetUsers(new[] { user1.UserId, user3.UserId });
            Assert.AreEqual(2, users.Count);
            
            var fetchedUser1 = users[0];
            Assert.AreEqual(user1.UserId, fetchedUser1.UserId);
            Assert.AreEqual(user1.UserName, fetchedUser1.UserName);

            var fetchedUser2 = users[1];
            Assert.AreEqual(user3.UserId, fetchedUser2.UserId);
            Assert.AreEqual(user3.UserName, fetchedUser2.UserName);
        }

        [Test]
        public void CanDeleteSingleUser()
        {
            _dao.CreateUser("loki2302_1");
            var userToDelete = _dao.CreateUser("loki2302_2");
            _dao.CreateUser("loki2302_3");

            var userCountBeforeDelete = _dao.GetUserCount();
            Assert.AreEqual(3, userCountBeforeDelete);

            _dao.DeleteUser(userToDelete.UserId);

            var userCountAfterDelete = _dao.GetUserCount();
            Assert.AreEqual(2, userCountAfterDelete);
        }

        [Test]
        public void CanDeleteMultipleUsers()
        {
            _dao.CreateUser("loki2302_1");
            var userToDelete1 = _dao.CreateUser("loki2302_2");
            var userToDelete2 = _dao.CreateUser("loki2302_3");

            var userCountBeforeDelete = _dao.GetUserCount();
            Assert.AreEqual(3, userCountBeforeDelete);

            _dao.DeleteUsers(new[]{ userToDelete1.UserId, userToDelete2.UserId });

            var userCountAfterDelete = _dao.GetUserCount();
            Assert.AreEqual(1, userCountAfterDelete);
        }

        [Test]
        public void CanChangeUserName()
        {
            var user = _dao.CreateUser("loki2302");
            Assert.AreEqual("loki2302", user.UserName);

            user = _dao.ChangeUserName(user.UserId, "qwerty");
            Assert.AreEqual("qwerty", user.UserName);
        }

        [Test]
        public void CanGetAllUsersWithPagination()
        {
            const int numberOfUsers = 10;
            for (var i = 0; i < numberOfUsers; ++i)
            {
                var userName = string.Format("loki2302_{0}", i);
                _dao.CreateUser(userName);
            }

            var page0 = _dao.GetAllUsers(3, 0);
            Assert.AreEqual(numberOfUsers, page0.NumberOfItems);
            Assert.AreEqual(4, page0.NumberOfPages);
            Assert.AreEqual(0, page0.CurrentPage);
            Assert.AreEqual(3, page0.Items.Count);
            Assert.AreEqual("loki2302_0", page0.Items[0].UserName);
            Assert.AreEqual("loki2302_1", page0.Items[1].UserName);
            Assert.AreEqual("loki2302_2", page0.Items[2].UserName);

            var page1 = _dao.GetAllUsers(3, 1);
            Assert.AreEqual(numberOfUsers, page1.NumberOfItems);
            Assert.AreEqual(4, page1.NumberOfPages);
            Assert.AreEqual(1, page1.CurrentPage);
            Assert.AreEqual(3, page1.Items.Count);
            Assert.AreEqual("loki2302_3", page1.Items[0].UserName);
            Assert.AreEqual("loki2302_4", page1.Items[1].UserName);
            Assert.AreEqual("loki2302_5", page1.Items[2].UserName);

            var page2 = _dao.GetAllUsers(3, 2);
            Assert.AreEqual(numberOfUsers, page2.NumberOfItems);
            Assert.AreEqual(4, page2.NumberOfPages);
            Assert.AreEqual(2, page2.CurrentPage);
            Assert.AreEqual(3, page2.Items.Count);
            Assert.AreEqual("loki2302_6", page2.Items[0].UserName);
            Assert.AreEqual("loki2302_7", page2.Items[1].UserName);
            Assert.AreEqual("loki2302_8", page2.Items[2].UserName);

            var page3 = _dao.GetAllUsers(3, 3);
            Assert.AreEqual(numberOfUsers, page3.NumberOfItems);
            Assert.AreEqual(4, page3.NumberOfPages);
            Assert.AreEqual(3, page3.CurrentPage);
            Assert.AreEqual(1, page3.Items.Count);
            Assert.AreEqual("loki2302_9", page3.Items[0].UserName);
        }
    }
}
