using System;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Mappers;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class GetUserTransactionScript
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public GetUserTransactionScript(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public UserDTO GetUser(UserContext context, int userId)
        {
            throw new NotImplementedException();
        }
    }
}