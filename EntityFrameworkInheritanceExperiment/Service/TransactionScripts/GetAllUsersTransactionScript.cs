using System;
using System.Collections.Generic;
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

        public IList<UserDTO> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}