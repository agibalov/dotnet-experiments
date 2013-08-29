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

        public UserDTO SignInWithEmailAndPassword(UserContext context, string email, string password)
        {
            var emailAddress = context.EmailAddresses
                .SingleOrDefault(e => e.Email == email);
            if (emailAddress == null)
            {
                throw new EmailNotRegisteredException();
            }

            var user = emailAddress.User;
            var passwordAuthenticationMethod = context.AuthenticationMethods
                .OfType<PasswordAuthenticationMethod>()
                .SingleOrDefault(p => p.UserId == user.UserId && p.Password == password);
            if (passwordAuthenticationMethod == null)
            {
                throw new IncorrectPasswordException();
            }
            
            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}