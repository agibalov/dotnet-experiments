using System.Web.Http;
using Ninject;
using Owin;

namespace WpfWebApiExperiment.WebApiServer
{
    public class WebAppConfigurer
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var kernel = new StandardKernel();
            kernel.Bind<NoteRepository>().ToSelf().InSingletonScope();

            var httpConfiguration = new HttpConfiguration
            {
                DependencyResolver = new NinjectDependencyResolver(kernel)
            };

            httpConfiguration.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{id}",
                new { id = RouteParameter.Optional });

            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}