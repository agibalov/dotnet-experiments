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
            if (authentication.IsSuccessful)
            {
                ViewBag.Message = string.Format(
                    "id: {0}, user_name: {1}, email: {2}",
                    authentication.ProviderUserId,
                    authentication.UserName,
                    authentication.ExtraData["email"]);
            }
            else
            {
                ViewBag.Message = "not successfull";
            }

            return View();
        }

        private Uri GetGoogleCallbackUrl()
        {
            var uriBuilder = new UriBuilder(Request.Url)
            {
                Query = "?", // it doesn't work with string.Empty (https://github.com/mj1856/DotNetOpenAuth.GoogleOAuth2/issues/4)
                Fragment = string.Empty,
                Path = Url.Action("GoogleAuthCallback")
            };
            return uriBuilder.Uri;//.AbsoluteUri;
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
                ViewBag.Message = string.Format(
                    "Access token: {0}, first name: {1}, last name: {2}, id: {3}, email: {4}", 
                    accessToken,
                    me.first_name, 
                    me.last_name, 
                    me.id, 
                    me.email);
            }
            else
            {
                ViewBag.Message = string.Format("Error: error={0}, error_reason={1}", error, errorReason);
            }

            return View();
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
                ViewBag.Message = string.Format(
                    "id: {0}, name: {1}, screen_name: {2}",
                    twitterUser.Id,
                    twitterUser.Name,
                    twitterUser.ScreenName);
            }
            else
            {

                ViewBag.Message = "User cancelled";
            }

            return View();
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
        
        public string ProviderUserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
