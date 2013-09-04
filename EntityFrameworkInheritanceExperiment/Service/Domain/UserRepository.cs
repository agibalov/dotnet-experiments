using System.Collections.Generic;
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
        public IList<DDDUser> GetAllUsers(UserContext context)
        {
            var users = context.Users
                .Include(u => u.AuthenticationMethods)
                .Include(u => u.EmailAddresses)
                .ToList();

            return users
                .Select(u => new DDDUser(context, u))
                .ToList();
        }

        public DDDUser FindUserByIdOrThrow(UserContext context, int userId)
        {
            var user = context.Users
                .Include(u => u.AuthenticationMethods)
                .Include(u => u.EmailAddresses)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new NoSuchUserException();
            }

            return new DDDUser(context, user);
        }

        public DDDUser FindUserByEmail(UserContext context, string email)
        {
            var emailAddress = context.EmailAddresses
                                      .Include(e => e.User)
                                      .SingleOrDefault(x => x.Email == email);
            if (emailAddress == null)
            {
                return null;
            }

            return new DDDUser(context, emailAddress.User);
        }

        public DDDUser FindUserByGoogleUserId(UserContext context, string googleUserId)
        {
            var googleAuthMethod = context.AuthenticationMethods
                                          .OfType<GoogleAuthenticationMethod>()
                                          .Include(am => am.User)
                                          .SingleOrDefault(x => x.GoogleUserId == googleUserId);
            if (googleAuthMethod == null)
            {
                return null;
            }

            return new DDDUser(context, googleAuthMethod.User);
        }

        public DDDUser FindUserByFacebookUserId(UserContext context, string facebookUserId)
        {
            var facebookAuthMethod = context.AuthenticationMethods
                                            .OfType<FacebookAuthenticationMethod>()
                                            .Include(am => am.User)
                                            .SingleOrDefault(x => x.FacebookUserId == facebookUserId);
            if (facebookAuthMethod == null)
            {
                return null;
            }

            return new DDDUser(context, facebookAuthMethod.User);
        }

        public DDDUser FindUserByTwitterUserId(UserContext context, string twitterUserId)
        {
            var twitterAuthMethod = context.AuthenticationMethods
                                           .OfType<TwitterAuthenticationMethod>()
                                           .Include(am => am.User)
                                           .SingleOrDefault(x => x.TwitterUserId == twitterUserId);
            if (twitterAuthMethod == null)
            {
                return null;
            }

            return new DDDUser(context, twitterAuthMethod.User);
        }
    }
}