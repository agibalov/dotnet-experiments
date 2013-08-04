using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class PrintStatusCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public PrintStatusCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
        {
            _dummyWindowsServiceManager = dummyWindowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _dummyWindowsServiceManager.IsInstalled();
            var isRunning = isInstalled && _dummyWindowsServiceManager.IsRunning();
            Logger.Info("Service installed: {0}", isInstalled);
            Logger.Info("Service running: {0}", isRunning);
        }
    }
}