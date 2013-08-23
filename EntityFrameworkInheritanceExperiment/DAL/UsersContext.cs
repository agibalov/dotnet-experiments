using System.Data.Entity;
using EntityFrameworkInheritanceExperiment.DAL.Entities;

namespace EntityFrameworkInheritanceExperiment.DAL
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationMethod> AuthenticationMethods { get; set; }

        public UsersContext(string connectionStringName)
            : base(connectionStringName)
        {
        }
    }
}
