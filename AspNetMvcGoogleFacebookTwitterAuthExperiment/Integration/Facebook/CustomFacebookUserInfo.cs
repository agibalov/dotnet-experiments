using Newtonsoft.Json;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Facebook
{
    public class CustomFacebookUserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Link { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Timezone { get; set; }
        public string Locale { get; set; }
        public bool Verified { get; set; }
        public string UpdatedTime { get; set; }

        public override string ToString()
        {
            return string.Format("CustomFacebookUserInfo {0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}