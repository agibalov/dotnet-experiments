using System;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class GetUserTransactionScript
    {
        public UserDTO GetUser(UserContext context, int userId)
        {
            throw new NotImplementedException();
        }
    }
}