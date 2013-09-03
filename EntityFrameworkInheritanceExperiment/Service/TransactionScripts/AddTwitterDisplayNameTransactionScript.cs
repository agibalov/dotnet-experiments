using System;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddTwitterDisplayNameTransactionScript
    {
        public UserDTO AddTwitterDisplayName(UserContext context, int userId, string twitterDisplayName)
        {
            throw new NotImplementedException();
        }
    }
}