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