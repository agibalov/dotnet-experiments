using System.Data.Entity;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{
    [Service]
    public class UserRepository
    {
        public User FindUserByIdOrThrow(UserContext context, int userId)
        {
            var user = context.Users
                .Include(u => u.AuthenticationMethods)
                .Include(u => u.EmailAddresses)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new NoSuchUserException();
            }

            return user;
        }

        public User FindUserByEmail(UserContext context, string email)
        {
            var emailAddress = context.EmailAddresses
                                      .Include(e => e.User)
                                      .SingleOrDefault(x => x.Email == email);
            if (emailAddress == null)
            {
                return null;
            }

            return emailAddress.User;
        }

        public User FindUserByGoogleUserId(UserContext context, string googleUserId)
        {
            var googleAuthMethod = context.AuthenticationMethods
                                          .OfType<GoogleAuthenticationMethod>()
                                          .Include(am => am.User)
                                          .SingleOrDefault(x => x.GoogleUserId == googleUserId);
            if (googleAuthMethod == null)
            {
                return null;
            }

            return googleAuthMethod.User;
        }

        public User FindUserByFacebookUserId(UserContext context, string facebookUserId)
        {
            var facebookAuthMethod = context.AuthenticationMethods
                                            .OfType<FacebookAuthenticationMethod>()
                                            .Include(am => am.User)
                                            .SingleOrDefault(x => x.FacebookUserId == facebookUserId);
            if (facebookAuthMethod == null)
            {
                return null;
            }

            return facebookAuthMethod.User;
        }

        public User FindUserByTwitterUserId(UserContext context, string twitterUserId)
        {
            var twitterAuthMethod = context.AuthenticationMethods
                                           .OfType<TwitterAuthenticationMethod>()
                                           .Include(am => am.User)
                                           .SingleOrDefault(x => x.TwitterUserId == twitterUserId);
            if (twitterAuthMethod == null)
            {
                return null;
            }

            return twitterAuthMethod.User;
        }
    }
}