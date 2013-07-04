using DapperExperiment.MultipleTablesTests.DAL;
using DapperExperiment.MultipleTablesTests.Service;
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
            BlogService = new BlogService(DatabaseHelper, userDao, postDao);
            BlogService.CreateSchema();
        }
    }
}