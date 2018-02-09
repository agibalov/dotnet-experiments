using System;
using System.Management;
using System.ServiceProcess;

namespace SelfManageableWindowsServiceExperiment.Infrastructure
{
    public class WindowsServiceManager
    {
        private readonly string _serviceName;
        private readonly string _serviceDisplayName;
        private readonly string _executablePath;

        public WindowsServiceManager(
            string serviceName, 
            string serviceDisplayName, 
            string executablePath)
        {
            _serviceName = serviceName;
            _serviceDisplayName = serviceDisplayName;
            _executablePath = executablePath;
        }

        public void Install()
        {
            var win32ServiceManagementClass = new ManagementClass("Win32_Service");
            var inParams = win32ServiceManagementClass.GetMethodParameters("Create");
            inParams["Name"] = _serviceName;
            inParams["DisplayName"] = _serviceDisplayName;
            inParams["PathName"] = _executablePath;
            inParams["StartMode"] = ServiceStartMode.Automatic.ToString();

            var outParams = win32ServiceManagementClass.InvokeMethod("Create", inParams, null);
            var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
            if (returnValue != 0)
            {
                throw new WmiException(returnValue);
            }
        }

        public void Uninstall()
        {
            using (var service = GetWin32ServiceManagementObjectByName(_serviceName))
            {
                var outParams = service.InvokeMethod("Delete", null, null);
                var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                if (returnValue != 0)
                {
                    throw new WmiException(returnValue);
                }
            }
        }

        public bool IsInstalled()
        {
            try
            {
                Interrogate(_serviceName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsRunning()
        {
            using (var service = GetWin32ServiceManagementObjectByName(_serviceName))
            {
                var serviceStateString = service.Properties["State"].Value.ToString().Trim().ToLower();
                return serviceStateString == "running";
            }
        }

        public void Start()
        {
            using (var service = GetWin32ServiceManagementObjectByName(_serviceName))
            {
                var outParams = service.InvokeMethod("StartService", null, null);
                var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                if (returnValue != 0)
                {
                    throw new WmiException(returnValue);
                }

                Interrogate(_serviceName);
            }
        }

        public void Stop()
        {
            using (var service = GetWin32ServiceManagementObjectByName(_serviceName))
            {
                var outParams = service.InvokeMethod("StopService", null, null);
                var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                if (returnValue != 0)
                {
                    throw new WmiException(returnValue);
                }

                Interrogate(_serviceName);
            }
        }

        private static ManagementObject GetWin32ServiceManagementObjectByName(string serviceName)
        {
            var servicePath = string.Format("Win32_Service.Name='{0}'", serviceName);
            var managementPath = new ManagementPath(servicePath);
            var managementObject = new ManagementObject(managementPath);
            return managementObject;
        }

        private static void Interrogate(string serviceName)
        {
            using (var service = GetWin32ServiceManagementObjectByName(serviceName))
            {
                service.InvokeMethod("InterrogateService", null, null);
            }
        }
    }
}