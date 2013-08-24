using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithTwitterDisplayNameTransactionScript
    {
        public UserDTO AuthenticateWithTwitter(string twitterDisplayName)
        {
            throw new NotImplementedException();
        }
    }
}