namespace EntityFrameworkInheritanceExperiment.DTO
{
    public class GoogleAuthenticationMethodDTO : AuthenticationMethodDTO
    {
        public string GoogleUserId { get; set; }
        public string Email { get; set; }
    }
}