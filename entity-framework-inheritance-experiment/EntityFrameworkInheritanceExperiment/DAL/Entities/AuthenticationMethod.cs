namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public abstract class AuthenticationMethod
    {
        public int AuthenticationMethodId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}