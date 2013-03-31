using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.Exceptions;

namespace EntityFrameworkExperiment
{
    public class AuthenticationService
    {
        public User GetUserBySessionToken(BlogContext context, string sessionToken)
        {
            var session = context.Sessions.SingleOrDefault(s => s.SessionToken == sessionToken);
            if (session == null)
            {
                throw new InvalidSessionException();
            }

            return session.User;
        }
    }
}