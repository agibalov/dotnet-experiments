using System;
using System.Data.Entity;
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

        public AuthenticateWithFacebookTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public UserDTO AuthenticateWithFacebook(UserContext context, string facebookUserId, string email)
        {
            var facebookAuthMethod = context.AuthenticationMethods
                .OfType<FacebookAuthenticationMethod>()
                .Include(am => am.User)
                .SingleOrDefault(x => x.FacebookUserId == facebookUserId);

            User user;
            if (facebookAuthMethod != null)
            {
                user = facebookAuthMethod.User;
            }
            else
            {
                user = new User();
                context.Users.Add(user);

                facebookAuthMethod = new FacebookAuthenticationMethod
                    {
                        FacebookUserId = facebookUserId,
                        Email = email,
                        User = user
                    };
                context.AuthenticationMethods.Add(facebookAuthMethod);

                context.SaveChanges();
            }

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}