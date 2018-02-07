using System.Data.Entity;

namespace EFCodeFirstMigrationsExperiment.V1
{
    public class MyContextV1 : DbContext
    {
        public DbSet<PostV1> Posts { get; set; }

        public MyContextV1()
            : base("name=MyConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PostV1Configuration());
        }
    }
}