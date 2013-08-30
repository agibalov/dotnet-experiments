using System.Data.Entity;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [Service]
    public class UserManager
    {
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

        public void UserAddEmailAddress(UserContext context, User user, string email)
        {
            var emailAddress = new EmailAddress
            {
                Email = email,
                User = user
            };
            context.EmailAddresses.Add(emailAddress);
        }

        public void UserAddGoogleAuthenticationMethod(UserContext context, User user, string googleUserId)
        {
            var googleAuthMethod = new GoogleAuthenticationMethod
            {
                GoogleUserId = googleUserId,
                User = user
            };
            context.AuthenticationMethods.Add(googleAuthMethod);
        }

        public void UserAddFacebookAuthenticationMethod(UserContext context, User user, string facebookUserId)
        {
            var facebookAuthMethod = new FacebookAuthenticationMethod
            {
                FacebookUserId = facebookUserId,
                User = user
            };
            context.AuthenticationMethods.Add(facebookAuthMethod);
        }
    }
}