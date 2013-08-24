using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AuthenticateWithFacebookTransactionScript
    {
        public UserDTO AuthenticateWithFacebook(string facebookUserId, string email)
        {
            throw new NotImplementedException();
        }
    }
}