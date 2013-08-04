using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StopCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public StopCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
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
            if (!isRunning)
            {
                Logger.Error("Service is not running");
                return;
            }

            Logger.Info("Service is running, trying to stop it");

            _dummyWindowsServiceManager.Stop();

            Logger.Info("Successfully stopped the service");
        }
    }
}