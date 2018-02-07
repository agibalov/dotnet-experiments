using System.Collections.Specialized;
using OAuth2.Client.Impl;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using RestSharp;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Facebook
{
    public class FacebookFacade
    {
        private readonly string _facebookApplicationId;
        private readonly string _facebookApplicationSecret;
        private readonly string _facebookCallbackUrl;

        public FacebookFacade(
            string facebookApplicationId, 
            string facebookApplicationSecret,
            string facebookCallbackUrl)
        {
            _facebookApplicationId = facebookApplicationId;
            _facebookApplicationSecret = facebookApplicationSecret;
            _facebookCallbackUrl = facebookCallbackUrl;
        }

        public string GetAuthenticationUrl()
        {
            var facebookClient = MakeFacebookClient();
            return facebookClient.GetLoginLinkUri();
        }

        public FacebookUserInfo GetUserInfo(string code)
        {
            var parameters = new NameValueCollection
                {
                    {"code", code}
                };

            var facebookClient = MakeFacebookClient();
            var userInfo = facebookClient.GetUserInfo(parameters);
            if (userInfo == null)
            {
                return null;
            }

            return new FacebookUserInfo
                {
                    UserId = userInfo.Id,
                    Email = userInfo.Email,
                    AccessToken = facebookClient.AccessToken
                };
        }

        public CustomFacebookUserInfo GetCustomUserInfo(string accessToken)
        {
            var client = new RestClient("https://graph.facebook.com/");
            var request = new RestRequest("/me", Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", accessToken));
            var response = client.Execute<CustomFacebookUserInfo>(request);
            return response.Data;
        }

        private FacebookClient MakeFacebookClient()
        {
            var facebookClient = new FacebookClient(
                new RequestFactory(),
                new RuntimeClientConfiguration
                    {
                        ClientId = _facebookApplicationId,
                        ClientSecret = _facebookApplicationSecret,
                        IsEnabled = true,
                        Scope = "email",
                        RedirectUri = UriUtility.ToAbsolute(_facebookCallbackUrl)
                    });

            return facebookClient;
        }
    }
}