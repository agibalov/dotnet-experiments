using System.Collections.Generic;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{
    [Service]
    public class UserService
    {
        public bool UserHasPasswordSet(UserContext context, User user)
        {
            return context.AuthenticationMethods
                .OfType<PasswordAuthenticationMethod>()
                .Any(am => am.UserId == user.UserId);
        }

        public IList<PasswordAuthenticationMethod> UserGetPasswordAuthenticationMethods(UserContext context, User user)
        {
            return context.AuthenticationMethods
                .OfType<PasswordAuthenticationMethod>()
                .Where(am => am.UserId == user.UserId)
                .ToList();
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

        public void UserAddPasswordAuthenticationMethod(UserContext context, User user, string password)
        {
            var passwordAuthMethod = new PasswordAuthenticationMethod
                {
                    Password = password,
                    User = user
                };
            context.AuthenticationMethods.Add(passwordAuthMethod);
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

        public void UserAddTwitterAuthenticationMethod(UserContext context, User user, string twitterUserId, string twitterDisplayName)
        {
            var twitterAuthMethod = new TwitterAuthenticationMethod
                {
                    TwitterUserId = twitterUserId,
                    TwitterDisplayName = twitterDisplayName,
                    User = user
                };
            context.AuthenticationMethods.Add(twitterAuthMethod);
        }
    }
}