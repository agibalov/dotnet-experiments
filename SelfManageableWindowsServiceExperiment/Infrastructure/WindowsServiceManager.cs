using System;
using System.Management;
using System.ServiceProcess;

namespace SelfManageableWindowsServiceExperiment.Infrastructure
{
    public class WindowsServiceManager
    {
        public void Install(string serviceName, string displayName, string executablePath)
        {
            var win32ServiceManagementClass = new ManagementClass("Win32_Service");
            var inParams = win32ServiceManagementClass.GetMethodParameters("Create");
            inParams["Name"] = serviceName;
            inParams["DisplayName"] = displayName;
            inParams["PathName"] = executablePath;
            inParams["StartMode"] = ServiceStartMode.Automatic.ToString();

            var outParams = win32ServiceManagementClass.InvokeMethod("Create", inParams, null);
            var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
            if (returnValue != 0)
            {
                throw new WmiException(returnValue);
            }
        }

        public void Uninstall(string serviceName)
        {
            using (var service = GetWin32ServiceManagementObjectByName(serviceName))
            {
                var outParams = service.InvokeMethod("Delete", null, null);
                var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                if (returnValue != 0)
                {
                    throw new WmiException(returnValue);
                }
            }
        }

        public bool IsInstalled(string serviceName)
        {
            try
            {
                Interrogate(serviceName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsRunning(string serviceName)
        {
            using (var service = GetWin32ServiceManagementObjectByName(serviceName))
            {
                var serviceStateString = service.Properties["State"].Value.ToString().Trim().ToLower();
                return serviceStateString == "running";
            }
        }

        public void Start(string serviceName)
        {
            using (var service = GetWin32ServiceManagementObjectByName(serviceName))
            {
                var outParams = service.InvokeMethod("StartService", null, null);
                var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                if (returnValue != 0)
                {
                    throw new WmiException(returnValue);
                }

                Interrogate(serviceName);
            }
        }

        public void Stop(string serviceName)
        {
            using (var service = GetWin32ServiceManagementObjectByName(serviceName))
            {
                var outParams = service.InvokeMethod("StopService", null, null);
                var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                if (returnValue != 0)
                {
                    throw new WmiException(returnValue);
                }

                Interrogate(serviceName);
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