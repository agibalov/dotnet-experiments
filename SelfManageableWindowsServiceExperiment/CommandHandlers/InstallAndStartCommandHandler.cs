using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class InstallAndStartCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public InstallAndStartCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
        {
            _dummyWindowsServiceManager = dummyWindowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _dummyWindowsServiceManager.IsInstalled();
            if (!isInstalled)
            {
                Logger.Info("Service is not installed, trying to install");
                _dummyWindowsServiceManager.Install();
                Logger.Info("Successfully installed the service");
            }
            else
            {
                Logger.Info("Service is already installed");
            }

            var isRunning = _dummyWindowsServiceManager.IsRunning();
            if (!isRunning)
            {
                Logger.Info("Service is not running, trying to start");
                _dummyWindowsServiceManager.Start();
                Logger.Info("Successfully started the service");
            }
            else
            {
                Logger.Info("Service is already running");
            }
        }
    }
}