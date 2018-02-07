namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Models
{
    public class AuthenticationModel
    {
        public string Provider { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public object Extra { get; set; }
    }
}