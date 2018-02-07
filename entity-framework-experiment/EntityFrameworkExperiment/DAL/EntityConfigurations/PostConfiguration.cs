using System.Data.Entity.ModelConfiguration;
using EntityFrameworkExperiment.DAL.Entities;

namespace EntityFrameworkExperiment.DAL.EntityConfigurations
{
    public class PostConfiguration : EntityTypeConfiguration<Post>
    {
        public PostConfiguration()
        {
            ToTable("Posts");
            HasKey(post => post.PostId);
            HasRequired(post => post.User);
            
            HasMany(post => post.Comments)
                .WithRequired(comment => comment.Post)
                .WillCascadeOnDelete(false);
        }
    }
}