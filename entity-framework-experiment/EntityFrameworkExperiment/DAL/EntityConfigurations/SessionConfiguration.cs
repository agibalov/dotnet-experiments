using System.Data.Entity.ModelConfiguration;
using EntityFrameworkExperiment.DAL.Entities;

namespace EntityFrameworkExperiment.DAL.EntityConfigurations
{
    public class SessionConfiguration : EntityTypeConfiguration<Session>
    {
        public SessionConfiguration()
        {
            ToTable("Sessions");
            HasKey(session => session.SessionId);
            HasRequired(session => session.User);
        }
    }
}