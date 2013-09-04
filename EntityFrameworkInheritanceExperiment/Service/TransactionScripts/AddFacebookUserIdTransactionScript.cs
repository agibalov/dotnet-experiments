using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddFacebookUserIdTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public AddFacebookUserIdTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper, 
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO AddFacebookUserId(UserContext context, int userId, string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisFacebookUserId = _userRepository.FindUserByFacebookUserId(context, facebookUserId);
            if (somebodyWhoAlreadyHasThisFacebookUserId != null)
            {
                if (somebodyWhoAlreadyHasThisFacebookUserId.UserId != user.UserId)
                {
                    throw new FacebookUserIdAlreadyUsedException();
                }
            }
            else
            {
                _userService.UserAddFacebookAuthenticationMethod(context, user, facebookUserId);
            }

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(context, email);
            if (somebodyWhoAlreadyHasThisEmail != null)
            {
                if (somebodyWhoAlreadyHasThisEmail.UserId != user.UserId)
                {
                    throw new EmailAlreadyUsedException();
                }
            }
            else
            {
                _userService.UserAddEmailAddress(context, user, email);
            }

            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}