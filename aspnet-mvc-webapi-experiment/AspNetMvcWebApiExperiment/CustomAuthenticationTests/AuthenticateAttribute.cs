using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AspNetMvcWebApiExperiment.CustomAuthenticationTests
{
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        private const string MagicSessionTokenHeaderName = "MagicSessionToken";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Contains(MagicSessionTokenHeaderName))
            {
                var sessionTokenCookie = actionContext.Request.Headers.GetValues(MagicSessionTokenHeaderName).SingleOrDefault();
                if (sessionTokenCookie != null)
                {
                    return;
                }
            }

            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "You should authenticate" };
        }
    }
}