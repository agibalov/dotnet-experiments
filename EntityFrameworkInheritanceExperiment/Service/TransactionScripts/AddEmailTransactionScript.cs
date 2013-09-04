using System;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddEmailTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public AddEmailTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper, 
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO AddEmail(UserContext context, int userId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(context, email);
            if (somebodyWhoAlreadyHasThisEmail != null)
            {
                if (somebodyWhoAlreadyHasThisEmail.UserId != user.UserId)
                {
                    throw new EmailAlreadyUsedException();
                }
            }

            _userService.UserAddEmailAddress(context, user, email);
            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}