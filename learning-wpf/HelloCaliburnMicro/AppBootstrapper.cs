using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;

namespace HelloCaliburnMicro
{
    public class AppBootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        {
            _container.Singleton<IWindowManager, WindowManager>(); // this one is required

            _container.RegisterSingleton(typeof(KindOfRemoteService), null, typeof(KindOfRemoteService));
            _container.RegisterSingleton(typeof(ShellViewModel), null, typeof(ShellViewModel));
            _container.RegisterSingleton(typeof(Page1ViewModel), null, typeof(Page1ViewModel));
            _container.RegisterSingleton(typeof(Page2ViewModel), null, typeof(Page2ViewModel));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}