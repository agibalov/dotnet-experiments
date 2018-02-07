using System.Data.Entity.ModelConfiguration;

namespace EFCodeFirstMigrationsExperiment.V2
{
    public class PostV2Configuration : EntityTypeConfiguration<PostV2>
    {
        public PostV2Configuration()
        {
            ToTable("Posts");
            HasKey(x => x.PostId);
        }
    }
}