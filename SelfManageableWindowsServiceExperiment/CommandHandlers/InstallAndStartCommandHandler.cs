using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class InstallAndStartCommandHandler : ICommandHandler
    {
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
                _dummyWindowsServiceManager.Install();
            }

            var isRunning = _dummyWindowsServiceManager.IsRunning();
            if (!isRunning)
            {
                _dummyWindowsServiceManager.Start();
            }
        }
    }
}