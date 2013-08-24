using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class DeleteAuthenticationMethodTransactionScript
    {
        public UserDTO DeleteAuthenticationMethod(int userId, int authenticationMethodId)
        {
            throw new NotImplementedException();
        }
    }
}