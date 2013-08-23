namespace EntityFrameworkInheritanceExperiment.DTO
{
    public class EmailAuthenticationMethodDTO : AuthenticationMethodDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}