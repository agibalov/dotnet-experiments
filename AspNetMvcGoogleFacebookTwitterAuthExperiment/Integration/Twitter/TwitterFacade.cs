using System.Collections.Specialized;
using OAuth2.Client.Impl;
using OAuth2.Configuration;
using OAuth2.Infrastructure;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Twitter
{
    public class TwitterFacade
    {
        private readonly string _twitterConsumerKey;
        private readonly string _twitterConsumerSecret;
        private readonly string _twitterCallbackUrl;

        public TwitterFacade(
            string twitterConsumerKey, 
            string twitterConsumerSecret,
            string twitterCallbackUrl)
        {
            _twitterConsumerKey = twitterConsumerKey;
            _twitterConsumerSecret = twitterConsumerSecret;
            _twitterCallbackUrl = twitterCallbackUrl;
        }

        public string GetAuthenticationUrl()
        {
            var twitterClient = MakeTwitterClient();
            return twitterClient.GetLoginLinkUri();
        }

        public TwitterUserInfo GetUserInfo(string oauthToken, string oauthVerifier)
        {
            var parameters = new NameValueCollection
                {
                    {"oauth_token", oauthToken},
                    {"oauth_verifier", oauthVerifier}
                };

            var facebookClient = MakeTwitterClient();
            var userInfo = facebookClient.GetUserInfo(parameters);
            if (userInfo == null)
            {
                return null;
            }

            return new TwitterUserInfo
                {
                    UserId = userInfo.Id,
                };
        }

        private TwitterClient MakeTwitterClient()
        {
            var twitterClient = new TwitterClient(
                new RequestFactory(),
                new RuntimeClientConfiguration
                    {
                        ClientId = _twitterConsumerKey,
                        ClientSecret = _twitterConsumerSecret,
                        IsEnabled = true,
                        RedirectUri = UriUtility.ToAbsolute(_twitterCallbackUrl)
                    });

            return twitterClient;
        }
    }
}