using System;
using System.Threading;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Ninject;
using Owin;

namespace WpfWebApiExperiment.WebApiServer
{
    public class WebAppRunner
    {
        private ManualResetEvent _stopManualResetEvent;
        private Thread _workerThread;

        public void Start()
        {
            if (_stopManualResetEvent != null)
            {
                throw new InvalidOperationException("WebApiRunner is already running");
            }

            _stopManualResetEvent = new ManualResetEvent(false);
            _workerThread = new Thread(() =>
            {
                const string baseUrl = "http://localhost:8080/";
                using (WebApp.Start(baseUrl, ConfigureAppBuilder))
                {
                    _stopManualResetEvent.WaitOne();
                }
            });
            _workerThread.Start();
        }

        public void Stop()
        {
            if (_stopManualResetEvent == null)
            {
                throw new InvalidOperationException("WebApiRunner is not running");
            }

            _stopManualResetEvent.Set();
            _stopManualResetEvent = null;
            _workerThread = null;
        }

        // TODO: make this reusable
        private static void ConfigureAppBuilder(IAppBuilder appBuilder)
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