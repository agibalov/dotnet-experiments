using System.Linq;
using EntityFrameworkInheritanceExperiment.DAL;
using EntityFrameworkInheritanceExperiment.Service.Configuration;

namespace EntityFrameworkInheritanceExperiment.Service.TransactionScripts
{
    [TransactionScript]
    public class GetUserCountTransactionScript
    {
        public int GetUserCount(UsersContext context)
        {
            var userCount = context.Users.Count();
            return userCount;
        }
    }
}