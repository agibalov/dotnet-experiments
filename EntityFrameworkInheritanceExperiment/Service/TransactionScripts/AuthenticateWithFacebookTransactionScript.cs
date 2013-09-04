using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithFacebookTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public AuthenticateWithFacebookTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO AuthenticateWithFacebook(UserContext context, string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByFacebookUserId(context, facebookUserId);
            if (user == null)
            {
                user = _userRepository.FindUserByEmail(context, email);
                if (user == null)
                {
                    user = new User();
                    context.Users.Add(user);

                    _userService.UserAddEmailAddress(context, user, email);
                    _userService.UserAddFacebookAuthenticationMethod(context, user, facebookUserId);

                    context.SaveChanges();
                }
                else
                {
                    _userService.UserAddFacebookAuthenticationMethod(context, user, facebookUserId);
                    context.SaveChanges();
                }
            }
            else
            {
                var isNewEmailAddress = !context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    _userService.UserAddEmailAddress(context, user, email);
                    context.SaveChanges();
                }
            }
            
            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}