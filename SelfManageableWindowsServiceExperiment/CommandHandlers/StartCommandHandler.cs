using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StartCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WindowsServiceManager _windowsServiceManager;

        public StartCommandHandler(WindowsServiceManager windowsServiceManager)
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
            if (isRunning)
            {
                Logger.Error("Service is already running");
                return;
            }

            Logger.Info("Trying to start the service");
            _windowsServiceManager.Start();
            Logger.Info("Successfully started the service");
        }
    }
}