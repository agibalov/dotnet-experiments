using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StartCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public StartCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
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
                Logger.Error("Service is already running");
                return;
            }

            Logger.Info("Trying to start the service");

            _dummyWindowsServiceManager.Start();

            Logger.Info("Successfully started the service");
        }
    }
}