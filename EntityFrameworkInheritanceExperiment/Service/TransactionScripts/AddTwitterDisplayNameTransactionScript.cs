using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddTwitterDisplayNameTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public AddTwitterDisplayNameTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper, 
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO AddTwitterDisplayName(UserContext context, int userId, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisTwitterDisplayName = _userRepository.FindUserByTwitterUserId(context, twitterUserId);
            if (somebodyWhoAlreadyHasThisTwitterDisplayName != null)
            {
                if (somebodyWhoAlreadyHasThisTwitterDisplayName.UserId != user.UserId)
                {
                    throw new TwitterUserIdAlreadyUsedException();
                }
            }
            else
            {
                _userService.UserAddTwitterAuthenticationMethod(context, user, twitterUserId, twitterDisplayName);
            }

            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}