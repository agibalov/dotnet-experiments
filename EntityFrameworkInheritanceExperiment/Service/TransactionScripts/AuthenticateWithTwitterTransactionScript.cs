using System.Collections.Generic;
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
    public class AuthenticateWithTwitterTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserManager _userManager;

        public AuthenticateWithTwitterTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserManager userManager)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userManager = userManager;
        }

        public UserDTO AuthenticateWithTwitter(UserContext context, string twitterUserId, string twitterDisplayName)
        {
            var user = _userManager.FindUserByTwitterUserId(context, twitterUserId);
            if (user == null)
            {
                user = new User();
                context.Users.Add(user);

                _userManager.UserAddTwitterAuthenticationMethod(context, user, twitterUserId, twitterDisplayName);
            }

            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}