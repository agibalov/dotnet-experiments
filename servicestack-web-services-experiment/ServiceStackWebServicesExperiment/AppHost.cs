using System.Reflection;
using Funq;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStackWebServicesExperiment
{
    public class AppHost : AppHostHttpListenerBase
    {
        public AppHost()
            : base("Hello ServiceStack application", Assembly.GetExecutingAssembly())
        {
        }

        public override void Configure(Container container)
        {
        }
    }
}