using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StopAndUninstallCommandHandler : ICommandHandler
    {
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
                return;
            }

            var isRunning = _dummyWindowsServiceManager.IsRunning();
            if (isRunning)
            {
                _dummyWindowsServiceManager.Stop();
            }

            _dummyWindowsServiceManager.Uninstall();
        }
    }
}