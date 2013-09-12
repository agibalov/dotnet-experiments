using Newtonsoft.Json;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Twitter
{
    public class CustomTwitterUserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string Url { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
        public string TimeZone { get; set; }
        public bool Verified { get; set; }
        public string ProfileImageUrl { get; set; }

        public override string ToString()
        {
            return string.Format("CustomGoogleUserInfo {0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}