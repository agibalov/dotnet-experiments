using System;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithFacebookTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserManager _userManager;

        public AuthenticateWithFacebookTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserManager userManager)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userManager = userManager;
        }

        public UserDTO AuthenticateWithFacebook(UserContext context, string facebookUserId, string email)
        {
            var user = _userManager.FindUserByFacebookUserId(context, facebookUserId);
            if (user == null)
            {
                user = _userManager.FindUserByEmail(context, email);
                if (user == null)
                {
                    // brand new user
                    user = new User();
                    context.Users.Add(user);

                    var emailAddress = new EmailAddress
                    {
                        Email = email,
                        User = user
                    };
                    context.EmailAddresses.Add(emailAddress);

                    var facebookAuthMethod = new FacebookAuthenticationMethod
                    {
                        FacebookUserId = facebookUserId,
                        User = user
                    };
                    context.AuthenticationMethods.Add(facebookAuthMethod);

                    context.SaveChanges();
                }
                else
                {
                    // known email - associate facebookuserid with existing user
                    var facebookAuthMethod = new FacebookAuthenticationMethod
                    {
                        FacebookUserId = facebookUserId,
                        User = user
                    };
                    context.AuthenticationMethods.Add(facebookAuthMethod);

                    context.SaveChanges();
                }
            }
            else
            {
                // known facebookuserid
                var isKnownEmailAddress = context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isKnownEmailAddress)
                {
                    // known email address - do nothing
                }
                else
                {
                    // new email address - associate with existing user
                    var emailAddress = new EmailAddress
                    {
                        Email = email,
                        User = user
                    };
                    context.EmailAddresses.Add(emailAddress);

                    context.SaveChanges();
                }
            }
            
            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}