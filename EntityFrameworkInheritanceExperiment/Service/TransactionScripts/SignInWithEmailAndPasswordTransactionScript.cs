using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class SignInWithEmailAndPasswordTransactionScript
    {
        private readonly UserRepository _userRepository;

        public SignInWithEmailAndPasswordTransactionScript(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO SignInWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userRepository.FindUserByEmail(context, email);
            if(user == null)
            {
                throw new EmailNotRegisteredException();
            }

            var passwordAuthMethods = user.GetPasswords();
            var passwordIsOk = passwordAuthMethods.Any(am => am.Password == password);
            if (!passwordIsOk)
            {
                throw new IncorrectPasswordException();
            }

            return user.AsUserDTO();
        }
    }
}