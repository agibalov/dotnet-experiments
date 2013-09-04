using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithTwitterTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public AuthenticateWithTwitterTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO AuthenticateWithTwitter(UserContext context, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByTwitterUserId(context, twitterUserId);
            if (user == null)
            {
                user = new User();
                context.Users.Add(user);

                _userService.UserAddTwitterAuthenticationMethod(context, user, twitterUserId, twitterDisplayName);
            }

            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}