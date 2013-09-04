using System.Collections.Generic;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class GetAllUsersTransactionScript
    {
        private readonly UserRepository _userRepository;

        public GetAllUsersTransactionScript(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IList<UserDTO> GetAllUsers(UserContext context)
        {
            var users = _userRepository.GetAllUsers(context);
            return users.Select(u => u.AsUserDTO()).ToList();
        }
    }
}