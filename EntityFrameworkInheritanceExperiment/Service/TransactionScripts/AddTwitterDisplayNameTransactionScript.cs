using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddTwitterDisplayNameTransactionScript
    {
        private readonly UserRepository _userRepository;

        public AddTwitterDisplayNameTransactionScript(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO AddTwitterDisplayName(UserContext context, int userId, string twitterUserId, string twitterDisplayName)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisTwitterDisplayName = _userRepository.FindUserByTwitterUserId(context, twitterUserId);
            if (somebodyWhoAlreadyHasThisTwitterDisplayName != null)
            {
                if (somebodyWhoAlreadyHasThisTwitterDisplayName.UserId != user.UserId)
                {
                    throw new TwitterUserIdAlreadyUsedException();
                }
            }
            else
            {
                user.UserAddTwitterAuthenticationMethod(twitterUserId, twitterDisplayName);
            }

            context.SaveChanges();

            return user.AsUserDTO();
        }
    }
}