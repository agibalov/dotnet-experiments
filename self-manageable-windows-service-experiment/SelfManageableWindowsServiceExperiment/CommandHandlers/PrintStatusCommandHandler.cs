using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class PrintStatusCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WindowsServiceManager _windowsServiceManager;

        public PrintStatusCommandHandler(WindowsServiceManager windowsServiceManager)
        {
            _windowsServiceManager = windowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _windowsServiceManager.IsInstalled();
            var isRunning = isInstalled && _windowsServiceManager.IsRunning();
            Logger.Info("Service installed: {0}", isInstalled);
            Logger.Info("Service running: {0}", isRunning);
        }
    }
}