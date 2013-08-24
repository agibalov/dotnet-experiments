using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddTwitterDisplayNameTransactionScript
    {
        public UserDTO AddTwitterDisplayName(int userId, string twitterDisplayName)
        {
            throw new NotImplementedException();
        }
    }
}