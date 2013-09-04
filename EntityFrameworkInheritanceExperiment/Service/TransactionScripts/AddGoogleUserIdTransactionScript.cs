using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddGoogleUserIdTransactionScript
    {
        private readonly UserRepository _userRepository;

        public AddGoogleUserIdTransactionScript(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO AddGoogleUserId(UserContext context, int userId, string googleUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisGoogleUserId = _userRepository.FindUserByGoogleUserId(context, googleUserId);
            if (somebodyWhoAlreadyHasThisGoogleUserId != null)
            {
                if (somebodyWhoAlreadyHasThisGoogleUserId.UserId != user.UserId)
                {
                    throw new GoogleUserIdAlreadyUsedException();
                }
            }
            else
            {
                user.UserAddGoogleAuthenticationMethod(googleUserId);
            }

            var somebodyWhoAlreadyHasThisEmail = _userRepository.FindUserByEmail(context, email);
            if (somebodyWhoAlreadyHasThisEmail != null)
            {
                if (somebodyWhoAlreadyHasThisEmail.UserId != user.UserId)
                {
                    throw new EmailAlreadyUsedException();
                }
            }
            else
            {
                user.UserAddEmailAddress(email);
            }

            context.SaveChanges();

            return user.AsUserDTO();
        }
    }
}