using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace AspNetMvcBackgroundWorkerExperiment
{
    // Start is called on very first request, when App is not loaded
    // Stop is called before unloading the app
    // Loading and unloading is defined by app pool

    public class MvcApplication : System.Web.HttpApplication
    {
        private CounterService _counterService;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var kernel = new StandardKernel();

            kernel.Bind<CounterService>().ToSelf().InSingletonScope();

            var controllerFactory = new NinjectControllerFactory(kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            
            var controllerActivator = new NinjectHttpControllerActivator(kernel);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), controllerActivator);

            _counterService = kernel.Get<CounterService>();
            _counterService.Start();
        }

        protected void Application_End()
        {
            _counterService.Stop();
        }
    }
}