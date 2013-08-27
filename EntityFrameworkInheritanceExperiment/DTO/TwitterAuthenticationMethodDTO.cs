namespace EntityFrameworkInheritanceExperiment.DTO
{
    public class TwitterAuthenticationMethodDTO : AuthenticationMethodDTO
    {
        public string TwitterUserId { get; set; }
        public string TwitterDisplayName { get; set; }
    }
}