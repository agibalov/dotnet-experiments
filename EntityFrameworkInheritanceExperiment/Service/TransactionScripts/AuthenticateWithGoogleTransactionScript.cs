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
        private readonly UserManager _userManager;

        public AuthenticateWithGoogleTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserManager userManager)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userManager = userManager;
        }

        public UserDTO AuthenticateWithGoogle(UserContext context, string googleUserId, string email)
        {
            var user = _userManager.FindUserByGoogleUserId(context, googleUserId);
            if (user == null)
            {
                user = _userManager.FindUserByEmail(context, email);
                if (user == null)
                {
                    user = new User();
                    context.Users.Add(user);

                    _userManager.UserAddEmailAddress(context, user, email);
                    _userManager.UserAddGoogleAuthenticationMethod(context, user, googleUserId);

                    context.SaveChanges();
                }
                else
                {
                    _userManager.UserAddGoogleAuthenticationMethod(context, user, googleUserId);
                    context.SaveChanges();
                }
            }
            else
            {
                var isNewEmailAddress = !context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    _userManager.UserAddEmailAddress(context, user, email);
                    context.SaveChanges();
                }
            }

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}