using System.Data.Entity;
using EntityFrameworkInheritanceExperiment.DAL.Entities;

namespace EntityFrameworkInheritanceExperiment.DAL
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationMethod> AuthenticationMethods { get; set; }

        public UserContext(string connectionStringName)
            : base(connectionStringName)
        {
        }
    }
}
