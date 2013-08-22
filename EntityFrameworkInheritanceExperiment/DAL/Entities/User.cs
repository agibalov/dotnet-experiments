using System.Collections.Generic;

namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public IList<AuthenticationMethod> AuthenticationMethods { get; set; }
    }
}
