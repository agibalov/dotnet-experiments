using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class InstallAndStartCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WindowsServiceManager _windowsServiceManager;

        public InstallAndStartCommandHandler(WindowsServiceManager windowsServiceManager)
        {
            _windowsServiceManager = windowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _windowsServiceManager.IsInstalled();
            if (!isInstalled)
            {
                Logger.Info("Service is not installed, trying to install");
                _windowsServiceManager.Install();
                Logger.Info("Successfully installed the service");
            }
            else
            {
                Logger.Info("Service is already installed");
            }

            var isRunning = _windowsServiceManager.IsRunning();
            if (!isRunning)
            {
                Logger.Info("Service is not running, trying to start");
                _windowsServiceManager.Start();
                Logger.Info("Successfully started the service");
            }
            else
            {
                Logger.Info("Service is already running");
            }
        }
    }
}