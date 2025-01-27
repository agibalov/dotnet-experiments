﻿using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Ninject;
using RestSharp;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.ViewModels;
using WpfWebApiExperiment.ViewModels.NoteListScreen;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperiment
{
    public class AppBootstrapper : BootstrapperBase
    {
        private readonly IKernel _kernel = new StandardKernel();

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        {
            _kernel.Bind<IWindowManager>().To<WindowManager>();

            _kernel.Bind<IRestClient>().ToConstant(new RestClient("http://localhost:8080/"));
            _kernel.Bind<IApiClient>().To<ApiClient>().InSingletonScope();
            _kernel.Bind<IApiExecutor>().To<ApiExecutor>().InSingletonScope();

            _kernel.Bind<ShellViewModel>().ToSelf().InSingletonScope();
            _kernel.Bind<INavigationService>().ToMethod(c => c.Kernel.Get<ShellViewModel>());
            _kernel.Bind<ILongOperationExecutor>().To<LongOperationExecutor>();
            _kernel.Bind<ILongOperationListener>().ToMethod(c => c.Kernel.Get<ShellViewModel>());

            _kernel.Bind<NoteListScreenViewModel>().ToSelf();
            _kernel.Bind<NoteScreenViewModel>().ToSelf();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _kernel.GetAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _kernel.Get(service, key);
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }
    }
}
