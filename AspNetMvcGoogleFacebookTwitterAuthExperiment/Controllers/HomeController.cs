using System;
using System.Web.Mvc;
using DotNetOpenAuth.GoogleOAuth2;
using Facebook;
using TweetSharp;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Controllers
{
    public class HomeController : Controller
    {
        private const string GoogleClientId = "347919506647.apps.googleusercontent.com";
        private const string GoogleClientSecret = "uD-KxJPqLBAQt6NqPyhZAN_f";

        private const string FacebookApplicationId = "219687484855349";
        private const string FacebookApplicationSecret = "d58490eecd561a33275ccfeeabc0e5d5";

        private const string TwitterConsumerKey = "xanAvVljVi9yCgbGkBZBcg";
        private const string TwitterConsumerSecret = "yqaJ1T4Hm7Q0CJ656tGy5hkrSwNkYZGH3dVzbfcVcw";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignInWithGoogle()
        {
            var googleClient = new GoogleOAuth2Client(GoogleClientId, GoogleClientSecret);
            googleClient.RequestAuthentication(HttpContext, GetGoogleCallbackUrl());
            return null;
        }

        public ActionResult GoogleAuthCallback()
        {
            var googleClient = new GoogleOAuth2Client(GoogleClientId, GoogleClientSecret);
            var authentication = googleClient.VerifyAuthentication(HttpContext, GetGoogleCallbackUrl());

            var authenticationModel = new AuthenticationModel
                {
                    ProviderName = "Google",
                    IsAuthenticated = authentication.IsSuccessful
                };

            if (authentication.IsSuccessful)
            {
                authenticationModel.ProviderUserId = authentication.ProviderUserId;
                authenticationModel.UserName = authentication.UserName;
                authenticationModel.UserEmail = authentication.ExtraData["email"];
                authenticationModel.FirstName = authentication.ExtraData["given_name"];
                authenticationModel.LastName = authentication.ExtraData["family_name"];
            }
            
            return View("Index", authenticationModel);
        }

        private Uri GetGoogleCallbackUrl()
        {
            var uriBuilder = new UriBuilder(Request.Url)
            {
                Query = string.Empty,
                Fragment = string.Empty,
                Path = Url.Action("GoogleAuthCallback")
            };
            return uriBuilder.Uri;
        }

        private string GetFacebookCallbackUrl()
        {
            var uriBuilder = new UriBuilder(Request.Url)
                {
                    Query = string.Empty,
                    Fragment = string.Empty,
                    Path = Url.Action("FacebookAuthCallback")
                };
            return uriBuilder.Uri.AbsoluteUri;
        }

        public ActionResult SignInWithFacebook()
        {
            var facebookClient = new FacebookClient();
            var facebookLoginUrl = facebookClient.GetLoginUrl(new
                {
                    client_id = FacebookApplicationId,
                    client_secret = FacebookApplicationSecret,
                    redirect_uri = GetFacebookCallbackUrl(),
                    response_type = "code",
                    scope = "email"
                });

            return Redirect(facebookLoginUrl.AbsoluteUri);
        }

        public ActionResult FacebookAuthCallback(
            [Bind(Prefix = "code")] string code, 
            [Bind(Prefix = "error")] string error,
            [Bind(Prefix = "error_reason")]string errorReason)
        {
            var authenticationModel = new AuthenticationModel
                {
                    ProviderName = "Facebook"
                };

            if (!string.IsNullOrEmpty(code))
            {
                var facebookClient = new FacebookClient();
                dynamic result = facebookClient.Post("oauth/access_token", new
                    {
                        client_id = FacebookApplicationId,
                        client_secret = FacebookApplicationSecret,
                        redirect_uri = GetFacebookCallbackUrl(),
                        code
                    });

                var accessToken = result.access_token;
                facebookClient.AccessToken = accessToken;

                dynamic me = facebookClient.Get("me?fields=first_name,last_name,id,email");

                authenticationModel.IsAuthenticated = true;
                authenticationModel.ProviderUserId = me.id;
                authenticationModel.UserName = null;
                authenticationModel.UserEmail = me.email;
                authenticationModel.FirstName = me.first_name;
                authenticationModel.LastName = me.last_name;
            }
            else
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Comments = string.Format("Error: error={0}, error_reason={1}", error, errorReason);
            }

            return View("Index", authenticationModel);
        }

        public ActionResult SignInWithTwitter()
        {
            var twitterService = new TwitterService(
                TwitterConsumerKey, 
                TwitterConsumerSecret);
            var requestToken = twitterService.GetRequestToken(GetTwitterCallbackUrl());
            var twitterLoginUrl = twitterService.GetAuthorizationUri(requestToken);
            return Redirect(twitterLoginUrl.AbsoluteUri);
        }

        public ActionResult TwitterAuthCallback(
            [Bind(Prefix = "oauth_token")] string oauthToken,
            [Bind(Prefix = "oauth_verifier")] string oauthVerifier,
            [Bind(Prefix = "denied")] string denied)
        {
            var authenticationModel = new AuthenticationModel
                {
                    ProviderName = "Twitter"
                };

            if (string.IsNullOrEmpty(denied))
            {
                var requestToken = new OAuthRequestToken
                    {
                        Token = oauthToken
                    };

                var twitterService = new TwitterService(TwitterConsumerKey, TwitterConsumerSecret);
                var accessToken = twitterService.GetAccessToken(requestToken, oauthVerifier);
                twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
                var twitterUser = twitterService.VerifyCredentials(new VerifyCredentialsOptions());

                authenticationModel.IsAuthenticated = true;
                authenticationModel.ProviderUserId = Convert.ToString(twitterUser.Id);
                authenticationModel.UserName = twitterUser.ScreenName;
            }
            else
            {
                authenticationModel.IsAuthenticated = false;
            }

            return View("Index", authenticationModel);
        }

        private string GetTwitterCallbackUrl()
        {
            var uriBuilder = new UriBuilder(Request.Url)
            {
                Query = string.Empty,
                Fragment = string.Empty,
                Path = Url.Action("TwitterAuthCallback")
            };
            return uriBuilder.Uri.AbsoluteUri;
        }
    }

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
