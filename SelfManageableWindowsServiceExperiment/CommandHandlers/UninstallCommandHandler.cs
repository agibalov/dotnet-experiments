using System;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class UninstallCommandHandler : ICommandHandler
    {
        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public UninstallCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
        {
            _dummyWindowsServiceManager = dummyWindowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _dummyWindowsServiceManager.IsInstalled();
            if (!isInstalled)
            {
                Console.WriteLine("Service not installed");
                return;
            }

            var isRunning = _dummyWindowsServiceManager.IsRunning();
            if (isRunning)
            {
                Console.WriteLine("Service is running");
                return;
            }

            _dummyWindowsServiceManager.Uninstall();
        }
    }
}