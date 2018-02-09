using System.Windows;
using System.Windows.Controls;
using Ninject;

namespace HelloArchitecture
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            mainWindow.Show();

            var contentFrame = (Frame)mainWindow.FindName("ContentFrame");

            var kernel = new StandardKernel();
            kernel.Bind<Frame>().ToConstant(contentFrame);
            kernel.Bind<INavService>().To<MyNavService>().InSingletonScope();

            kernel.Bind<MainPage>().ToSelf().InSingletonScope();
            kernel.Bind<object>().To<MainPageViewModel>().Named(typeof(MainPage).Name + "viewmodel");

            kernel.Bind<KindOfRemoteService>().ToSelf().InSingletonScope();

            var navService = kernel.Get<INavService>();
            navService.NavigateTo(typeof(MainPage), "hello from OnStartup()!!!");
        }
    }
}
