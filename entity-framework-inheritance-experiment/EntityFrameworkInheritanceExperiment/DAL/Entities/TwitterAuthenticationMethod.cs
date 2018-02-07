namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public class TwitterAuthenticationMethod : AuthenticationMethod
    {
        public string TwitterUserId { get; set; }
        public string TwitterDisplayName { get; set; }
    }
}