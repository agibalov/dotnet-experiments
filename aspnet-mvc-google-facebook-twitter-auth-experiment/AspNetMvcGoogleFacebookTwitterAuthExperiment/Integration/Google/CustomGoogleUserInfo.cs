using Newtonsoft.Json;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Google
{
    public class CustomGoogleUserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool VerifiedEmail { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Locale { get; set; }

        public override string ToString()
        {
            return string.Format("CustomGoogleUserInfo {0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}