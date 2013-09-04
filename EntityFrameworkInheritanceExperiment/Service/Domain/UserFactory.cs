using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{
    public class UserFactory
    {
        private readonly UserContext _context;

        public UserFactory(UserContext context)
        {
            _context = context;
        }

        public DDDUser MakeUser()
        {
            var user = new User();
            _context.Users.Add(user);
            return new DDDUser(_context, user);
        }
    }
}