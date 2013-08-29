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

                    _userManager.UserAddEmailAddress(context, user, email);
                    _userManager.UserAddFacebookAuthenticationMethod(context, user, facebookUserId);

                    context.SaveChanges();
                }
                else
                {
                    // known email - associate facebookuserid with existing user
                    _userManager.UserAddFacebookAuthenticationMethod(context, user, facebookUserId);
                    context.SaveChanges();
                }
            }
            else
            {
                // known facebookuserid
                var isNewEmailAddress = !context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    // new email address - associate with existing user
                    _userManager.UserAddEmailAddress(context, user, email);
                    context.SaveChanges();
                }
            }
            
            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}