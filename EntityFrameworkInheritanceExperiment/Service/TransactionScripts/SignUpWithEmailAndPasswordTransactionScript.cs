using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class SignUpWithEmailAndPasswordTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public SignUpWithEmailAndPasswordTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO SignUpWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userRepository.FindUserByEmail(context, email);
            if (user != null)
            {
                var userHasPasswordSet = _userService.UserHasPasswordSet(context, user);
                if (userHasPasswordSet)
                {
                    throw new EmailAlreadyUsedException();
                }
                
                _userService.UserAddPasswordAuthenticationMethod(context, user, password);
            }
            else
            {
                user = new User();
                context.Users.Add(user);

                _userService.UserAddEmailAddress(context, user, email);
                _userService.UserAddPasswordAuthenticationMethod(context, user, password);
            }

            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}