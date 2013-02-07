using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFrameworkTests
{
    [TestClass]
    public class DummyTests
    {
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
        }

        [TestMethod]
        public void CanCreateBlog()
        {
            using (var context = new BlogContext())
            {
                Assert.AreEqual(0, context.Blogs.Count());

                var newBlog = new Blog { Name = "My New Blog" };
                context.Blogs.Add(newBlog);
                context.SaveChanges();

                var blogsList = (from blog in context.Blogs
                                 select blog).ToList();

                Assert.AreEqual(1, blogsList.Count);
            }
        }
    }
}
