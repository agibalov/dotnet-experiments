using System.Windows;
using WpfWebApiExperiment.WebApiServer;

namespace WpfWebApiExperiment
{
    public partial class App : Application
    {
        private readonly WebAppRunner _webAppRunner = new WebAppRunner();

        public App()
        {
            InitializeComponent();

            Startup += OnStartup;
            Exit += OnExit;
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            _webAppRunner.Start();
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            _webAppRunner.Stop();
        }
    }
}
