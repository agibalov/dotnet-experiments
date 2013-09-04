using System;
using System.Data.Entity;
using System.Linq;
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
    public class SignInWithEmailAndPasswordTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public SignInWithEmailAndPasswordTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserRepository userRepository,
            UserService userService)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userRepository = userRepository;
            _userService = userService;
        }

        public UserDTO SignInWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userRepository.FindUserByEmail(context, email);
            if(user == null)
            {
                throw new EmailNotRegisteredException();
            }

            var passwordAuthMethods = _userService.UserGetPasswordAuthenticationMethods(context, user);
            var passwordIsOk = passwordAuthMethods.Any(am => am.Password == password);
            if (!passwordIsOk)
            {
                throw new IncorrectPasswordException();
            }
            
            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}