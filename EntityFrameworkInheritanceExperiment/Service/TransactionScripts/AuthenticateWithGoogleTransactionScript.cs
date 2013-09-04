using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithGoogleTransactionScript
    {
        private readonly UserFactory _userFactory;
        private readonly UserRepository _userRepository;

        public AuthenticateWithGoogleTransactionScript(
            UserFactory userFactory,
            UserRepository userRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public UserDTO AuthenticateWithGoogle(UserContext context, string googleUserId, string email)
        {
            var user = _userRepository.FindUserByGoogleUserId(context, googleUserId);
            if (user == null)
            {
                user = _userRepository.FindUserByEmail(context, email);
                if (user == null)
                {
                    user = _userFactory.MakeUser(context);
                    user.AddEmail(email);
                    user.AddGoogle(googleUserId);
                    context.SaveChanges();
                }
                else
                {
                    user.AddGoogle(googleUserId);
                    context.SaveChanges();
                }
            }
            else
            {
                var isNewEmailAddress = !context.EmailAddresses
                    .Any(e => e.UserId == user.UserId && e.Email == email);
                if (isNewEmailAddress)
                {
                    user.AddEmail(email);
                    context.SaveChanges();
                }
            }

            return user.AsUserDTO();
        }
    }
}