namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public class FacebookAuthenticationMethod : AuthenticationMethod
    {
        public string FacebookUserId { get; set; }
        public string Email { get; set; }
    }
}