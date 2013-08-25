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
    public class AuthenticateWithTwitterDisplayNameTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public AuthenticateWithTwitterDisplayNameTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public UserDTO AuthenticateWithTwitter(UserContext context, string twitterDisplayName)
        {
            var twitterAuthMethod = context.AuthenticationMethods
                .OfType<TwitterAuthenticationMethod>()
                .Include(am => am.User)
                .SingleOrDefault(x => x.TwitterDisplayName == twitterDisplayName);

            User user;
            if (twitterAuthMethod != null)
            {
                user = twitterAuthMethod.User;
            }
            else
            {
                user = new User();
                context.Users.Add(user);

                twitterAuthMethod = new TwitterAuthenticationMethod
                    {
                        TwitterDisplayName = twitterDisplayName,
                        User = user
                    };
                context.AuthenticationMethods.Add(twitterAuthMethod);

                context.SaveChanges();
            }

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}