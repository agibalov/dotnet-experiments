using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class SignUpWithEmailAndPasswordTransactionScript
    {
        private readonly UserFactory _userFactory;
        private readonly UserRepository _userRepository;

        public SignUpWithEmailAndPasswordTransactionScript(
            UserFactory userFactory,
            UserRepository userRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public UserDTO SignUpWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userRepository.FindUserByEmail(context, email);
            if (user != null)
            {
                var userHasPasswordSet = user.UserHasPasswordSet();
                if (userHasPasswordSet)
                {
                    throw new EmailAlreadyUsedException();
                }
                
                user.UserAddPasswordAuthenticationMethod(password);
            }
            else
            {
                user = _userFactory.MakeUser(context);
                user.UserAddEmailAddress(email);
                user.UserAddPasswordAuthenticationMethod(password);
            }

            context.SaveChanges();

            return user.AsUserDTO();
        }
    }
}