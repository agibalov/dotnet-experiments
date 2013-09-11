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
            return View(new SignInPageModel
                {
                    GoogleAuthUrl = _googleFacade.GetAuthenticationUrl(),
                    FacebookAuthUrl = _facebookFacade.GetAuthenticationUrl(),
                    TwitterAuthUrl = _twitterFacade.GetAuthenticationUrl()
                });
        }

        public ActionResult GoogleAuthCallback(string code)
        {
            var userInfo = _googleFacade.GetUserInfo(code);

            var authModel = new AuthModel
                {
                    Provider = "Google"
                };
            if (userInfo != null)
            {
                authModel.UserId = userInfo.UserId;
                authModel.Email = userInfo.Email;
            }

            return View("AuthCallback", authModel);
        }

        public ActionResult FacebookAuthCallback(string code)
        {
            var userInfo = _facebookFacade.GetUserInfo(code);

            var authModel = new AuthModel
            {
                Provider = "Facebook"
            };
            if (userInfo != null)
            {
                authModel.UserId = userInfo.UserId;
                authModel.Email = userInfo.Email;
            }

            return View("AuthCallback", authModel);
        }

        public ActionResult TwitterAuthCallback(
            [Bind(Prefix = "oauth_token")] string oauthToken, 
            [Bind(Prefix = "oauth_verifier")] string oauthVerifier)
        {
            var userInfo = _twitterFacade.GetUserInfo(oauthToken, oauthVerifier);

            var authModel = new AuthModel
            {
                Provider = "Twitter"
            };
            if (userInfo != null)
            {
                authModel.UserId = userInfo.UserId;
            }

            return View("AuthCallback", authModel);
        }
    }
}
