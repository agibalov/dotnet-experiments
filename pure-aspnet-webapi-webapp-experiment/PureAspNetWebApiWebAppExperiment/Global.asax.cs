using System;
using System.Web;
using System.Web.Http;

namespace PureAspNetWebApiWebAppExperiment
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                "BlogApi", 
                "{action}", 
                new { controller = "Post" });
        }
    }
}