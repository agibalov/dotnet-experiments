using System;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public InstallCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
        {
            _dummyWindowsServiceManager = dummyWindowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _dummyWindowsServiceManager.IsInstalled();
            if (isInstalled)
            {
                Console.WriteLine("Service already installed");
                return;
            }

            _dummyWindowsServiceManager.Install();
        }
    }
}