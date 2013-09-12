using System.Web.Mvc;
using AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Facebook;
using AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Google;
using AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Twitter;
using AspNetMvcGoogleFacebookTwitterAuthExperiment.Models;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Controllers
{
    public class HomeController : Controller
    {
        private readonly GoogleFacade _googleFacade = new GoogleFacade(
            "347919506647.apps.googleusercontent.com", 
            "uD-KxJPqLBAQt6NqPyhZAN_f",
            "~/Home/GoogleAuthCallback");

        private readonly FacebookFacade _facebookFacade = new FacebookFacade(
            "219687484855349",
            "d58490eecd561a33275ccfeeabc0e5d5",
            "~/Home/FacebookAuthCallback");

        private readonly TwitterFacade _twitterFacade = new TwitterFacade(
            "xanAvVljVi9yCgbGkBZBcg",
            "yqaJ1T4Hm7Q0CJ656tGy5hkrSwNkYZGH3dVzbfcVcw",
            "~/Home/TwitterAuthCallback");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthenticateWithGoogle()
        {
            return Redirect(_googleFacade.GetAuthenticationUrl());
        }

        public ActionResult AuthenticateWithFacebook()
        {
            return Redirect(_facebookFacade.GetAuthenticationUrl());
        }

        public ActionResult AuthenticateWithTwitter()
        {
            return Redirect(_twitterFacade.GetAuthenticationUrl());
        }

        public ActionResult GoogleAuthCallback(string code)
        {
            var userInfo = _googleFacade.GetUserInfo(code);

            var authenticationModel = new AuthenticationModel
                {
                    Provider = "Google"
                };
            if (userInfo != null)
            {
                authenticationModel.UserId = userInfo.UserId;
                authenticationModel.Email = userInfo.Email;
                authenticationModel.AccessToken = userInfo.AccessToken;
                authenticationModel.Extra = _googleFacade.GetCustomUserInfo(userInfo.AccessToken);
            }

            return View("Index", authenticationModel);
        }

        public ActionResult FacebookAuthCallback(string code)
        {
            var userInfo = _facebookFacade.GetUserInfo(code);

            var authenticationModel = new AuthenticationModel
            {
                Provider = "Facebook"
            };
            if (userInfo != null)
            {
                authenticationModel.UserId = userInfo.UserId;
                authenticationModel.Email = userInfo.Email;
                authenticationModel.AccessToken = userInfo.AccessToken;
                authenticationModel.Extra = _facebookFacade.GetCustomUserInfo(userInfo.AccessToken);
            }

            return View("Index", authenticationModel);
        }

        public ActionResult TwitterAuthCallback(
            [Bind(Prefix = "oauth_token")] string oauthToken, 
            [Bind(Prefix = "oauth_verifier")] string oauthVerifier)
        {
            var userInfo = _twitterFacade.GetUserInfo(oauthToken, oauthVerifier);

            var authenticationModel = new AuthenticationModel
            {
                Provider = "Twitter"
            };
            if (userInfo != null)
            {
                authenticationModel.UserId = userInfo.UserId;
                authenticationModel.AccessToken = userInfo.AccessToken;
                authenticationModel.AccessTokenSecret = userInfo.AccessTokenSecret;
                /*authenticationModel.Extra = _twitterFacade.GetCustomUserInfo(
                    userInfo.AccessToken,
                    userInfo.AccessTokenSecret);*/
            }

            return View("Index", authenticationModel);
        }
    }
}
