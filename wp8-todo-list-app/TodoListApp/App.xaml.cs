using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace TodoListApp
{
    public sealed partial class App : CaliburnApplication
    {
        private WinRTContainer _container;
        private INavigationService _navigationService;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();
            
            _container
                .PerRequest<MainViewModel>()
                .PerRequest<Page2ViewModel>();

            PrepareViewFirst();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _navigationService = _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Initialize();

            var resumed = false;
            if(args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                resumed = _navigationService.ResumeState();
            }

            if (!resumed)
            {
                // DisplayRootViewFor<MainViewModel>(); // doesn't work: http://stackoverflow.com/questions/24793797/navigatetoviewmodel-with-caliburn-micro-2-and-windows-phone-8-1
                DisplayRootView<MainView>();
            }
        }

        protected override void OnSuspending(object sender, SuspendingEventArgs e)
        {
            _navigationService.SuspendState();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if(instance == null)
            {
                throw new InvalidOperationException("Service " + service + " is not registered!");
            }

            return instance;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }    
}