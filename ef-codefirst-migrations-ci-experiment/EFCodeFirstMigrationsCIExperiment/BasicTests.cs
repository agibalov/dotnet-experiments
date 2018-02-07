using System;
using System.Linq;
using NUnit.Framework;

namespace EFCodeFirstMigrationsCIExperiment
{
    [TestFixture]
    public class BasicTests
    {
        [Test]
        public void CanCreateNewUser()
        {
            int originalUserCount;
            using (var context = new MyContext())
            {
                originalUserCount = context.Users.Count();
            }

            using (var context = new MyContext())
            {
                var user = new User
                    {
                        UserName = string.Format("User #{0}", Guid.NewGuid().ToString()),
                        FirstName = "loki",
                        LastName = "2302"
                    };
                context.Users.Add(user);
                context.SaveChanges();
            }

            int newUserCount;
            using (var context = new MyContext())
            {
                newUserCount = context.Users.Count();
            }

            Assert.AreEqual(originalUserCount + 1, newUserCount);
        }
    }
}