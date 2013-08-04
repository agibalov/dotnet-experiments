namespace SelfManageableWindowsServiceExperiment.Infrastructure
{
    public class DummyWindowsServiceManager
    {
        private readonly string _serviceName;
        private readonly string _serviceDisplayName;
        private readonly string _executablePath;
        private readonly WindowsServiceManager _windowsServiceManager;

        public DummyWindowsServiceManager(
            string serviceName, 
            string serviceDisplayName, 
            string executablePath,
            WindowsServiceManager windowsServiceManager)
        {
            _serviceName = serviceName;
            _serviceDisplayName = serviceDisplayName;
            _executablePath = executablePath;
            _windowsServiceManager = windowsServiceManager;
        }

        public void Install()
        {
            _windowsServiceManager.Install(
                _serviceName, 
                _serviceDisplayName, 
                _executablePath);
        }

        public void Uninstall()
        {
            _windowsServiceManager.Uninstall(_serviceName);
        }

        public void Start()
        {
            _windowsServiceManager.Start(_serviceName);
        }

        public void Stop()
        {
            _windowsServiceManager.Stop(_serviceName);
        }

        public bool IsInstalled()
        {
            return _windowsServiceManager.IsInstalled(_serviceName);
        }

        public bool IsRunning()
        {
            return _windowsServiceManager.IsRunning(_serviceName);
        }
    }
}