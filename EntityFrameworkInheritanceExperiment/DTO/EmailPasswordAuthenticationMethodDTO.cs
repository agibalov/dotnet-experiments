namespace EntityFrameworkInheritanceExperiment.DTO
{
    public class EmailPasswordAuthenticationMethodDTO : AuthenticationMethodDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}