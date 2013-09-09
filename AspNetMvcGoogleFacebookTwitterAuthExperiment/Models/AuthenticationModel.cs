namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Models
{
    public class AuthenticationModel
    {
        public string ProviderName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Comments { get; set; }
        
        public string ProviderUserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}