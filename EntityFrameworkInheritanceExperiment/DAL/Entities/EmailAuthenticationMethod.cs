namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public class EmailAuthenticationMethod : AuthenticationMethod
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}