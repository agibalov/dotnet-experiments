﻿using System.Collections.Specialized;
using OAuth2.Client.Impl;
using OAuth2.Configuration;
using OAuth2.Infrastructure;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Google
{
    public class GoogleFacade
    {
        private readonly string _googleClientId;
        private readonly string _googleClientSecret;
        private readonly string _googleCallbackUrl;

        public GoogleFacade(
            string googleClientId, 
            string googleClientSecret,
            string googleCallbackUrl)
        {
            _googleClientId = googleClientId;
            _googleClientSecret = googleClientSecret;
            _googleCallbackUrl = googleCallbackUrl;
        }

        public string GetAuthenticationUrl()
        {
            var googleClient = MakeGoogleClient();
            return googleClient.GetLoginLinkUri();
        }

        public GoogleUserInfo GetUserInfo(string code)
        {
            var parameters = new NameValueCollection
                {
                    {"code", code}
                };

            var googleClient = MakeGoogleClient();
            var userInfo = googleClient.GetUserInfo(parameters);
            if (userInfo == null)
            {
                return null;
            }

            return new GoogleUserInfo
                {
                    UserId = userInfo.Id,
                    Email = userInfo.Email
                };
        }

        private GoogleClient MakeGoogleClient()
        {
            var googleClient = new GoogleClient(
                new RequestFactory(),
                new RuntimeClientConfiguration
                    {
                        ClientId = _googleClientId,
                        ClientSecret = _googleClientSecret,
                        IsEnabled = true,
                        Scope = "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email",
                        RedirectUri = UriUtility.ToAbsolute(_googleCallbackUrl)
                    });

            return googleClient;
        }
    }
}