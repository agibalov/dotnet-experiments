using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace AspNetMvcOpenIdExperiment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignIn()
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
