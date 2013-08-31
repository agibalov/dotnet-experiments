using System;
using System.Data.Entity;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DAL.Entities;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class SignInWithEmailAndPasswordTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly UserManager _userManager;

        public SignInWithEmailAndPasswordTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserManager userManager)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userManager = userManager;
        }

        public UserDTO SignInWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userManager.FindUserByEmail(context, email);
            if(user == null)
            {
                throw new EmailNotRegisteredException();
            }

            var passwordAuthMethods = _userManager.UserGetPasswordAuthenticationMethods(context, user);
            var passwordIsOk = passwordAuthMethods.Any(am => am.Password == password);
            if (!passwordIsOk)
            {
                throw new IncorrectPasswordException();
            }
            
            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}