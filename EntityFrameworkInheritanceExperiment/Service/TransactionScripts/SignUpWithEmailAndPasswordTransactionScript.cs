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
    public class SignUpWithEmailAndPasswordTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public SignUpWithEmailAndPasswordTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public UserDTO SignUpWithEmailAndPassword(UserContext context, string email, string password)
        {
            var emailAddress = context.EmailAddresses.SingleOrDefault(e => e.Email == email);
            if (emailAddress != null)
            {
                throw new EmailAlreadyUsedException();
            }

            var user = new User();
            context.Users.Add(user);

            var authenticationMethod = new PasswordAuthenticationMethod
                {
                    Password = password,
                    User = user
                };
            context.AuthenticationMethods.Add(authenticationMethod);

            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(authenticationMethod.User);
        }
    }
}