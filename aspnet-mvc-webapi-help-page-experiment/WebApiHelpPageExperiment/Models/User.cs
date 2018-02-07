using System.Collections.Generic;

namespace WebApiHelpPageExperiment.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public IList<Account> Accounts { get; set; }
    }
}