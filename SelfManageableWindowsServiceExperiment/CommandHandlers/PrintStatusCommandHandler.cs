using System;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class PrintStatusCommandHandler : ICommandHandler
    {
        private readonly DummyWindowsServiceManager _dummyWindowsServiceManager;

        public PrintStatusCommandHandler(DummyWindowsServiceManager dummyWindowsServiceManager)
        {
            _dummyWindowsServiceManager = dummyWindowsServiceManager;
        }

        public void Handle()
        {
            var isInstalled = _dummyWindowsServiceManager.IsInstalled();
            var isRunning = isInstalled && _dummyWindowsServiceManager.IsRunning();
            Console.WriteLine("Installed: {0}", isInstalled);
            Console.WriteLine("Running: {0}", isRunning);
        }
    }
}