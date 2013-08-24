using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddGoogleUserIdTransactionScript
    {
        public UserDTO AddGoogleUserId(int userId, string googleUserId, string email)
        {
            throw new NotImplementedException();
        }
    }
}