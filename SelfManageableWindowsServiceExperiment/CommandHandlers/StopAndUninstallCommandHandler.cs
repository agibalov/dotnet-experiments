using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StopAndUninstallCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public StopAndUninstallCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
        {
            _dummyWindowsServiceManager = dummyWindowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _dummyWindowsServiceManager.IsInstalled();
            if (!isInstalled)
            {
                Logger.Info("Service is not installed");
                return;
            }

            Logger.Info("Service is installed");

            var isRunning = _dummyWindowsServiceManager.IsRunning();
            if (isRunning)
            {
                Logger.Info("Service is running, trying to stop it");

                _dummyWindowsServiceManager.Stop();

                Logger.Info("Stopped the service successfully");
            }
            else
            {
                Logger.Info("Service is not running");    
            }

            Logger.Info("Trying to uninstall the service");

            _dummyWindowsServiceManager.Uninstall();

            Logger.Info("Successfully uninstalled the service");
        }
    }
}