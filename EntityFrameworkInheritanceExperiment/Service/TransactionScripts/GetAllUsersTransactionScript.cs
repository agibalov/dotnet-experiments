using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class GetAllUsersTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public GetAllUsersTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public IList<UserDTO> GetAllUsers(UserContext context)
        {
            return context.Users
                .Select(u => u)
                .Include(u => u.AuthenticationMethods)
                .Include(u => u.EmailAddresses)
                .Select(_userToUserDtoMapper.MapUserToUserDTO)
                .ToList();
        }
    }
}