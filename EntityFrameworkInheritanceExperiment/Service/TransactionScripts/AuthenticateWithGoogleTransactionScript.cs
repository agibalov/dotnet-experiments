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
    public class AuthenticateWithGoogleTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public AuthenticateWithGoogleTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public UserDTO AuthenticateWithGoogle(UserContext context, string googleUserId, string email)
        {
            var googleAuthMethod = context.AuthenticationMethods
                .OfType<GoogleAuthenticationMethod>()
                .Include(am => am.User)
                .SingleOrDefault(x => x.GoogleUserId == googleUserId);

            User user;
            if (googleAuthMethod != null)
            {
                user = googleAuthMethod.User;
            }
            else
            {
                user = new User();
                context.Users.Add(user);

                googleAuthMethod = new GoogleAuthenticationMethod
                    {
                        GoogleUserId = googleUserId,
                        Email = email,
                        User = user
                    };
                context.AuthenticationMethods.Add(googleAuthMethod);

                context.SaveChanges();
            }

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}