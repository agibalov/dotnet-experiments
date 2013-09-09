using System;
using TweetSharp;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Twitter
{
    public class TwitterFacade
    {
        private readonly string _twitterConsumerKey;
        private readonly string _twitterConsumerSecret;

        public TwitterFacade(
            string twitterConsumerKey,
            string twitterConsumerSecret)
        {
            _twitterConsumerKey = twitterConsumerKey;
            _twitterConsumerSecret = twitterConsumerSecret;
        }

        public TwitterUserInfo GetUserInfo(string oauthToken, string oauthVerifier)
        {
            var requestToken = new OAuthRequestToken
                {
                    Token = oauthToken
                };

            var twitterService = new TwitterService(_twitterConsumerKey, _twitterConsumerSecret);
            var accessToken = twitterService.GetAccessToken(requestToken, oauthVerifier);
            twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            var twitterUser = twitterService.VerifyCredentials(new VerifyCredentialsOptions());

            return new TwitterUserInfo
                {
                    UserId = Convert.ToString(twitterUser.Id),
                    ScreenName = twitterUser.ScreenName
                };
        }
    }
}