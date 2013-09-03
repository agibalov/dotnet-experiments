using System;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class AddEmailAndPasswordTransactionScript
    {
        public UserDTO AddEmailAndPassword(UserContext context, int userId, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}