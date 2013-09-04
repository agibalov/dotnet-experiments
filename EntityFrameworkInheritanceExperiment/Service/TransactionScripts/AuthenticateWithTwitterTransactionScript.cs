using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithTwitterTransactionScript
    {
        private readonly UserFactory _userFactory;
        private readonly UserRepository _userRepository;

        public AuthenticateWithTwitterTransactionScript(
            UserFactory userFactory,
            UserRepository userRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public UserDTO AuthenticateWithTwitter(UserContext context, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByTwitterUserId(context, twitterUserId);
            if (user == null)
            {
                user = _userFactory.MakeUser(context);
                user.AddTwitter(twitterUserId, twitterDisplayName);
            }

            context.SaveChanges();

            return user.AsUserDTO();
        }
    }
}