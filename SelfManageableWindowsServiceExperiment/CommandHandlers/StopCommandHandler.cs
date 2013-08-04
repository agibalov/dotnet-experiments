using System;
using SelfManageableWindowsServiceExperiment.Infrastructure;

namespace SelfManageableWindowsServiceExperiment.CommandHandlers
{
    public class StopCommandHandler : ICommandHandler
    {
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
                Console.WriteLine("Service not installed");
                return;
            }

            var isRunning = _dummyWindowsServiceManager.IsRunning();
            if (!isRunning)
            {
                Console.WriteLine("Service is not running");
                return;
            }

            _dummyWindowsServiceManager.Stop();
        }
    }
}