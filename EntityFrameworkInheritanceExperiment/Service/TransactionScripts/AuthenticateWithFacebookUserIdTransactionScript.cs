using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithFacebookUserIdTransactionScript
    {
        public UserDTO AuthenticateWithFacebookUserId(string facebookUserId, string email)
        {
            throw new NotImplementedException();
        }
    }
}