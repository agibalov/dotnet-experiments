using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service.Configuration;
using EntityFrameworkInheritanceExperiment.Service.Domain;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class GetAllUsersTransactionScript
    {
        public IList<UserDTO> GetAllUsers(UserContext context)
        {
            return context.Users
                .Select(u => u)
                .Include(u => u.AuthenticationMethods)
                .Include(u => u.EmailAddresses)
                .ToList()
                .Select(u => new DDDUser(context, u).AsUserDTO()) // TODO: dirty hack, get rid ASAP
                .ToList();
        }
    }
}