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
    public class AuthenticateWithGoogleTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public AuthenticateWithGoogleTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO AuthenticateWithGoogle(UserContext context, string googleUserId, string email)
        {
            var user = _userRepository.FindUserByGoogleUserId(context, googleUserId);
            if (user == null)
            {
                user = _userRepository.FindUserByEmail(context, email);
                if (user == null)
                {
                    user = new User();
                    context.Users.Add(user);

                    _userService.UserAddEmailAddress(context, user, email);
                    _userService.UserAddGoogleAuthenticationMethod(context, user, googleUserId);

                    context.SaveChanges();
                }
                else
                {
                    _userService.UserAddGoogleAuthenticationMethod(context, user, googleUserId);
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