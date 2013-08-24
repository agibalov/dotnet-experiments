using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithGoogleUserIdTransactionScript
    {
        public UserDTO AuthenticateWithGoogleUserId(string googleUserId, string email)
        {
            throw new NotImplementedException();
        }
    }
}