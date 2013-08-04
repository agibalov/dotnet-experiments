using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StopAndUninstallCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WindowsServiceManager _windowsServiceManager;

        public StopAndUninstallCommandHandler(WindowsServiceManager windowsServiceManager)
        {
            _windowsServiceManager = windowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _windowsServiceManager.IsInstalled();
            if (!isInstalled)
            {
                Logger.Info("Service is not installed");
                return;
            }

            Logger.Info("Service is installed");

            var isRunning = _windowsServiceManager.IsRunning();
            if (isRunning)
            {
                Logger.Info("Service is running, trying to stop it");
                _windowsServiceManager.Stop();
                Logger.Info("Stopped the service successfully");
            }
            else
            {
                Logger.Info("Service is not running");    
            }

            Logger.Info("Trying to uninstall the service");
            _windowsServiceManager.Uninstall();
            Logger.Info("Successfully uninstalled the service");
        }
    }
}