using System.Data.Entity;
using EntityFrameworkExperiment2.DAL.Entities;

namespace EntityFrameworkExperiment2.DAL
{
    public class RenoteContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public RenoteContext(string connectionStringName)
            : base(connectionStringName)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public RenoteContext()
            : this("RenoteConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                        .HasMany(x => x.Tags)
                        .WithMany(x => x.Notes)
                        .Map(x => x
                                      .ToTable("NoteTags").MapLeftKey("NoteId").MapRightKey("MemberId"));

            modelBuilder.Entity<Note>()
                        .HasRequired(x => x.User)
                        .WithMany()
                        .WillCascadeOnDelete(false);
        }
    }
}