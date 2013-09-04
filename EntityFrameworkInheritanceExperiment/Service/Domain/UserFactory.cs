using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{
    [Service]
    public class UserFactory
    {
        public DDDUser MakeUser(UserContext context)
        {
            var user = new User();
            context.Users.Add(user);
            return new DDDUser(context, user);
        }
    }
}