using System.Data.Entity;
using EntityFrameworkInheritanceExperiment.DAL.Entities;

namespace EntityFrameworkInheritanceExperiment.DAL
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationMethod> AuthenticationMethods { get; set; }
        public DbSet<EmailPasswordAuthenticationMethod> EmailPasswordAuthenticationMethods { get; set; }
        public DbSet<GoogleAuthenticationMethod> GoogleAuthenticationMethods { get; set; }
        public DbSet<FacebookAuthenticationMethod> FacebookAuthenticationMethods { get; set; }
        public DbSet<TwitterAuthenticationMethod> TwitterAuthenticationMethods { get; set; }

        public UsersContext(string connectionStringName)
            : base(connectionStringName)
        {
        }
    }
}
