using NLog;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class InstallCommandHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WindowsServiceManager _windowsServiceManager;

        public InstallCommandHandler(WindowsServiceManager windowsServiceManager)
        {
            _windowsServiceManager = windowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _windowsServiceManager.IsInstalled();
            if (isInstalled)
            {
                Logger.Error("Service is already installed");
                return;
            }

            Logger.Info("Service is not installed");
            _windowsServiceManager.Install();
            Logger.Info("Successfully installed the service");
        }
    }
}