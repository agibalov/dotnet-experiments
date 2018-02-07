using System.Web.Http;
using System.Web.Http.Description;
using WebApiHelpPageExperiment.Infrastructure;

namespace WebApiHelpPageExperiment
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{action}",
                defaults: new { controller = "MyApi" }
            );

            config.Services.Replace(typeof(IDocumentationProvider), new AttributesDocumentationProvider());
        }
    }
}
