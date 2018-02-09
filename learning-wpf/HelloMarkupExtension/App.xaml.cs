using System.Windows;
using Ninject;

namespace HelloMarkupExtension
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var kernel = new StandardKernel(new NinjectSettings { InjectNonPublic = true });
            NinjectServiceLocator.Kernel = kernel;

            kernel.Bind<CalculatorService>().ToSelf().InSingletonScope();
            kernel.Bind<BoolToVisibilityConverterService>().ToSelf().InSingletonScope();

            base.OnStartup(e);
        }
    }

    public class CalculatorService
    {
        public int AddNumbers(int a, int b)
        {
            return a + b;
        }
    }

    public class BoolToVisibilityConverterService
    {
        public Visibility MakeVisibilityFromBool(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public static class NinjectServiceLocator
    {
        public static IKernel Kernel { get; set; }
    }
}
