using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.Domain
{
    [Service]
    public class UserService
    {
        private readonly UserFactory _userFactory;
        private readonly UserRepository _userRepository;

        public UserService(UserFactory userFactory, UserRepository userRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        // TODO: put all transaction scripts here, make them return DDDUser instead of UserDTO
    }
}