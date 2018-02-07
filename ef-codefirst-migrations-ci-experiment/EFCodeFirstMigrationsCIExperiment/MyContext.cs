using System.Data.Entity;

namespace EFCodeFirstMigrationsCIExperiment
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MyContext()
            : base("name=MyConnectionString")
        {
        }
    }
}