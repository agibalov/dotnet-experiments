using System.Data.Entity.ModelConfiguration;

namespace EFCodeFirstMigrationsExperiment.V1
{
    public class PostV1Configuration : EntityTypeConfiguration<PostV1>
    {
        public PostV1Configuration()
        {
            ToTable("Posts");
            HasKey(x => x.PostId);
        }
    }
}