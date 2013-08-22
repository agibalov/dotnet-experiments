namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public class EmailPasswordAuthenticationMethod : AuthenticationMethod
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}