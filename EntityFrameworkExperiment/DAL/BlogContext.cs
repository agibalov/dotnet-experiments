using System.Data.Entity;
using EntityFrameworkExperiment.DAL.Entities;

namespace EntityFrameworkExperiment.DAL
{
    public class BlogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}