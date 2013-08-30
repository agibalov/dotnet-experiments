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
        private readonly UserManager _userManager;

        public SignUpWithEmailAndPasswordTransactionScript(
            UserToUserDTOMapper userToUserDtoMapper,
            UserManager userManager)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _userManager = userManager;
        }

        public UserDTO SignUpWithEmailAndPassword(UserContext context, string email, string password)
        {
            var user = _userManager.FindUserByEmail(context, email);
            if (user != null)
            {
                var userHasPasswordSet = _userManager.UserHasPasswordSet(context, user);
                if (userHasPasswordSet)
                {
                    throw new EmailAlreadyUsedException();
                }
                
                _userManager.UserAddPasswordAuthenticationMethod(context, user, password);
            }
            else
            {
                user = new User();
                context.Users.Add(user);

                _userManager.UserAddEmailAddress(context, user, email);
                _userManager.UserAddPasswordAuthenticationMethod(context, user, password);
            }

            context.SaveChanges();

            return _userToUserDtoMapper.MapUserToUserDTO(user);
        }
    }
}