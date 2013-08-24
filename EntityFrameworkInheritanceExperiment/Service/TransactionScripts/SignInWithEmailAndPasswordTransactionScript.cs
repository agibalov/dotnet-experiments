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

        public SignInWithEmailAndPasswordTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public UserDTO SignInWithEmailAndPassword(UsersContext context, string email, string password)
        {
            var authenticationMethod = context.AuthenticationMethods
                .OfType<EmailAuthenticationMethod>()
                .Include("User")
                .SingleOrDefault(am => am.Email == email);
            if (authenticationMethod == null)
            {
                throw new EmailNotRegisteredException();
            }

            if (authenticationMethod.Password != password)
            {
                throw new IncorrectPasswordException();
            }

            var user = authenticationMethod.User;
            
            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}