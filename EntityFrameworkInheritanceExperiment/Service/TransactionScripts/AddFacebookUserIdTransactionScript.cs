using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddFacebookUserIdTransactionScript
    {
        public UserDTO AddFacebookUserId(int userId, string facebookUserId, string email)
        {
            throw new NotImplementedException();
        }
    }
}