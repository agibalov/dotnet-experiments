using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class UninstallCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public UninstallCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
        {
            _dummyWindowsServiceManager = dummyWindowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _dummyWindowsServiceManager.IsInstalled();
            if (!isInstalled)
            {
                Logger.Error("Service is not installed");
                return;
            }

            Logger.Info("Service is installed");

            var isRunning = _dummyWindowsServiceManager.IsRunning();
            if (isRunning)
            {
                Logger.Error("Service is running");
                return;
            }

            Logger.Info("Service is not running, trying to uninstall it");

            _dummyWindowsServiceManager.Uninstall();

            Logger.Info("Successfully uninstalled the service");
        }
    }
}