namespace EntityFrameworkInheritanceExperiment.DTO
{
    public class FacebookAuthenticationMethodDTO : AuthenticationMethodDTO
    {
        public string FacebookUserId { get; set; }
        public string Email { get; set; }
    }
}