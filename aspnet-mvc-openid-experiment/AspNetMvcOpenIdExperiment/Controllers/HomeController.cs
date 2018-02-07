using System;
using System.Net.Http;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Newtonsoft.Json;

namespace AspNetMvcOpenIdExperiment.Controllers
{
    public class HomeController : Controller
    {
        private const string FacebookApplicationId = "219687484855349";
        private const string FacebookApplicationSecret = "d58490eecd561a33275ccfeeabc0e5d5";

        private const string GoogleClientId = "347919506647.apps.googleusercontent.com";
        private const string GoogleClientSecret = "uD-KxJPqLBAQt6NqPyhZAN_f";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignInWithGoogleOAuth2()
        {
            var webServerClient = new WebServerClient(new AuthorizationServerDescription
            {
                TokenEndpoint = new Uri("https://accounts.google.com/o/oauth2/token"),
                AuthorizationEndpoint = new Uri("https://accounts.google.com/o/oauth2/auth"),
                ProtocolVersion = ProtocolVersion.V20
            })
            {
                ClientIdentifier = GoogleClientId,
                ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(GoogleClientSecret)
            };

            var authorization = webServerClient.ProcessUserAuthorization(Request);
            if (authorization == null)
            {
                var request = webServerClient.PrepareRequestUserAuthorization(new[] { "https://www.googleapis.com/auth/userinfo.email" });
                request.Send(HttpContext);
                HttpContext.Response.End();
                return null;
            }

            if (string.IsNullOrEmpty(authorization.AccessToken))
            {
                ViewBag.Message = "Cancelled or something weird";
            }
            else
            {
                var authorizingHandler = webServerClient.CreateAuthorizingHandler(authorization);
                var httpClient = new HttpClient(authorizingHandler);
                var jsonResponse = httpClient
                    .GetAsync("https://www.googleapis.com/oauth2/v1/userinfo")
                    .Result
                    .Content
                    .ReadAsStringAsync()
                    .Result;
                var googleMe = JsonConvert.DeserializeObject<GoogleMe>(jsonResponse);

                ViewBag.Message = string.Format(
                    "Access token: {0}, JSON response: {1}, Email: {2}, Id: {3}",
                    authorization.AccessToken,
                    jsonResponse,
                    googleMe.Email,
                    googleMe.Id);
            }

            return View();
        }

        public ActionResult SignInWithFacebookOAuth2()
        {
            var webServerClient = new WebServerClient(new AuthorizationServerDescription
                {
                    TokenEndpoint = new Uri("https://graph.facebook.com/oauth/access_token"),
                    AuthorizationEndpoint = new Uri("https://graph.facebook.com/oauth/authorize"),
                    ProtocolVersion = ProtocolVersion.V20
                })
                {
                    ClientIdentifier = FacebookApplicationId,
                    ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(FacebookApplicationSecret)
                };

            var authorization = webServerClient.ProcessUserAuthorization(Request);
            if (authorization == null)
            {
                var request = webServerClient.PrepareRequestUserAuthorization(new[] {"email"});
                request.Send(HttpContext);
                HttpContext.Response.End();
                return null;
            }

            if (string.IsNullOrEmpty(authorization.AccessToken))
            {
                ViewBag.Message = "Cancelled or something weird";
            }
            else
            {
                var authorizingHandler = webServerClient.CreateAuthorizingHandler(authorization);
                var httpClient = new HttpClient(authorizingHandler);
                var jsonResponse = httpClient
                    .GetAsync("https://graph.facebook.com/me?fields=email")
                    .Result
                    .Content
                    .ReadAsStringAsync()
                    .Result;
                var facebookMe = JsonConvert.DeserializeObject<FacebookMe>(jsonResponse);

                ViewBag.Message = string.Format(
                    "Access token: {0}, JSON response: {1}, Email: {2}, Id: {3}",
                    authorization.AccessToken,
                    jsonResponse,
                    facebookMe.Email,
                    facebookMe.Id);
            }

            return View();
        }

        public class FacebookMe
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }

        public class GoogleMe
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }

        public ActionResult SignInWithGoogleOpenId()
        {
            using (var openIdRelyingParty = new OpenIdRelyingParty())
            {
                var response = openIdRelyingParty.GetResponse();
                if (response == null)
                {
                    var fetchRequest = new FetchRequest();
                    fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
                    fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                    fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);

                    var request = openIdRelyingParty.CreateRequest("https://www.google.com/accounts/o8/id");
                    request.AddExtension(fetchRequest);
                    return request
                        .RedirectingResponse
                        .AsActionResult();
                }
                
                var responseStatus = response.Status;
                if (responseStatus == AuthenticationStatus.Authenticated)
                {
                    var fetchResponse = response.GetExtension<FetchResponse>();

                    var email = fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Email);
                    var firstName = fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First);
                    var lastName = fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last);

                    ViewBag.Message = string.Format(
                        "ClaimedId: '{0}', Email: '{1}', First name: '{2}', Last name: '{3}'",
                        response.ClaimedIdentifier,
                        email,
                        firstName,
                        lastName);
                }
                else if (responseStatus == AuthenticationStatus.Canceled)
                {
                    ViewBag.Message = "Cancelled at provider";
                }
                else if (responseStatus == AuthenticationStatus.Failed)
                {
                    ViewBag.Message = string.Format("Error: {0}", response.Exception.Message);
                }
                else
                {
                    ViewBag.Message = "Have no idea what happened";
                }

                return View();
            }
        }
    }
}
