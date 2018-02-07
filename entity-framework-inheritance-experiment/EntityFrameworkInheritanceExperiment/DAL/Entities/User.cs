using System.Collections.Generic;

namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public IList<AuthenticationMethod> AuthenticationMethods { get; set; }
        public IList<EmailAddress> EmailAddresses { get; set; }

        // Looks like known bug in EF: I can't have entity that only has Id field, need at least one another column
        public string DUMMY { get; set; }

        public User()
        {
            AuthenticationMethods = new List<AuthenticationMethod>();
            EmailAddresses = new List<EmailAddress>();
        }
    }
}
