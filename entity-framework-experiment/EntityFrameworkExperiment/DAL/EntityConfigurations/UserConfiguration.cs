using System.Data.Entity.ModelConfiguration;
using EntityFrameworkExperiment.DAL.Entities;

namespace EntityFrameworkExperiment.DAL.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            HasKey(user => user.UserId);
            
            HasMany(user => user.Sessions)
                .WithRequired(session => session.User)
                .WillCascadeOnDelete(false);
            
            HasMany(user => user.Posts)
                .WithRequired(post => post.User)
                .WillCascadeOnDelete(false);
            
            HasMany(user => user.Comments)
                .WithRequired(comment => comment.User)
                .WillCascadeOnDelete(false);
        }
    }
}