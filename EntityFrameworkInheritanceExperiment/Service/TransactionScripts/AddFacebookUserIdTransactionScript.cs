using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddFacebookUserIdTransactionScript
    {
        private readonly UserRepository _userRepository;

        public AddFacebookUserIdTransactionScript(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO AddFacebookUserId(UserContext context, int userId, string facebookUserId, string email)
        {
            var user = _userRepository.FindUserByIdOrThrow(context, userId);

            var somebodyWhoAlreadyHasThisFacebookUserId = _userRepository.FindUserByFacebookUserId(context, facebookUserId);
            if (somebodyWhoAlreadyHasThisFacebookUserId != null)
            {
                if (somebodyWhoAlreadyHasThisFacebookUserId.UserId != user.UserId)
                {
                    throw new FacebookUserIdAlreadyUsedException();
                }
            }
            else
            {
                user.AddFacebook(facebookUserId);
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
                user.AddEmail(email);
            }

            context.SaveChanges();

            return user.AsUserDTO();
        }
    }
}