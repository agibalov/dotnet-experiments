using System;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class DeleteAuthenticationMethodTransactionScript
    {
        public UserDTO DeleteAuthenticationMethod(UserContext context, int userId, int authenticationMethodId)
        {
            throw new NotImplementedException();
        }
    }
}