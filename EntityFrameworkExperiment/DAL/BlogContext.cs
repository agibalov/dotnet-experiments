using System.Data.Entity;
using EntityFrameworkExperiment.DAL.Entities;

namespace EntityFrameworkExperiment.DAL
{
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}