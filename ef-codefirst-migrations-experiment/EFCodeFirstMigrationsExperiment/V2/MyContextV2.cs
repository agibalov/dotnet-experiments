using System.Data.Entity;

namespace EFCodeFirstMigrationsExperiment.V2
{
    public class MyContextV2 : DbContext
    {
        public DbSet<PostV2> Posts { get; set; }

        public MyContextV2()
            : base("name=MyConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PostV2Configuration());
        }
    }
}