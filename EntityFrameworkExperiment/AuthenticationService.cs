using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.Exceptions;

namespace EntityFrameworkExperiment
{
    public class AuthenticationService
    {
        public void MakeSureSessionTokenIsOk(BlogContext context, string sessionToken)
        {
            GetUserBySessionToken(context, sessionToken);
        }

        public User GetUserBySessionToken(BlogContext context, string sessionToken)
        {
            var session = context.Sessions
                .Include("User")
                .SingleOrDefault(s => s.SessionToken == sessionToken);
            if (session == null)
            {
                throw new InvalidSessionException();
            }

            return session.User;
        }
    }
}