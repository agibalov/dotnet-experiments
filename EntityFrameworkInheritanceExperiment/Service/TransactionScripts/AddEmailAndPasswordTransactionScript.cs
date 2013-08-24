using System;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddEmailAndPasswordTransactionScript
    {
        public UserDTO AddEmailAndPassword(int userId, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}