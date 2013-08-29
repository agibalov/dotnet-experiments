namespace EntityFrameworkInheritanceExperiment.DAL.Entities
{
    public class EmailAddress
    {
        public int EmailAddressId { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
    }
}