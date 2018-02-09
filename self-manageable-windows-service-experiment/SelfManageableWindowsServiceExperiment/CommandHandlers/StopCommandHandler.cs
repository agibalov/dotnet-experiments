using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StopCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WindowsServiceManager _windowsServiceManager;

        public StopCommandHandler(WindowsServiceManager windowsServiceManager)
        {
            _windowsServiceManager = windowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _windowsServiceManager.IsInstalled();
            if (!isInstalled)
            {
                Logger.Error("Service is not installed");
                return;
            }

            Logger.Info("Service is installed");

            var isRunning = _windowsServiceManager.IsRunning();
            if (!isRunning)
            {
                Logger.Error("Service is not running");
                return;
            }

            Logger.Info("Service is running, trying to stop it");
            _windowsServiceManager.Stop();
            Logger.Info("Successfully stopped the service");
        }
    }
}