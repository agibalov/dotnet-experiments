using System.Data.Entity.ModelConfiguration;
using EntityFrameworkExperiment.DAL.Entities;

namespace EntityFrameworkExperiment.DAL.EntityConfigurations
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            ToTable("Comments");
            HasKey(comment => comment.CommentId);
            
            HasRequired(comment => comment.User)
                .WithMany(user => user.Comments)
                .WillCascadeOnDelete(false);
            
            HasRequired(comment => comment.Post)
                .WithMany(post => post.Comments)
                .WillCascadeOnDelete(false);
        }
    }
}